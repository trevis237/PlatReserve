using PlatReserve.Models;
using PlatReserve.Services;
using PlatReserve.ViewModels;

namespace PlatReserve.Views;

public partial class SignupPage : ContentPage
{

    public SignupPage(SignupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

 //   private async void OnSignupClicked(object sender, EventArgs e)
	//{
	//	string nom = NameEntry.Text?.Trim();
	//	string phone = PhoneEntry.Text?.Trim();

	//	if(string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(phone))
	//	{
 //           await DisplayAlertAsync("Erreur", "Remplissez tous les champs", "OK");
 //           return;
 //       }

	//	var realm = _realmFactory.GetRealm();

 //       // Vérifier si le numéro existe déjà
 //       if (realm.All<Personne>().Any(p => p.PhoneNumber == phone))
 //       {
 //           await DisplayAlertAsync("Erreur", "Ce numéro est déjà utilisé", "OK");
 //           return;
 //       }
	//	Personne nouveauVoyageur = null;
	//	realm.Write(() =>
	//	{
	//		nouveauVoyageur = new Personne
	//		{
	//			Name = nom,
	//			PhoneNumber = phone,
	//			Role = RoleUtilisateur.Voyageur
	//		};
 //           realm.Add(nouveauVoyageur);
 //       });

 //       // Connexion automatique après inscription
 //       _authService.ConnecterUtilisateur(nouveauVoyageur);

	//	var voyageurPage = IPlatformApplication.Current.Services.GetService<VoyageurDashboardPage>();
	//	await Navigation.PushAsync(voyageurPage);
 //   }

    private async void OnBackToLoginClicked(object sender, EventArgs e) => await Navigation.PopAsync();
}