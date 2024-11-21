using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public static class GameNavigationService
    {
        public static async Task ShowGameDetailsPageAsync(string gameCode)
        {
            if (string.IsNullOrEmpty(gameCode))
            {
                throw new ArgumentNullException(nameof(gameCode));
            }

            try
            {
                var route = $"GameDetailsPage?gameCode={Uri.EscapeDataString(gameCode)}&uiCode={Uri.EscapeDataString("en")}";

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (Shell.Current != null)
                    {
                        await Shell.Current.GoToAsync(route);
                    }
                    else
                    {
                        throw new InvalidOperationException("Shell.Current is null");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
                throw new NavigationException("Failed to navigate to game details page.", ex);
            }
        }
    }

    public class NavigationException : Exception
    {
        public NavigationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}