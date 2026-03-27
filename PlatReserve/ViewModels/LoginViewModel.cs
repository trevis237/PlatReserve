using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlatReserve.Models;
using PlatReserve.Services;
using PlatReserve.Views;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace PlatReserve.ViewModels
{
    // ViewModels/LoginViewModel.cs
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IRealmService _realmService;
        private readonly AuthService _authService;

        //[ObservableProperty] private string _phone;
        [ObservableProperty] public partial string Phone { get; set; }
        [ObservableProperty] private string _password;

        public LoginViewModel(IRealmService realmService, AuthService authService)
        {
            _realmService = realmService;
            _authService = authService;
        }

        [RelayCommand]
        public async Task Login()
        {
            var realm = _realmService.GetRealm();
            var user = realm.All<Personne>().FirstOrDefault(p => p.Telephone == Phone && p.MotDePasse == Password);

            if (user != null)
            {
                _authService.User = user;
                if (user.Role == UserRole.Admin)
                    await Shell.Current.GoToAsync(nameof(AdminPage));
                else
                    await Shell.Current.GoToAsync(nameof(VoyageurDashboardPage));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlertAsync("Erreur", "Identifiants incorrects", "OK");
            }
        }
    }
}
