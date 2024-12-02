//using Microsoft.Maui.Controls;
//using System;
//using System.Threading.Tasks;

//namespace CardGameCorner.Services
//{
//    public static class GameNavigationService
//    {
//        public static async Task ShowGameDetailsPageAsync(string gameCode)
//        {
//            if (string.IsNullOrEmpty(gameCode))
//            {
//                throw new ArgumentNullException(nameof(gameCode));
//            }

//            try
//            {
//                var route = $"GameDetailsPage?gameCode={Uri.EscapeDataString(gameCode)}&uiCode={Uri.EscapeDataString("en")}";

//                await MainThread.InvokeOnMainThreadAsync(async () =>
//                {
//                    if (Shell.Current != null)
//                    {
//                        await Shell.Current.GoToAsync(route);
//                    }
//                    else
//                    {
//                        throw new InvalidOperationException("Shell.Current is null");
//                    }
//                });
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
//                throw new NavigationException("Failed to navigate to game details page.", ex);
//            }
//        }
//    }

//    public class NavigationException : Exception
//    {
//        public NavigationException(string message, Exception innerException)
//            : base(message, innerException)
//        {
//        }
//    }
//}

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
                        throw new InvalidOperationException("Shell.Current is null. Cannot navigate.");
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

    //public static class GameNavigationService
    //{
    //    private static readonly IServiceProvider _serviceProvider = Application.Current.Handler.MauiContext.Services;

    //    public static async Task ShowGameDetailsPageAsync(string gameCode)
    //    {
    //        if (string.IsNullOrEmpty(gameCode))
    //        {
    //            throw new ArgumentNullException(nameof(gameCode));
    //        }

    //        try
    //        {
    //            var secureStorage = _serviceProvider.GetService<ISecureStorage>();

    //            if (secureStorage == null)
    //            {
    //                throw new InvalidOperationException("ISecureStorage implementation not found in services.");
    //            }

    //            var gameDetailsPage = new Views.GameDetailsPage(secureStorage)
    //            {
    //                GameCode = gameCode,
    //                UiCode = "en"
    //            };

    //            await MainThread.InvokeOnMainThreadAsync(async () =>
    //            {
    //                var navigation = Application.Current.MainPage?.Navigation;
    //                if (navigation != null)
    //                {
    //                    await navigation.PushAsync(gameDetailsPage);
    //                }
    //                else
    //                {
    //                    throw new InvalidOperationException("Navigation stack is null. Cannot navigate.");
    //                }
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
    //            throw new NavigationException("Failed to navigate to game details page.", ex);
    //        }
    //    }
    //}
    public class NavigationException : Exception
    {
        public NavigationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}