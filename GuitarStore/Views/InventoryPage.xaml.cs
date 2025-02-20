using GuitarStore.ViewModels;
using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.Views;
using Microsoft.Maui.Controls;

namespace GuitarStore.Views;

public partial class InventoryPage : ContentPage
{
    private readonly ProductViewModel _viewModel;

    public InventoryPage(ProductViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadProductsCommand.Execute(null);

    }



    private async void OnProductSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedProduct = e.CurrentSelection.FirstOrDefault() as Product;

        if (selectedProduct == null)
        {
            return;
        }

        var action = await DisplayActionSheet("Choose an action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                if (selectedProduct is Guitar guitar)
                {
                    await Shell.Current.GoToAsync($"AddGuitarPage", true, new Dictionary<string, object>
                    {
                        { "guitarId", guitar.Id }
                    });
                }
                else if (selectedProduct is Amp amp)
                {
                    await Shell.Current.GoToAsync($"AddAmpPage", true, new Dictionary<string, object>
                    {
                        { "ampId", amp.Id }
                    });
                }
                else if (selectedProduct is Pedal pedal)
                {
                    await Shell.Current.GoToAsync($"AddPedalPage", true, new Dictionary<string, object>
                    {
                        { "pedalId", pedal.Id }
                    });
                }
                else if (selectedProduct is Accessory accessory)
                {
                    await Shell.Current.GoToAsync($"AddAccessoryPage", true, new Dictionary<string, object>
                    {
                        { "accessoryId", accessory.Id }
                    });
                }

                break;

            case "Delete":
                // Confirm before deleting
                var confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {selectedProduct.Make} {selectedProduct.Model}?", "Yes", "No");
                if (confirm)
                {
                    await _viewModel.DeleteProductCommand.ExecuteAsync(selectedProduct);
                }
                break;
        }

    // Clear selection to allow re-selecting the same item
    ((CollectionView)sender).SelectedItem = null;
    }

    private void OnSortChanged(object sender, EventArgs e)
    {
        _viewModel.SortProducts();
    }

    public async void OnAddInventoryClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Choose Department", "Cancel", null, "Add Guitar", "Add Amp", "Add Pedal", "Add Accessory");

        switch (action)
        {
            case "Add Guitar":
                await Shell.Current.GoToAsync(nameof(AddGuitarPage));
                break;
            case "Add Amp":
                await Shell.Current.GoToAsync(nameof(AddAmpPage));
                break;
            case "Add Pedal":
                await Shell.Current.GoToAsync(nameof(AddPedalPage));
                break;
            case "Add Accessory":
                await Shell.Current.GoToAsync(nameof(AddAccessoryPage));
                break;
            default:
                break;
        }

    }
}