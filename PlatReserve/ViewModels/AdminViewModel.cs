using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlatReserve.Models;
using PlatReserve.Services;
using Realms;
using System.Linq;

namespace PlatReserve.ViewModels
{
    // On utilise "partial" pour que le Toolkit génère le code automatique
    public partial class AdminViewModel : ObservableObject
    {
        private readonly IRealmService _realmService;
        private readonly Realm _realm;

        // --- LISTES VIVANTES (Live Queries) ---
        public IQueryable<Bus> ListeDesBus { get; }
        public IQueryable<Trajet> ListeDesTrajets { get; }
        public IQueryable<Paiement> ListeDesPaiements { get; }
        public IQueryable<Agence> ListeDesAgences { get; }

        // --- PROPRIÉTÉS POUR LE FORMULAIRE (Liées au XAML) ---
        [ObservableProperty] public partial Agence AgenceConcernee { get; set; } = new();
        
        [ObservableProperty] private string _busImmatriculation;
        [ObservableProperty] private int _busPlaces;
        //[ObservableProperty] private Agence _agenceSelectionnee;

        [ObservableProperty] private string _trajetDepart;
        [ObservableProperty] private string _trajetArrivee;
        [ObservableProperty] private double _trajetPrix;
        [ObservableProperty] private Bus _busSelectionne;

        // Propriétés de dates liées aux Pickers
        [ObservableProperty] private DateTime _dateDepart = DateTime.Now;
        [ObservableProperty] private TimeSpan _heureDepart = DateTime.Now.TimeOfDay;
        [ObservableProperty] private DateTime _dateArrivee = DateTime.Now;
        [ObservableProperty] private TimeSpan _heureArrivee = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(3));

        // --- CALCULS FINANCIERS (Propriétés calculées) ---
        // Rappel : On utilise ToList() pour éviter l'erreur "Sum not supported"
        public double RevenueTotal => ListeDesPaiements.ToList().Sum(p => p.Montant);
        public int TotalBilletsVendus => ListeDesPaiements.Count();

        // --- CONSTRUCTEUR ---
        public AdminViewModel(IRealmService realmService)
        {
            _realmService = realmService;
            _realm = _realmService.GetRealm();

            // On initialise les Live Queries
            ListeDesBus = _realm.All<Bus>();
            ListeDesTrajets = _realm.All<Trajet>();
            ListeDesPaiements = _realm.All<Paiement>();
            ListeDesAgences = _realm.All<Agence>();

            
        }

        [RelayCommand]
        private void AjouterAgence()
        {
            //if (!_realm.All<Agence>().Any())
            if (string.IsNullOrWhiteSpace(AgenceConcernee.Nom) || string.IsNullOrEmpty(AgenceConcernee.Nom)) return;

            
                _realm.Write(() => {
                    var agency = new Agence
                    {
                        Nom = AgenceConcernee.Nom,
                        NumeroLicence = "ABC-123"
                    };
                    //_realm.Add(new Agence { Nom = "General Express", NumeroLicence = "ABC-123" });
                });
            
        }

        // --- ACTIONS (COMMANDES) ---

        [RelayCommand]
        public async Task AjouterBus()
        {

            // Création d'une agence par défaut si la base est vide (pour tes tests)
            AjouterAgence();

            //if (AgenceSelectionnee == null)
            //{
            //    Application.Current.MainPage.DisplayAlertAsync("Attention", "Veuillez choisir une agence !", "OK");
            //    return;
            //}

            if (string.IsNullOrWhiteSpace(BusImmatriculation)) return;

            _realm.Write(() => {
                var nouveauBus = new Bus
                {
                    Immatriculation = BusImmatriculation,
                    NombreDePlaces = BusPlaces,
                    PlacesRestantes = BusPlaces,
                    AgenceProprietaire = AgenceConcernee
                };
                _realm.Add(nouveauBus);
            });
            await Shell.Current.DisplayAlertAsync("success", "Bus ajouter avec success", "Ok");

            // Réinitialiser les champs
            BusImmatriculation = string.Empty;
            BusPlaces = 0;
        }

        [RelayCommand]
        public async Task AjouterTrajet()
        {
            if (BusSelectionne == null) return;

            // Fusion des dates et heures
            var departFinal = new DateTimeOffset(DateDepart.Date.Add(HeureDepart));
            var arriveeFinale = new DateTimeOffset(DateArrivee.Date.Add(HeureArrivee));

             await _realm.WriteAsync(async () => {
                var nouveauTrajet = new Trajet
                {
                    Depart = TrajetDepart,
                    Arrivee = TrajetArrivee,
                    Prix = TrajetPrix,
                    BusAssigne = BusSelectionne,
                    DateDepart = departFinal,
                    DateArrivee = arriveeFinale
                };
                _realm.Add(nouveauTrajet);
            });
            await Shell.Current.DisplayAlertAsync("success", "trajet ajouter avec success", "Ok");

            // Réinitialiser
            TrajetDepart = TrajetArrivee = string.Empty;
            TrajetPrix = 0;
        }

        [RelayCommand]
        public void SupprimerBus(Bus busASupprimer)
        {
            _realm.Write(() => {
                // Attention : Realm gère les relations. 
                // Si tu supprimes un bus, vérifie qu'il n'a plus de trajets !
                _realm.Remove(busASupprimer);
            });
        }

        [RelayCommand]
        public void SupprimerTrajet(Trajet trajetASupprimer)
        {
            if (trajetASupprimer == null) return;

            _realm.Write(() =>
            {
                _realm.Remove(trajetASupprimer);
            });
        }

        // Pour modifier, on utilise généralement une navigation vers une page d'édition
        


        [RelayCommand]
        public async Task ModifierTrajet(Trajet trajet)
        {
            // Ici, tu peux soit ouvrir une nouvelle page d'édition, 
            // soit remplir les champs de ton formulaire actuel avec les données du trajet choisi
            TrajetDepart = trajet.Depart;
            TrajetArrivee = trajet.Arrivee;
            TrajetPrix = trajet.Prix;
            BusSelectionne = trajet.BusAssigne;

            await Application.Current.MainPage.DisplayAlertAsync("Edition", "Les infos du trajet ont été remontées dans le formulaire en haut.", "OK");
        }
    }
}
