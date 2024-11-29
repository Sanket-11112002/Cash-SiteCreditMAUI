using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class CardItem
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Language { get; set; }
        public string Edition { get; set; }
        public string Rarity { get; set; }
        public decimal Cash { get; set; }
        public int Quantity { get; set; }
        public decimal SiteCredit { get; set; }
    }
    //public string CardNumber { get; set; }
    //public string Condition { get; set; }
}
