using System.Text.Json.Serialization;

namespace CardGameCorner.Models
{
    public class Game
    {
        [JsonPropertyName("game")]
        public string GameCode { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("bgImage")]
        public string BackgroundImage { get; set; }

        [JsonPropertyName("logoImage")]
        public string LogoImage { get; set; }

        [JsonPropertyName("homeBestDealsImage")]
        public string HomeBestDealsImage { get; set; }
    }
}