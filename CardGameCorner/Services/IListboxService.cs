using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameCorner.Models;

namespace CardGameCorner.Services
{
    public interface IListboxService
    {
        public  Task<List<LanguageModal>> GetLanguagesAsync();
    }
}
