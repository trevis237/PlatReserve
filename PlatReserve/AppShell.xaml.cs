namespace PlatReserve
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // On enregistre manuellement les pages qui ne sont pas dans la barre de navigation
            Routing.RegisterRoute(nameof(Views.BilletDetailPage), typeof(Views.BilletDetailPage));
            Routing.RegisterRoute(nameof(Views.SignupPage), typeof(Views.SignupPage));

            Routing.RegisterRoute(nameof(Views.ReservationPage), typeof(Views.ReservationPage));
            Routing.RegisterRoute(nameof(Views.VoyageurDashboardPage), typeof(Views.VoyageurDashboardPage));
            Routing.RegisterRoute(nameof(Views.AdminPage), typeof(Views.AdminPage));
        }
    }
}
