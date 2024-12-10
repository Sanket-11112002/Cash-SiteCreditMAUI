//using CardGameCorner.Models;
//using CardGameCorner.Services;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;

//public class MyAccountViewModel : ObservableObject
//{
//    private readonly IMyAccountService _myAccountService;
//    private UserProfile _userProfile;
//    private bool _isEditMode;

//    public ICommand EditCommand { get; }
//    public ICommand BackCommand { get; }
//    public ICommand DoneCommand { get; }

//    public MyAccountViewModel(IMyAccountService myAccountService)
//    {
//        _myAccountService = myAccountService;
//        EditCommand = new Command(() => IsEditMode = true);
//        BackCommand = new Command(() => IsEditMode = false);
//        DoneCommand = new AsyncRelayCommand(SaveProfileAsync);

//        Task.Run(LoadProfileAsync);
//    }

//    public bool IsEditMode
//    {
//        get => _isEditMode;
//        set => SetProperty(ref _isEditMode, value);
//    }

//    public UserProfile UserProfile
//    {
//        get => _userProfile;
//        set => SetProperty(ref _userProfile, value);
//    }

//    private async Task LoadProfileAsync()
//    {
//        try
//        {
//            IsBusy = true;
//            UserProfile = await _myAccountService.GetUserProfileAsync();
//        }
//        catch (Exception ex)
//        {
//            await Shell.Current.DisplayAlert("Error", "Failed to load profile", "OK");
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }

//    private async Task SaveProfileAsync()
//    {
//        try
//        {
//            IsBusy = true;
//            var result = await _myAccountService.UpdateUserProfileAsync(UserProfile);
//            if (result)
//            {
//                IsEditMode = false;
//                await Shell.Current.DisplayAlert("Success", "Profile updated successfully", "OK");
//            }
//        }
//        catch (Exception ex)
//        {
//            await Shell.Current.DisplayAlert("Error", "Failed to update profile", "OK");
//        }
//        finally
//        {
//            IsBusy = false;
//        }
//    }

//    private bool _isBusy;
//    public bool IsBusy
//    {
//        get => _isBusy;
//        set => SetProperty(ref _isBusy, value);
//    }
//}


using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels
{
    public partial class MyAccountViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly IMyAccountService _myAccountService;
        private readonly ISecureStorage _secureStorage;
        private UserProfile _userProfile;
        private bool _isEditMode;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string lastName;

        [ObservableProperty]
        private string company;

        [ObservableProperty]
        private string vatNumber;

        [ObservableProperty]
        private string fiscalCode;

        [ObservableProperty]
        private string phone;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private string zip;

        [ObservableProperty]
        private string province;

        [ObservableProperty]
        private string city;

        [ObservableProperty]
        private string country;


        [ObservableProperty]
        private string editButtonText;

        [ObservableProperty]
        private string backButtonText;

        [ObservableProperty]
        private string doneButtonText;

        [ObservableProperty]
        private string loginRequiredTitle;

        [ObservableProperty]
        private string loginRequiredMessage;

        [ObservableProperty]
        private string loginText;

        [ObservableProperty]
        private string continueText;

        public MyAccountViewModel(IMyAccountService myAccountService, ISecureStorage secureStorage)
        {
            UpdateLocalizedStrings();

            // Subscribe to language change events
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;

            _myAccountService = myAccountService;
            _secureStorage = secureStorage;

            // Modify EditCommand to explicitly set IsEditMode
            EditCommand = new Command(() =>
            {
                IsEditMode = true;
                // Trigger property changed to update toolbar
                OnPropertyChanged(nameof(IsEditMode));
            });

            BackCommand = new Command(() =>
            {
                IsEditMode = false;
                // Trigger property changed to update toolbar
                OnPropertyChanged(nameof(IsEditMode));
            });

            DoneCommand = new AsyncRelayCommand(SaveProfileAsync);
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
                Email = AppResources.Email; 
                Name = AppResources.Name; 
                LastName = AppResources.Last_Name;
                Company = AppResources.Company; 
                VatNumber = AppResources.VAT_Number; 
                FiscalCode = AppResources.Fiscal_Code;
                Phone = AppResources.Phone; 
                Address = AppResources.Address;
                Zip = AppResources.ZIP;
                Province = AppResources.Province; 
                City = AppResources.City; 
                Country = AppResources.Country;

                EditButtonText = AppResources.Edit;
                BackButtonText = AppResources.Back;
                DoneButtonText = AppResources.Done;
                LoginRequiredTitle = AppResources.LoginRequiredTitle;
                LoginRequiredMessage = AppResources.LoginRequiredMessage;
                LoginText = AppResources.Login;
                ContinueText = AppResources.Continue;

                // Trigger property changed events to update UI
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(Company));
                OnPropertyChanged(nameof(VatNumber));
                OnPropertyChanged(nameof(FiscalCode));
                OnPropertyChanged(nameof(Phone));
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(Zip));
                OnPropertyChanged(nameof(Province));
                OnPropertyChanged(nameof(City));
                OnPropertyChanged(nameof(Country));

                OnPropertyChanged(nameof(EditButtonText));
                OnPropertyChanged(nameof(BackButtonText));
                OnPropertyChanged(nameof(DoneButtonText));
                OnPropertyChanged(nameof(LoginRequiredTitle));
                OnPropertyChanged(nameof(LoginRequiredMessage));
                //OnPropertyChanged(nameof(ContinueText));
                //OnPropertyChanged(nameof(LoginText));
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand EditCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand DoneCommand { get; }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public UserProfile UserProfile
        {
            get => _userProfile;
            set => SetProperty(ref _userProfile, value);
        }

        public async Task<UserProfile> InitializeAsync()
        {
            // Check if user is logged in before loading profile
            var token = await _secureStorage.GetAsync("jwt_token");
            if (string.IsNullOrEmpty(token))
            {
                // Clear any existing profile data
                UserProfile = null;
                return null;
            }

            try
            {
                IsBusy = true;

                // Fetch the user profile
                UserProfile = await _myAccountService.GetUserProfileAsync();

                // Populate the observable properties with user profile data
                //if (UserProfile != null)
                //{
                //    Email = UserProfile.Email;
                //    Name = UserProfile.Name;
                //    LastName = UserProfile.LastName;
                //    Company = UserProfile.Company;
                //    VatNumber = UserProfile.VatNumber;
                //    FiscalCode = UserProfile.FiscalCode;
                //    Phone = UserProfile.Phone;
                //    Address = UserProfile.Address;
                //    Zip = UserProfile.Zip;
                //    Province = UserProfile.Province;
                //    City = UserProfile.City;
                //    Country = UserProfile.Country;
                //}

                return UserProfile;
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                UserProfile = null;
                await Shell.Current.DisplayAlert("Error", "Failed to load profile", "OK");
                return null;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveProfileAsync()
        {
            try
            {
                if (UserProfile == null)
                {
                    await Shell.Current.DisplayAlert("Error", "No profile to update", "OK");
                    return;
                }

                // Update UserProfile with current values
                UserProfile.Email = Email;
                UserProfile.Name = Name;
                UserProfile.LastName = LastName;
                UserProfile.Company = Company;
                UserProfile.VatNumber = VatNumber;
                UserProfile.FiscalCode = FiscalCode;
                UserProfile.Phone = Phone;
                UserProfile.Address = Address;
                UserProfile.Zip = Zip;
                UserProfile.Province = Province;
                UserProfile.City = City;
                UserProfile.Country = Country;

                IsBusy = true;
                var result = await _myAccountService.UpdateUserProfileAsync(UserProfile);
                if (result)
                {
                    IsEditMode = false;
                    OnPropertyChanged(nameof(IsEditMode));
                    await Shell.Current.DisplayAlert("Success", "Profile updated successfully", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update profile", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to update profile", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}