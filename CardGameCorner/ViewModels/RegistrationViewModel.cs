using CardGameCorner.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardGameCorner.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly string apiUrl = "https://api.magiccorner.it/api/myaccount/register";
        private readonly string bearerToken = "0d1bb073-9dfb-4c6d-a1c0-1e8f7d5d8e9f";

        // Properties for form inputs
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string VatNumber { get; set; }
        public string FiscalCode { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Language { get; set; } = "en";

        // Error message
        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        // Command for registration
        public ICommand RegisterCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        //public RegistrationViewModel()
        //{
        //    RegisterCommand = new Command(async () => await RegisterAsync());
        //}

        public RegistrationViewModel()
        {
            SubmitCommand = new Command(async () => await RegisterAsync());
            CancelCommand = new Command(async () => await NavigateToLoginPageAsync());
        }

        private async Task RegisterAsync()
        {
            ErrorMessage = string.Empty;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            var registrationData = new
            {
                email = Email,
                password = Password,
                name = Name,
                lastName = LastName,
                company = Company,
                vatNumber = VatNumber,
                fiscalCode = FiscalCode,
                address = Address,
                zip = Zip,
                province = Province,
                city = City,
                country = Country,
                phone = Phone,
                UIc = Language
            };

            var jsonContent = JsonConvert.SerializeObject(registrationData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RegistrationResponse>(responseData);

                    // Check if JWT token is present in the response, indicating successful registration
                    if (!string.IsNullOrEmpty(result?.Token))
                    {
                        // Registration successful, navigate to login page
                        await NavigateToLoginPageAsync();
                    }
                    else
                    {
                        ErrorMessage = "Registration failed: Token not received";
                    }
                }
                else
                {
                    // Handle failed registration or duplicate user message
                    ErrorMessage = "Registration failed: " + await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
            }
        }
        private async Task NavigateToLoginPageAsync()
        {
            // Navigate to LoginPage
            await Shell.Current.GoToAsync("..");
        }
    }
}
