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
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels
{
    public partial class MyAccountViewModel : BaseViewModel
    {
        private readonly IMyAccountService _myAccountService;
        private readonly ISecureStorage _secureStorage;
        private UserProfile _userProfile;
        private bool _isEditMode;

        public MyAccountViewModel(IMyAccountService myAccountService, ISecureStorage secureStorage)
        {
            _myAccountService = myAccountService;
            _secureStorage = secureStorage;

            EditCommand = new Command(() => IsEditMode = true);
            BackCommand = new Command(() => IsEditMode = false);
            DoneCommand = new AsyncRelayCommand(SaveProfileAsync);
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
                return UserProfile;
            }

            try
            {
                IsBusy = true;
                UserProfile = await _myAccountService.GetUserProfileAsync();
                return UserProfile;
            }
            catch (Exception ex)
            {
                // Clear profile on failure
                UserProfile = null;
                await Shell.Current.DisplayAlert("Error", "Failed to load profile", "OK");
                return UserProfile;
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

                IsBusy = true;
                var result = await _myAccountService.UpdateUserProfileAsync(UserProfile);
                if (result)
                {
                    IsEditMode = false;
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