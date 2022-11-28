using Newtonsoft.Json;

namespace PartnerWebApp
{
    [JsonObject]
    public class UserDetail
    {
        [JsonProperty]
        public string? UserName { get;set; }
        [JsonProperty]
        public string? Password { get;set; } 
    }
}
