using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class CardSearchRequest
    {
        public string Title { get; set; }        // The title to search for
        public string Game { get; set; }        // The game name (e.g., Magic)
        public string Set { get; set; }         // The set code
        public string Lang { get; set; }        // Language filter
        public int Foil { get; set; }           // Foil filter: 0 or 1
        public int FirstEdition { get; set; }
    }
}
