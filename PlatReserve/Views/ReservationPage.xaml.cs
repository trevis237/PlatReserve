using Microsoft.Maui.Controls;
using PlatReserve.ViewModels;
using PlatReserve.Models;

namespace PlatReserve.Views;

public partial class ReservationPage : ContentPage
{
    private readonly VoyageurViewModel _viewModel;
    public ReservationPage(VoyageurViewModel vm)
	{
		InitializeComponent();

        // On dit à la page XAML : "Ton cerveau (contexte de données), c'est ce ViewModel"
        BindingContext = vm;
        this._viewModel = vm;
    }

    private async void OnReserverClicked(object sender, EventArgs e)
    {
        // 1. On récupère le bouton qui a été cliqué
        var bouton = (Button)sender;

        // 2. On récupère le Trajet lié à ce bouton (le CommandParameter)
        // C'est ici qu'on obtient l'objet réel, pas juste le type !
        var trajetChoisi = (Trajet)bouton.CommandParameter;

        if (trajetChoisi == null) return;

        // 3. Simulation de paiement (UI)
        bool confirmer = await DisplayAlertAsync("Paiement",
            $"Confirmer le paiement de {trajetChoisi.Prix} FCFA ?", "OUI", "NON");

        if (confirmer)
        {
            // 4. Appel au ViewModel
            var monBillet = _viewModel.ReserverEtPayer(trajetChoisi, trajetChoisi.Prix, "Orange Money");

            if (monBillet != null)
            {
                await DisplayAlertAsync("Succès", "Réservation effectuée !", "Voir mon billet");

                // 5. NAVIGATION VERS LES DÉTAILS
                // On passe le billet à la page suivante via le dictionnaire de navigation
                var navigationParameter = new Dictionary<string, object>
            {
                { "BilletSelectionne", monBillet }
            };

                await Shell.Current.GoToAsync("BilletDetailPage", navigationParameter);
            }
            else
            {
                await DisplayAlertAsync("Erreur", "Impossible de réserver (Plus de places).", "OK");
            }
        }
    }



    //private void OnLoginClicked(object sender, EventArgs e)
    //{
    //    string phone = phoneEntry.Text;

    //    if (string.IsNullOrEmpty(phone) )
    //    {
    //        DisplayAlertAsync("Erreur", "Veuillez entrer votre numero de telephone.", "OK");
    //        return;
    //    }

    //    bool success = _viewModel.Login(phone);

    //    if (success) 
    //    {
    //        StatutLabel.Text = $"connecte en tant que {_viewModel.UtilisateurConnecter.Nom}";
    //        StatutLabel.TextColor = Colors.Green;
    //    }
    //    else
    //    {
    //        DisplayAlertAsync("Erreur", "Ce numéro est introuvable. Veuillez vous inscrire.", "OK");
    //    }
    //}

    //private void OnSignupClicked(object sender, EventArgs e)
    //{
    //    string phone = phoneEntry.Text;
    //    string Nom = NameEntry.Text;

    //    if(string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(Nom))
    //    {
    //        DisplayAlertAsync("Erreur", "Veuillez remplir le nom et le numéro.", "OK");
    //        return;
    //    }

    //    bool success = _viewModel.Signup(Nom, phone);

    //    if (success)
    //    {
    //        StatutLabel.Text = $"Inscrit et connecté : {_viewModel.UtilisateurConnecter.Nom}";
    //        StatutLabel.TextColor = Colors.Green;
    //    }
    //    else
    //    {
    //        DisplayAlertAsync("Erreur", "Ce numéro est déjà inscrit. Cliquez sur 'Se Connecter'.", "OK");
    //    }


    //}

    //public void OnReserverClicked(object sender, EventArgs e)
    //{
    //    // On vérifie si l'utilisateur est connecté
    //    if(_viewModel.UtilisateurConnecter == null)
    //    {
    //        DisplayAlertAsync("Attention", "Vous devez être connecté pour réserver.", "OK");
    //        return;
    //    }

    //    // On récupère le bouton qui a été cliqué
    //    var bouton = sender as Button;

    //    // On récupère le trajet lié à ce bouton (grâce au CommandParameter dans le XAML)
    //    var trajetChoisie = bouton.CommandParameter as Trajet;

    //    if(trajetChoisie != null)
    //    {
    //        if(trajetChoisie.BusAssigne.PlacesRestantes > 0)
    //        {
    //            _viewModel.ReserverBillet(trajetChoisie);
    //            DisplayAlertAsync("Succès", "Billet réservé avec succès !", "OK");
    //        }
    //        else
    //        {
    //            DisplayAlert("Désolé", "Il n'y a plus de places dans ce bus.", "OK");
    //        }
    //    }
    //}
}