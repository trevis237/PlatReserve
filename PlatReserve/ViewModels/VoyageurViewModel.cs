//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input; // Nouveau : pour les commandes
//using PlatReserve.Models;
//using PlatReserve.Services;
//using PlatReserve.Views;
//using Realms;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Reactive.Linq;

//namespace PlatReserve.ViewModels
//{
//    public partial class VoyageurViewModel : ObservableObject
//    {
//        private readonly Realm _realm;
//        private readonly AuthService _authService;

//        [ObservableProperty]
//        private string _nomUtilisateur;

//        public ObservableCollection<Trajet> TrajetsDisponibles { get; }
//        //public IQueryable<Trajet> TrajetsDisponibles => _realm.All<Trajet>();
//        public VoyageurViewModel(IRealmService realmFactory, AuthService authService)
//        {
//            _realm = realmFactory.GetRealm();
//            _authService = authService;
//            var coll = _realm.All<Trajet>().AsEnumerable();
//            TrajetsDisponibles = new ObservableCollection<Trajet>( coll);
//            //.Where(t => t.BusAssigne != null && t.BusAssigne.AgenceProprietaire != null);
//            MettreAJourInfos();

//            Debug.WriteLine(TrajetsDisponibles.Count());
//        }

//        public void MettreAJourInfos()
//        {
//            _nomUtilisateur = _authService.User?.Nom ?? "Voyageur";

//        }

//        // ============================================================
//        // 🚀 LA LOGIQUE DE RÉSERVATION ET PAIEMENT
//        // ============================================================

//        public Billet ReserverEtPayer(Trajet trajet, double montant, string methode)
//        {
//            // 1. Sécurité : On vérifie s'il reste des places
//            if (trajet.BusAssigne.PlacesRestantes <= 0)
//                return null;

//            try
//            {
//                Billet billetGenere = null;

//                _realm.Write(() =>
//                {
//                    // A. Créer la Réservation (DANS le Write)
//                    var nouvelleResa = new Reservation
//                    {
//                        Voyageur = _authService.User,
//                        TrajetConcerne = trajet,
//                        DateReservation = DateTimeOffset.Now,
//                        EstPayee = true
//                    };
//                    _realm.Add(nouvelleResa); // Indispensable d'être dans le Write

//                    // B. Créer le Paiement
//                    var paye = new Paiement
//                    {
//                        Montant = montant,
//                        Methode = methode,
//                        TransactionId = "PAY-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
//                        ReservationAssociee = nouvelleResa
//                    };
//                    _realm.Add(paye);

//                    // C. Créer le Billet
//                    billetGenere = new Billet
//                    {
//                        InfoReservation = nouvelleResa,
//                        ReferenceBillet = "TKT-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper()
//                    };
//                    _realm.Add(billetGenere);

//                    // D. Mettre à jour les places du bus
//                    trajet.BusAssigne.PlacesRestantes -= 1;
//                });

//                return billetGenere; // On renvoie le billet pour la navigation
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Erreur Realm : {ex.Message}");
//                return null;
//            }
//        }


//            public IEnumerable<Billet> MesBillets => _realm.All<Billet>()
//                .ToList()
//                .Where(b => b.InfoReservation.Voyageur.Id == _authService.User.Id);

//            [RelayCommand]
//            public async Task AnnulerBillet(Billet billet)
//            {
//                if (billet == null) return;

//                if (!billet.InfoReservation.AnnulationPossible)
//                {
//                    await Application.Current.MainPage.DisplayAlertAsync("Refusé", "L'annulation doit se faire 24h avant le départ.", "OK");
//                    return;
//                }

//                _realm.Write(() => {
//                    // 1. On rend la place au bus
//                    billet.InfoReservation.TrajetConcerne.BusAssigne.PlacesRestantes += 1;
//                    // 2. On supprime la réservation et le billet
//                    _realm.Remove(billet.InfoReservation);
//                    _realm.Remove(billet);
//                });

//                // On rafraîchit l'interface
//                OnPropertyChanged(nameof(MesBillets));
//            }

//        [RelayCommand]
//        public async Task AllerAuxDetails(Billet billetClique)
//        {
//            // On crée un dictionnaire pour passer l'objet
//            var navigationParameter = new Dictionary<string, object>
//{
//    { "BilletSelectionne", billetClique }
//};

//            // On navigue vers la page de détails en passant le paramètre
//            await Shell.Current.GoToAsync(nameof(BilletDetailPage), navigationParameter);
//        }
//    }
//    }







using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlatReserve.Models;
using PlatReserve.Services;
using PlatReserve.Views;
using Realms;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PlatReserve.ViewModels
{
    public partial class VoyageurViewModel : ObservableObject
    {
        private readonly Realm _realm;
        private readonly AuthService _authService;

        [ObservableProperty]
        private string _nomUtilisateur;

        // Propriété pour l'ActivityIndicator (la roue de chargement)
        [ObservableProperty]
        private bool _isBusy;

        public ObservableCollection<Trajet> TrajetsDisponibles { get; }

        public VoyageurViewModel(IRealmService realmFactory, AuthService authService)
        {
            _realm = realmFactory.GetRealm();
            _authService = authService;

            // On récupère les trajets qui ont bien un bus et une agence (sécurité anti-crash)
            var coll = _realm.All<Trajet>().ToList().Where(t => t.BusAssigne != null && t.BusAssigne.AgenceProprietaire != null);
            TrajetsDisponibles = new ObservableCollection<Trajet>(coll);

            MettreAJourInfos();
        }

        public void MettreAJourInfos()
        {
            // Correction : on utilise la propriété publique NomUtilisateur pour notifier l'UI
            NomUtilisateur = _authService.User?.Nom ?? "Voyageur";
        }

        // ============================================================
        // 🚀 LA LOGIQUE DE SIMULATION (Celle qui manquait !)
        // ============================================================

        public async Task<Billet> SimulerPaiementEtReserver(Trajet trajet, double montant)
        {
            IsBusy = true; // Lance la roue de chargement

            await Task.Delay(2500); // Attend 2.5 secondes pour faire "vrai"

            var billetGenere = ReserverEtPayer(trajet, montant, "Mobile Money");

            IsBusy = false; // Arrête la roue

            // On rafraîchit la liste "Mes Billets" car on vient d'en acheter un
            OnPropertyChanged(nameof(MesBillets));

            return billetGenere;
        }

        // ============================================================
        // 🎫 LOGIQUE RÉSERVATIONS ET BILLETS
        // ============================================================

        public Billet ReserverEtPayer(Trajet trajet, double montant, string methode)
        {
            if (trajet.BusAssigne.PlacesRestantes <= 0)
                return null;

            try
            {
                Billet billetGenere = null;
                _realm.Write(() =>
                {
                    var nouvelleResa = new Reservation
                    {
                        Voyageur = _authService.User,
                        TrajetConcerne = trajet,
                        DateReservation = DateTimeOffset.Now,
                        EstPayee = true
                    };
                    _realm.Add(nouvelleResa);

                    var paye = new Paiement
                    {
                        Montant = montant,
                        Methode = methode,
                        TransactionId = "PAY-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                        ReservationAssociee = nouvelleResa
                    };
                    _realm.Add(paye);

                    billetGenere = new Billet
                    {
                        InfoReservation = nouvelleResa,
                        ReferenceBillet = "TKT-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper()
                    };
                    _realm.Add(billetGenere);

                    trajet.BusAssigne.PlacesRestantes -= 1;
                });

                return billetGenere;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur Realm : {ex.Message}");
                return null;
            }
        }

        public IEnumerable<Billet> MesBillets => _realm.All<Billet>()
            .ToList()
            .Where(b => b.InfoReservation?.Voyageur?.Id == _authService.User?.Id);

        [RelayCommand]
        public async Task AnnulerBillet(Billet billet)
        {
            if (billet == null) return;

            if (!billet.InfoReservation.AnnulationPossible)
            {
                await Application.Current.MainPage.DisplayAlertAsync("Refusé", "Délai de 24h dépassé.", "OK");
                return;
            }

            _realm.Write(() => {
                billet.InfoReservation.TrajetConcerne.BusAssigne.PlacesRestantes += 1;
                _realm.Remove(billet.InfoReservation);
                _realm.Remove(billet);
            });

            OnPropertyChanged(nameof(MesBillets));
        }

        [RelayCommand]
        public async Task AllerAuxDetails(Billet billetClique)
        {
            if (billetClique == null) return;

            // MÉTHODE ROBUSTE POUR ANDROID : On passe l'ID dans l'URL
            // On utilise ?BilletId=... pour que la page suivante puisse le récupérer
            await Shell.Current.GoToAsync($"{nameof(BilletDetailPage)}?BilletId={billetClique.Id}");
        }
    }
}
