using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class OrderResponse
    {
        [JsonProperty("orderid")]
        public int OrderId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
