using GuitarStore.ViewModels;
using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.Views;
using Microsoft.Maui.Controls;

namespace GuitarStore.Views;

public partial class AccessoryPage : ContentPage
{
    private readonly AccessoryViewModel _viewModel;

    public AccessoryPage(AccessoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadAccessoriesCommand.Execute(null);

    }



    private async void OnAccessorySelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedAccessory = e.CurrentSelection.FirstOrDefault() as Accessory;

        if (selectedAccessory == null)
        {
            return;
        }

        var action = await DisplayActionSheet("Choose an action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                // Navigate to edit 
                await Shell.Current.GoToAsync($"AddAccessoryPage", true, new Dictionary<string, object>
                {
                    { "AccessoryId", selectedAccessory.Id }
                });

                break;

            case "Delete":
                // Confirm before deleting
                var confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {selectedAccessory.Make} {selectedAccessory.Model}?", "Yes", "No");
                if (confirm)
                {
                    await _viewModel.DeleteAccessoryCommand.ExecuteAsync(selectedAccessory);
                }
                break;
        }

    // Clear selection to allow re-selecting the same item
    ((CollectionView)sender).SelectedItem = null;
    }
    private void OnSortChanged(object sender, EventArgs e)
    {
        _viewModel.SortAccessories();
    }
    public async void OnAddAccessoryClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddAccessoryPage");
    }
}