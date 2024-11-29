using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class ProductList
    {
        public string Id { get; set; }
        public string Game { get; set; }
        public string Model { get; set; }
        public string ModelEn { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public string Rarity { get; set; }
        public string Category { get; set; }
        public decimal Buylist { get; set; }
        public decimal Sitecredit { get; set; }
        public int Quantity { get; set; }
        public string Language { get; set; }
        public string Condition { get; set; }
    }
}
