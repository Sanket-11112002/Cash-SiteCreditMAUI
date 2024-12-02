using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
     public interface IAlertService
     {
            Task<bool> ShowConfirmationAsync(string title, string message, string acceptText = "Yes", string cancelText = "No");
            Task ShowAlertAsync(string title, string message, string cancelText = "OK");
     }
}
