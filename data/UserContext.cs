using Microsoft.EntityFrameworkCore;
namespace BoostLingo
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public UserContext() {}
        public UserContext(DbContextOptions < UserContext > options): base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "UsersDb");
        }
    }
}