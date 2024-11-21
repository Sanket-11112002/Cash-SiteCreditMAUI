using CardGameCorner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public interface IMyAccountService
    {
        Task<UserProfile> GetUserProfileAsync();
        Task<bool> UpdateUserProfileAsync(UserProfile profile);
    }
}
