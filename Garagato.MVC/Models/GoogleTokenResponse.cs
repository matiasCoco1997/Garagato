using Newtonsoft.Json;

namespace Garagato.MVC.Models
{
    public class GoogleTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccesToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        // Puedes agregar otros campos que necesites
    }
}
