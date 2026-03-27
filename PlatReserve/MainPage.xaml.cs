using Microsoft.Maui.Controls;
using PlatReserve.ViewModels;
using PlatReserve.Models;

namespace PlatReserve
{
    public partial class MainPage : ContentPage
    {
        private readonly ReservationViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            //_viewModel = viewModel;

            // On dit à la page XAML : "Ton cerveau (contexte de données), c'est ce ViewModel"
            BindingContext = _viewModel;
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
        }
    }

}
