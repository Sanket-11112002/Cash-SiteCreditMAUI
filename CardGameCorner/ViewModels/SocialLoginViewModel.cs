using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SocialLoginWithMaui.Messages;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CardGameCorner.ViewModels
{
    public partial class SocialLoginViewModel : BaseViewModel
    {
        // Base auth URL for your services
        private readonly Dictionary<string, string> authUrls = new Dictionary<string, string>
        {
            { "Google", "https://api.magiccorner.it/api/auth/mobile/Google" },
            { "Facebook", "https://api.magiccorner.it/api/auth/mobile/Facebook" }
        };

        [ObservableProperty]
        public string? authToken;

        [ObservableProperty]
        public string? email;  // Changed from userEmail to email

        public SocialLoginViewModel()
        {
            WeakReferenceMessenger.Default.Register<ProtocolMessage>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }

        [RelayCommand]
        private async Task OnAuthenticate(string scheme)
        {
            try
            {
                AuthToken = string.Empty;
                Email = string.Empty;  // Changed from UserEmail to Email

                if (!authUrls.TryGetValue(scheme, out string? authUrl))
                {
                    await App.Current.MainPage.DisplayAlert("Error", $"Authentication URL not configured for {scheme}", "OK");
                    return;
                }
                if (scheme == "Google")
                {
                    await Browser.OpenAsync("https://accounts.google.com/Logout", BrowserLaunchMode.SystemPreferred);
                    await Task.Delay(1000); // Slight delay to ensure logout is processed
                }

                // Web Authentication flow
                var callbackUrl = new Uri("mcbuylist://");

                var result = await WebAuthenticator.Default.AuthenticateAsync(
                   new WebAuthenticatorOptions()
                   {
                       Url = new Uri(authUrl),
                       CallbackUrl = callbackUrl,
                       PrefersEphemeralWebBrowserSession = true
                   });

                if (result != null)
                {
                    OnMessageReceived(result.CallbackUri.OriginalString);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Alert", $"Failed: {ex.Message}", "OK");
            }
        }

        private void OnMessageReceived(string value)
        {
            try
            {
                Console.WriteLine($"New message received: {value}");

                Uri uri = new Uri(value);
                string query = uri.Fragment;
                query = query.TrimStart('#');
                NameValueCollection queryParameters = HttpUtility.ParseQueryString(query);

                // Extract parameters based on your specific callback format
                string accessToken = queryParameters["access_token"] ?? string.Empty;
                string emailValue = queryParameters["email"] ?? string.Empty;

                // Decode the URL-encoded email
                string decodedEmail = HttpUtility.UrlDecode(emailValue);

                Email = decodedEmail;  // Changed from UserEmail to Email
                AuthToken = accessToken;

                // You can store these values securely for later use
                SecureStorage.Default.SetAsync("access_token", accessToken);
                SecureStorage.Default.SetAsync("user_email", decodedEmail);

                // Display the token and email
                MainThread.BeginInvokeOnMainThread(() => {
                    AuthToken = $"Email: {decodedEmail}{Environment.NewLine}AccessToken: {accessToken}";
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing callback: {ex.Message}");
                MainThread.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Error", $"Failed to process login: {ex.Message}", "OK");
                });
            }
        }
    }
}
