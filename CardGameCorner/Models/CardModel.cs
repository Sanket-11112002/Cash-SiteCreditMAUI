using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
        public class CardModel
        {
            public string Name { get; set; }
            public string Rarity { get; set; }
            public string Category { get; set; }
            public string ImageUrl { get; set; }
            public decimal CashPrice { get; set; }
            public decimal SiteCredit { get; set; }
            public bool IsFirstEdition { get; set; }
            public bool IsReverse { get; set; }
        }
}
