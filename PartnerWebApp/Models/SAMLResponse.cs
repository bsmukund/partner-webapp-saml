using Newtonsoft.Json;

namespace PartnerWebApp.Model
{
    [Serializable]
    [JsonObject]
    public class SAMLResponse
    {
        public string? samlresponse { get; set; }
        public bool? validationstatus { get; set; }
    }
}
