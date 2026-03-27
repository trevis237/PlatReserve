//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using PlatReserve.Models;
//using PlatReserve.Views;
//using Realms;
//namespace PlatReserve.ViewModels;

//public partial class VoyageurDashboardViewModel : ObservableObject
//{
//    [RelayCommand]
//    public async Task AllerAuxDetails(Billet billetClique)
//    {
//        // On crée un dictionnaire pour passer l'objet
//        var navigationParameter = new Dictionary<string, object>
//{
//    { "BilletSelectionne", billetClique }
//};

//        // On navigue vers la page de détails en passant le paramètre
//        await Shell.Current.GoToAsync(nameof(BilletDetailPage), navigationParameter);
//    }
//}

