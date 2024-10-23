using Newtonsoft.Json;

namespace Garagato.MVC.Models
{
    public class GoogleUserData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
