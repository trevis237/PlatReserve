using PlatReserve.Models;
using PlatReserve.Services;
using PlatReserve.ViewModels;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace PlatReserve.Views;

public partial class LoginPage : ContentPage
{
    private readonly SignupViewModel vm;

    public LoginPage(SignupViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        this.vm = vm;

        // On s'assure qu'un Admin existe dans la base pour tes tests
        //CreerAdminParDefaut();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // 1. On récupère les valeurs directement sur l'écran
        string telTape = EntryPhone.Text?.Trim();
        string passTape = EntryPassword.Text?.Trim();

        if (string.IsNullOrEmpty(telTape) || string.IsNullOrEmpty(passTape))
        {
            await DisplayAlertAsync("Erreur", "Veuillez remplir les deux cases", "OK");
            return;
        }

        // 2. On les donne à "manger" manuellement au ViewModel pour être sûr
        vm.Phone = telTape;
        vm.Password = passTape;

        // 3. On appelle la fonction de connexion
        
        await vm.Login();
    }

    private async void OnGoToSignupClicked(object sender, EventArgs e)
    {
     await   vm.AllerALaPageInscription();
    }

    //private void ContentPage_Loaded(object sender, EventArgs e)
    //{
    //    vm.CreateDefaultAdmin();
    //}



    //public void CreerAdminParDefaut()
    //{
    //    try
    //    {
    //        var realm = _realmFactory.GetRealm();

    //        // On vérifie s'il existe n'importe quelle personne (pas seulement l'admin)
    //        var count = realm.All<Personne>().Count();

    //        // On écrit dans la console de sortie (Output) de Visual Studio pour vérifier
    //        System.Diagnostics.Debug.WriteLine($"[DEBUG REALM] Nombre de personnes trouvées : {count}");

    //        if (count == 0)
    //        {
    //            System.Diagnostics.Debug.WriteLine("[DEBUG REALM] La base est vide, création de l'admin...");

    //            realm.Write(() =>
    //            {
    //                var admin = new Personne
    //                {
    //                    Nom = "Sonna Trevis",
    //                    Telephone = "657341358",
    //                    // On utilise bien notre propriété Role qui remplit le RoleId en cachette
    //                    //Role = RoleUtilisateur.Admin
    //                };

    //                realm.Add(admin);
    //            });


    //            System.Diagnostics.Debug.WriteLine("[DEBUG REALM] Admin créé avec succès !");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        System.Diagnostics.Debug.WriteLine($"[DEBUG REALM] ERREUR lors du seed : {ex.Message}");
    //    }
    //}

    //private async void OnLoginClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // 1.Trim() : enlève tous les espaces invisibles au début et à la fin du texte !
    //        string phone = PhoneEntry.Text?.Trim();

    //        if (string.IsNullOrWhiteSpace(phone))
    //        {
    //            await DisplayAlertAsync("Erreur", "Le numéro de téléphone ne peut pas être vide.", "OK");
    //            return;
    //        }

    //        var realm = _realmFactory.GetRealm();


    //        // MODE DIAGNOSTIC (Pour voir ce qu'il y a dans la base)
    //        var tousLesUtilisateurs = realm.All<Personne>().ToList();

    //        if (tousLesUtilisateurs.Count == 0)
    //        {
    //            await DisplayAlertAsync("Problème", "La base de données est 100% vide ! L'Admin n'a pas été créé.", "OK");
    //            return; // On arrête là
    //        }


    //        // On cherche l'utilisateur
    //        var utilisateur = realm.All<Personne>().FirstOrDefault(p => p.Telephone == phone);

    //        if (utilisateur != null)
    //        {
    //            _authService.User(utilisateur);

    //            if (utilisateur.RoleId == (int)RoleUtilisateur.Admin)
    //            {
    //                var adminPage = IPlatformApplication.Current.Services.GetService<AdminPage>();
    //                await Navigation.PushAsync(adminPage);
    //            }
    //            else
    //            {
    //                var voyageur = IPlatformApplication.Current.Services.GetService<VoyageurDashboardPage>();

    //                await Navigation.PushAsync(voyageur);
    //            }
    //        }
    //        else
    //        {
    //            // On affiche les numéros qui sont REELLEMENT dans la base
    //            string numerosDansLaBase = string.Join(", ", tousLesUtilisateurs.Select(u => u.PhoneNumber));

    //            await DisplayAlertAsync("Échec", $"Tu as tapé : '{phone}'.\n\nNuméros existants dans la base : {numerosDansLaBase}", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlertAsync("Erreur Critique", ex.Message, "OK");
    //    }
    //}
    //private async void OnGoToSignupClicked(object sender, EventArgs e)
    //{
    //    // On envoie l'utilisateur vers la page d'inscription
    //    var SignupPage = IPlatformApplication.Current.Services.GetService<SignupPage>();
    //     await Navigation.PushAsync( SignupPage);
    //}
}