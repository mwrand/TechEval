using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BoostLingo
{
    [DataContract]
    public class UserResponse
    {
        [DataMember(Name="bio")]
        public string Bio { get; set; }
        
        [DataMember(Name="id")]
        public string Id { get; set; }
        
        [DataMember(Name="language")]
        public string Language { get; set; }
        
        [DataMember(Name="name")]
        public string Name {get;set;}
        
        [DataMember(Name="version")]
        public string Version { get; set; }
    }
    
    [DataContract]
    public class User
    {
        [DataMember(Name="bio")]
        public string Bio { get; set; }
        
        [DataMember(Name="id")]
        public string Id { get; set; }
        
        [DataMember(Name="language")]
        public string Language { get; set; }
        
        [DataMember(Name="firstName")]
        public string FirstName {get;set;}
        
        [DataMember(Name = "lastName")] 
        public string LastName { get; set; }
        
        [DataMember(Name="version")]
        public string Version { get; set; }
    }
}