using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Game { get; set; }
        public string Model { get; set; }
        public string ModelEn { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public string Rarity { get; set; }
        public string Category { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class ApiResponse
    {
        public List<Product> Products { get; set; }
        public int Total { get; set; }
    }
}
