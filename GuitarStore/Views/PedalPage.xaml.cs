using GuitarStore.ViewModels;
using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.Views;
using Microsoft.Maui.Controls;

namespace GuitarStore.Views;

public partial class PedalPage : ContentPage
{
    private readonly PedalViewModel _viewModel;

    public PedalPage(PedalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadPedalsCommand.Execute(null);

    }



    private async void OnPedalSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedPedal = e.CurrentSelection.FirstOrDefault() as Pedal;

        if (selectedPedal == null)
        {
            return;
        }

        var action = await DisplayActionSheet("Choose an action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                // Navigate to edit 
                await Shell.Current.GoToAsync($"AddPedalPage", true, new Dictionary<string, object>
                {
                    { "pedalId", selectedPedal.Id }
                });

                break;

            case "Delete":
                // Confirm before deleting
                var confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {selectedPedal.Make} {selectedPedal.Model}?", "Yes", "No");
                if (confirm)
                {
                    await _viewModel.DeletePedalCommand.ExecuteAsync(selectedPedal);
                }
                break;
        }

    // Clear selection to allow re-selecting the same item
    ((CollectionView)sender).SelectedItem = null;
    }
    private void OnSortChanged(object sender, EventArgs e)
    {
        _viewModel.SortPedals();
    }
    public async void OnAddPedalClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddPedalPage");
    }
}