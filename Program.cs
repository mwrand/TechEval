using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BoostLingo
{
    class Program
    {
        private static string _jsonFilePath = "";
        
        static async Task Main(string[] args)
        {
            SetConfigValues();
            while (AskToRun().Result)
            {}
        }

        private static async Task<bool> AskToRun()
        {
            var userResponse = await GetUsersFromWeb();
            if (userResponse.Count > 0)
            {
                var users = ConvertUsersResponseToUsers(userResponse);
                var usersSaved = SaveUsersToDB(users);
                if (usersSaved.Result) LoadUsersFromDB();
            }

            return WantToImportAgain();
        }

        static List<User> ConvertUsersResponseToUsers(List<UserResponse> userResponse)
        {
            var users = new List<User>();
                
            foreach (var up in userResponse)
            {
                var nameSplit = up.Name.Split(' ', 2);
                
                var user = new User()
                { 
                    Bio = up.Bio,
                    Id = up.Id,
                    FirstName = nameSplit[0],
                    LastName = nameSplit[1],
                    Language = up.Language,
                    Version = up.Version
                };
                
                users.Add(user);
            }

            return users;
        }

        static async Task<string> GetFile()
        {
            var json = string.Empty;
            
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(_jsonFilePath);
                var contents = await response.Content.ReadAsStringAsync();

                return contents;
            }
            catch
            {
                Console.WriteLine("The User site appears to be down.  Please check try again later.");
            }
           
            return json;
        }

        static async Task<List<UserResponse>> GetUsersFromWeb()
        {
            var usersJson = await GetFile();
            var users = new List<UserResponse>();

            if (!String.IsNullOrEmpty(usersJson))
            {
                try
                {
                    var response = JsonConvert.DeserializeObject<List<UserResponse>>(usersJson);
                    if (response != null) users = response;
                }
                catch
                {
                    Console.WriteLine("The 3rd party user data file is corrupted.");
                }
            }
            
            if (users.Count == 0)
                Console.WriteLine("There are no users to import at this time.");

            return users;
        }

        static void LoadUsersFromDB()
        {
            var context = new UserContext();
            var users = context.Users
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName);
            
            Console.WriteLine("LastName" + ", " + "FirstName" + " | " + "Language" + " | " + "Bio" + " | " + "Version");

            foreach (var user in users)
            {
                Console.WriteLine(user.LastName + ", " + user.LastName + " | " + user.Language + " | " + user.Bio.Substring(0, 20) + " | " + user.Version);
            }
        }

        static async Task<bool> SaveUsersToDB(List<User> users)
        {
            var savedSuccess = false;
            var context = new UserContext();

            foreach (var user in users)
                context.Users.Add(user);

            try
            {
                await context.SaveChangesAsync();
                savedSuccess = true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("The records has already been inserted.  Please try again when there are new users.");
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error save the new users to the database.  Please try again.");
            }

            return savedSuccess;
        }

        static void SetConfigValues()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true);

            var config = builder.Build();
            _jsonFilePath = config["ThirdPartyResources:JsonFilePath"];
        }

        static bool WantToImportAgain()
        {
            //Spacing and timing for 
            System.Threading.Thread.Sleep(1000);
            Console.Out.WriteLine(" ");
            Console.Out.WriteLine(" ");
            System.Threading.Thread.Sleep(2000);
            
            Console.Out.WriteLine("Would you like to import more users? [Y/N]");
            
            var key = Console.ReadKey();
            return key.Key != ConsoleKey.N;
        }
    }
}