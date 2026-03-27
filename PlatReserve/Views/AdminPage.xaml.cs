using PlatReserve.Models;
using PlatReserve.Services;
using PlatReserve.ViewModels;

namespace PlatReserve.Views;

public partial class AdminPage : ContentPage
{
    public AdminPage(ViewModels.AdminViewModel vm)
    {
        InitializeComponent();
        // On branche le ViewModel reçu par injection
        BindingContext = vm;
    }
}