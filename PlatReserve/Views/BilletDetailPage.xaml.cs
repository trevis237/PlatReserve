using PlatReserve.Models;
using PlatReserve.Services;
using PlatReserve.ViewModels;

namespace PlatReserve.Views;

public partial class BilletDetailPage : ContentPage
{
	public BilletDetailPage(BilletDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}