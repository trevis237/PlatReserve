using PlatReserve.Models;
using PlatReserve.ViewModels;

namespace PlatReserve.Views
{
    public partial class VoyageurDashboardPage : ContentPage
    {
        private readonly VoyageurViewModel _viewModel;

        public VoyageurDashboardPage(VoyageurViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            // On lie la page au cerveau
            BindingContext = _viewModel;
        }

        // Cette méthode s'exécute chaque fois qu'on arrive sur la page
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // ✅ On appelle la nouvelle méthode du ViewModel
            _viewModel.MettreAJourInfos();
        }

        //private async void OnReserverClicked(object sender, EventArgs e)
        //{
        //    var bouton = sender as Button;
        //    var trajetChoisi = bouton.CommandParameter as Trajet;

        //    if (trajetChoisi == null) return;

        //    // Simulation de paiement
        //    bool confirmer = await DisplayAlertAsync("Paiement",
        //        $"Voulez-vous payer {trajetChoisi.Prix} FCFA pour aller à {trajetChoisi.Arrivee} ?",
        //        "OUI", "NON");

        //    if (confirmer)
        //    {
        //        // 1. On change 'bool succes' par 'var monBillet' (ou Billet monBillet)
        //        var monBillet = _viewModel.ReserverEtPayer(trajetChoisi, 5000, "Mobile Money");

        //        IsBusy = true; // Affiche un indicateur de chargement

        //        await Task.Delay(2000);

        //        // 2. Au lieu de 'if (succes)', on vérifie si le billet a bien été créé
        //        if (monBillet != null)
        //        {
        //            IsBusy = false; // Cache l'indicateur de chargement

        //            await DisplayAlertAsync("Succès", "Votre réservation est validée !", "Voir mon billet");

        //            // 3. NAVIGATION VERS LES DÉTAILS
        //            // On prépare le paramètre pour la page de détails
        //            var navigationParameter = new Dictionary<string, object>
        //{
        //    { "BilletId", monBillet.Id.ToString() }
        //};

        //            // On change de page vers les détails du billet
        //            await Shell.Current.GoToAsync("BilletDetailPage", navigationParameter);
        //        }
        //        else
        //        {
        //            // Si c'est null, c'est qu'il n'y avait plus de places
        //            await DisplayAlertAsync("Erreur", "Plus de places ou erreur technique", "OK");
        //        }
        //    }
        //}

        private async void OnReserverClicked(object sender, EventArgs e)
        {
            var bouton = (Button)sender;
            var trajetChoisi = (Trajet)bouton.CommandParameter;
            if (trajetChoisi == null) return;

            bool confirmer = await DisplayAlertAsync("Paiement", "Confirmer ?", "OUI", "NON");
            if (confirmer)
            {
                var monBillet = await _viewModel.SimulerPaiementEtReserver(trajetChoisi, 5000);

                if (monBillet != null)
                {
                    // --- DIAGNOSTIC DE DÉPART ---
                    string idAEnvoyer = monBillet.Id.ToString();
                    // await DisplayAlert("Départ", $"On envoie l'ID : {idAEnvoyer}", "OK");

                    var navParam = new Dictionary<string, object>
            {
                { "BilletId", idAEnvoyer } // "BilletId" est la clé
            };

                    await Shell.Current.GoToAsync("BilletDetailPage", navParam);
                }
            }
        }
    }
}