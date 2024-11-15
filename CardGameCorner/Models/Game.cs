using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class Game
    {
        [JsonPropertyName("game")]
        public string GameCode { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    }
}
