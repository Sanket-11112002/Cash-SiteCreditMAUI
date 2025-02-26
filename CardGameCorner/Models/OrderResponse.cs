using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class OrderListResponse
    {
        [JsonProperty("orders")]
        public List<OrderResponse> Orders { get; set; }
    }

    public class OrderResponse
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
