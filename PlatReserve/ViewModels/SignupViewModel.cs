using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlatReserve.Models;
using PlatReserve.Services;
using PlatReserve.Views;
using System.Reactive.Linq;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace PlatReserve.ViewModels;

public partial class SignupViewModel : ObservableObject
{
    private readonly IRealmService _realmService;
    private readonly AuthService _authService;

    [ObservableProperty] private string _nom;
    [ObservableProperty] private string _phone;
    [ObservableProperty] private string _password;

    public SignupViewModel(IRealmService realmService, AuthService authService)
    {
        _realmService = realmService;
        _authService = authService;

        //CreateDefaultAdmin();
    }

    [RelayCommand]
    public async Task AllerALaPageInscription()
    {
        // C'est ICI qu'on ouvre la page d'inscription
        await Shell.Current.GoToAsync(nameof(SignupPage));
    }

    [RelayCommand]
    public async Task Signup()
    {
        var realm = _realmService.GetRealm();

        // 1. VALIDATION : Champs vides ?
        if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlertAsync("Erreur", "Veuillez remplir TOUS les champs", "OK");
            return;
        }

        // 2. VALIDATION : Le numéro existe-t-il déjà ?
        // On cherche si quelqu'un a déjà ce numéro dans la base
        var utilisateurExistant = realm.All<Personne>().FirstOrDefault(p => p.Telephone == Phone);

        if (utilisateurExistant != null)
        {
            await Application.Current.MainPage.DisplayAlertAsync("Erreur", "Ce numéro de téléphone est déjà utilisé par un autre compte.", "OK");
            return; // On arrête tout ici !
        }

        // 3. ENREGISTREMENT
        Personne newUser = null;
        await realm.WriteAsync(() => {
            newUser = new Personne
            {
                Nom = Nom,
                Telephone = Phone,
                MotDePasse = Password,
                Role = UserRole.Voyageur // Un nouvel inscrit est TOUJOURS un voyageur
            };
            realm.Add(newUser);
        });

        // 4. CONNEXION AUTOMATIQUE
        _authService.User = newUser;

        // 5. NAVIGATION
        // Après une inscription, on est forcément un voyageur, donc on va au Dashboard Voyageur
        await Shell.Current.GoToAsync($"{nameof(VoyageurDashboardPage)}");
    }

    [RelayCommand] // N'oublie pas l'attribut pour que le bouton XAML puisse le voir !
    public async Task Login()
    {

         //Phone = "657341358"; 
         //Password = "1234";
        var realm = _realmService.GetRealm();

        // On nettoie les entrées (Trim) pour éviter les espaces invisibles
        //string phoneNettoye = Phone?.Trim();
        //string passNettoye = Password?.Trim();

        // On cherche l'utilisateur avec son téléphone ET son mot de passe
        var user = realm.All<Personne>().FirstOrDefault(p => p.Telephone == Phone && p.MotDePasse == Password);

        if (user != null)
        {
            _authService.User = user;

            // On vérifie le rôle pour savoir où envoyer l'utilisateur
            if (user.RoleValue == (int)UserRole.Admin)
            {
               
                // Navigation vers l'administration (On utilise // pour réinitialiser la pile)
                await Shell.Current.GoToAsync(nameof(AdminPage));
            }
            else
            {
                // Navigation vers le dashboard voyageur
                await Shell.Current.GoToAsync(nameof(VoyageurDashboardPage));
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlertAsync("Diagnostic",
             $"Base de données vérifiée. Aucun utilisateur trouvé avec Tel: {Phone} et Pass: {Password}", "OK");
        }
    }

    // Cette méthode est appelée automatiquement par le Toolkit dès que tu tapes une lettre
    partial void OnPhoneChanged(string value)
    {
        // On écrit dans la console de sortie de Visual Studio
        System.Diagnostics.Debug.WriteLine($"[TRACE] Téléphone en cours de saisie : {value}");
    }

    partial void OnPasswordChanged(string value)
    {
        System.Diagnostics.Debug.WriteLine($"[TRACE] Mot de passe en cours de saisie : {value}");
    }


}