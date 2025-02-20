using GuitarStore.ViewModels;
using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.Views;
using Microsoft.Maui.Controls;

namespace GuitarStore.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomeViewModel();
	}

	public async void OnInventoryClicked (object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(InventoryPage));
	}
    public async void OnGuitarsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(GuitarPage));
    }
    public async void OnAmpsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AmpPage));
    }
    public async void OnPedalsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PedalPage));
    }
    public async void OnAccessoriesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AccessoryPage));
    }
}