using CardGameCorner.Models;
using CardGameCorner.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

public class MyAccountViewModel : ObservableObject
{
    private readonly IMyAccountService _myAccountService;
    private UserProfile _userProfile;
    private bool _isEditMode;

    public ICommand EditCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand DoneCommand { get; }

    public MyAccountViewModel(IMyAccountService myAccountService)
    {
        _myAccountService = myAccountService;
        EditCommand = new Command(() => IsEditMode = true);
        BackCommand = new Command(() => IsEditMode = false);
        DoneCommand = new AsyncRelayCommand(SaveProfileAsync);

        Task.Run(LoadProfileAsync);
    }

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

    private async Task LoadProfileAsync()
    {
        try
        {
            IsBusy = true;
            UserProfile = await _myAccountService.GetUserProfileAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to load profile", "OK");
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
            IsBusy = true;
            var result = await _myAccountService.UpdateUserProfileAsync(UserProfile);
            if (result)
            {
                IsEditMode = false;
                await Shell.Current.DisplayAlert("Success", "Profile updated successfully", "OK");
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

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }
}
