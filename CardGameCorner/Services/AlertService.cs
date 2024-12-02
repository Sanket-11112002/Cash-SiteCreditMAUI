using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public class AlertService : IAlertService
    {
        public async Task<bool> ShowConfirmationAsync(string title, string message, string acceptText = "Yes", string cancelText = "No")
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, acceptText, cancelText);
        }

        public async Task ShowAlertAsync(string title, string message, string cancelText = "OK")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancelText);
        }
    }
}
