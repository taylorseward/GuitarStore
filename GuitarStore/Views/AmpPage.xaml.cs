using GuitarStore.ViewModels;
using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.Views;
using Microsoft.Maui.Controls;

namespace GuitarStore.Views;

public partial class AmpPage : ContentPage
{
    private readonly AmpViewModel _viewModel;

    public AmpPage(AmpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadAmpsCommand.Execute(null);

    }



    private async void OnAmpSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedAmp = e.CurrentSelection.FirstOrDefault() as Amp;

        if (selectedAmp == null)
        {
            return;
        }

        var action = await DisplayActionSheet("Choose an action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                // Navigate to edit 
                await Shell.Current.GoToAsync($"AddAmpPage", true, new Dictionary<string, object>
                {
                    { "ampId", selectedAmp.Id }
                });

                break;

            case "Delete":
                // Confirm before deleting
                var confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {selectedAmp.Make} {selectedAmp.Model}?", "Yes", "No");
                if (confirm)
                {
                    await _viewModel.DeleteAmpCommand.ExecuteAsync(selectedAmp);
                }
                break;
        }

    // Clear selection to allow re-selecting the same item
    ((CollectionView)sender).SelectedItem = null;
    }
    private void OnSortChanged(object sender, EventArgs e)
    {
        _viewModel.SortAmps();
    }
    public async void OnAddAmpClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddAmpPage");
    }
}