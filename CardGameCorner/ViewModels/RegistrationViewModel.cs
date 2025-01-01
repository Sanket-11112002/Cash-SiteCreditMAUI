using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace CardGameCorner.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
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

        public string Language { get; set; } = "English";

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


        [ObservableProperty]
        private string pEmail;

        [ObservableProperty]
        private string pPassword;

        [ObservableProperty]
        private string pName;

        [ObservableProperty]
        private string pLastName;

        [ObservableProperty]
        private string pCompany;

        [ObservableProperty]
        private string pVatNumber;

        [ObservableProperty]
        private string pFiscal;

        [ObservableProperty]
        private string pAddress;

        [ObservableProperty]
        private string pZip;

        [ObservableProperty]
        private string pProvince;

        [ObservableProperty]
        private string pCity;

        [ObservableProperty]
        private string pCountry;

        [ObservableProperty]
        private string pPhone;

        [ObservableProperty]
        private string pLanguage;

        [ObservableProperty]
        private string pTitle;

        [ObservableProperty]
        private string pSubmit;

        [ObservableProperty]
        private string pCancel;

        [ObservableProperty]
        private string errormsg;

        private bool isInvalidEmail;

        //public RegistrationViewModel()
        //{
        //    RegisterCommand = new Command(async () => await RegisterAsync());
        //}

        public RegistrationViewModel()
        {
            // Initialize with current language
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            SubmitCommand = new Command(async () => await RegisterAsync());
            CancelCommand = new Command(async () => await NavigateToLoginPageAsync());
        }
        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                // Update localized strings when language changes
                UpdateLocalizedStrings();


            }
        }

        private void UpdateLocalizedStrings()
        {
            // Ensure these are called on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                PEmail = AppResources.REmail;
                PPassword = AppResources.RPlaceholder;
                PName = AppResources.Name;
                PLastName = AppResources.Last_Name;
                PCompany = AppResources.Company;
                PVatNumber = AppResources.VAT_Number;
                PFiscal = AppResources.Fiscal_Code;
                PAddress = AppResources.Address;
                PZip = AppResources.ZIP;
                PProvince = AppResources.Province;
                PCity = AppResources.City;
                PCountry = AppResources.Country;
                PPhone = AppResources.Phone;
                PLanguage = AppResources.Language;
                PTitle = AppResources.Register;
                PSubmit = AppResources.Submit;
                PCancel = AppResources.Cancel;
                // Errormsg = AppResources.ValidEmail;

                if (isInvalidEmail)
                {
                    ErrorMessage = AppResources.ValidEmail;
                }
                else
                {
                    ErrorMessage = string.Empty;
                }

                OnPropertyChanged(nameof(ErrorMessage));


                // Trigger property changed events to update UI
                OnPropertyChanged(nameof(PName));
                OnPropertyChanged(nameof(PEmail));
                OnPropertyChanged(nameof(PPassword));
                OnPropertyChanged(nameof(PLastName));
                OnPropertyChanged(nameof(PCompany));
                OnPropertyChanged(nameof(PVatNumber));
                OnPropertyChanged(nameof(PFiscal));
                OnPropertyChanged(nameof(PAddress));
                OnPropertyChanged(nameof(PCity));
                OnPropertyChanged(nameof(PZip));
                OnPropertyChanged(nameof(PProvince));
                OnPropertyChanged(nameof(PCountry));
                OnPropertyChanged(nameof(PPhone));
                OnPropertyChanged(nameof(PLanguage));
                OnPropertyChanged(nameof(PTitle));
                OnPropertyChanged(nameof(PSubmit));
                OnPropertyChanged(nameof(PCancel));
                // OnPropertyChanged(nameof(Errormsg));
            });
        }

        // Ensure the PropertyChanged event is properly implemented
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private async Task RegisterAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = AppResources.FillRequiredFieldsMessage; // Localized message for both fields empty
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = AppResources.EmailRequiredMessage;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = AppResources.PasswordRequiredMessage;
                return;
            }

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            if (!IsValidEmail(Email))
            {
                isInvalidEmail = true; // Set error state
                ErrorMessage = AppResources.ValidEmail;
                OnPropertyChanged(nameof(ErrorMessage));
                return;
            }
            else
            {
                isInvalidEmail = false; // Reset error state
            }

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
                UIc = GetLanguageCode()
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
                        ErrorMessage = AppResources.RegistrationFailedMessage;
                    }
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        // Deserialize the response content to handle structured errors
                        var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseContent);

                        if (errorResponse != null)
                        {
                            // Handle specific password validation errors
                            if (errorResponse.ContainsKey("PasswordTooShort"))
                            {
                                ErrorMessage = AppResources.PasswordTooShortMessage;
                            }
                            else if (errorResponse.ContainsKey("PasswordRequiresDigit"))
                            {
                                ErrorMessage = AppResources.PasswordRequiresDigitMessage;
                            }
                            else if (errorResponse.ContainsKey("PasswordRequiresLower"))
                            {
                                ErrorMessage = AppResources.PasswordRequiresLowerMessage;
                            }
                            else if (errorResponse.ContainsKey("PasswordRequiresUpper"))
                            {
                                ErrorMessage = AppResources.PasswordRequiresUpperMessage;
                            }
                            else if (errorResponse.ContainsKey("PasswordRequiresUniqueChars"))
                            {
                                ErrorMessage = AppResources.PasswordRequiresUniqueCharsMessage;
                            }
                            else if (errorResponse.ContainsKey("PasswordRequiresNonAlphanumeric"))
                            {
                                ErrorMessage = AppResources.PasswordRequiresNonAlphanumericMessage;
                            }
                            else if (errorResponse.ContainsKey("DuplicateUserName"))
                            {
                                // Handle duplicate username error
                                ErrorMessage = AppResources.DuplicateUserNameMessage;
                            }
                            else
                            {
                                // Default error message if no specific key exists
                                ErrorMessage = AppResources.RegistrationFailedMessage;
                            }
                        }
                    }
                    catch
                    {
                        // If parsing fails, fallback to default error
                        ErrorMessage = AppResources.RegistrationFailedMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorMessage = "An error occurred: " + ex.Message;
                ErrorMessage = AppResources.RegistrationFailedMessage;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex to match standard email format
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
        }

        private async Task NavigateToLoginPageAsync()
        {
            // Navigate to LoginPage
            await Shell.Current.GoToAsync("..");
        }

        public string GetLanguageCode()
        {
            if (Language == "English")
            {
                return "en";
            }
            else if (Language == "Italian")
            {
                return "it";
            }
            return "en";  // Default to English if not set
        }

    }
}
