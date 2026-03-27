using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlatReserve.Models;
using PlatReserve.Services;
using MongoDB.Bson;
using System.Diagnostics;

namespace PlatReserve.ViewModels
{
    // On enlève [QueryProperty] et on utilise l'interface IQueryAttributable
    public partial class BilletDetailViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IRealmService _realmService;

        [ObservableProperty]
        private string _billetId;

        [ObservableProperty]
        private Billet _billetSelectionne;

        public BilletDetailViewModel(IRealmService realmService)
        {
            _realmService = realmService;
        }

        // Cette méthode attrape les paramètres AU MOMENT de la navigation
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("BilletId"))
            {
                // 1. On récupère l'ID
                BilletId = query["BilletId"].ToString();
                Debug.WriteLine($"⭐️ [ANDROID] ID reçu : {BilletId}");

                // 2. On cherche le billet IMMÉDIATEMENT
                var realm = _realmService.GetRealm();
                if (ObjectId.TryParse(BilletId, out var id))
                {
                    var b = realm.Find<Billet>(id);
                    if (b != null)
                    {
                        BilletSelectionne = b;
                        Debug.WriteLine($"✅ [ANDROID] Billet trouvé : {b.ReferenceBillet}");

                        // 3. ON FORCE LA NOTIFICATION (Crucial pour Android)
                        OnPropertyChanged(nameof(BilletSelectionne));
                        OnPropertyChanged(nameof(AfficherBoutonAnnuler));
                    }
                    else
                    {
                        Debug.WriteLine("❌ [ANDROID] Billet introuvable dans la base !");
                    }
                }
            }
        }

        public bool AfficherBoutonAnnuler => BilletSelectionne?.InfoReservation?.AnnulationPossible ?? false;

        // ... garde ta commande AnnulerReservation ...
    }
}