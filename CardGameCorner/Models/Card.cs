using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class Card
    {
        public ImageSource Image { get; set; }
        public string ImageUrl { get; set; }
        public string Label { get; set; }
        public string Note { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public string ProductUrl { get; set; } // URL of the product

    }
}
