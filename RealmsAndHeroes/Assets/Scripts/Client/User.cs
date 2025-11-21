using Newtonsoft.Json;

namespace Client
{
    public class User
    {
        [JsonProperty("account_id")]
        public string Id { get; set; }
        
        public string Username { get; set; }
        public string Email { get; set; }
    }
}