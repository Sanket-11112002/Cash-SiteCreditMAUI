using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public interface INavigationService
    {
        Task NavigateToLoginAsync();
        Task NavigateToHomeAsync();
        Task LogoutAsync();
    }
}
