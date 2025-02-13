using GuitarStore.ViewModels;
using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.Views;
using Microsoft.Maui.Controls;

namespace GuitarStore.Views;

public partial class GuitarPage : ContentPage
{
    private readonly GuitarViewModel _viewModel;

    public GuitarPage(GuitarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadGuitarsCommand.Execute(null);
        
    }



    private async void OnGuitarSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedGuitar = e.CurrentSelection.FirstOrDefault() as Guitar;

        if (selectedGuitar == null)
        {
            return;
        }

        var action = await DisplayActionSheet("Choose an action", "Cancel", null, "Edit", "Delete");

        switch (action)
        {
            case "Edit":
                // Navigate to AddGuitarPage for editing
                await Shell.Current.GoToAsync($"///AddGuitarPage", true, new Dictionary<string, object>
                {
                    { "guitarId", selectedGuitar.Id }
                });

                break;

            case "Delete":
                // Confirm before deleting
                var confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {selectedGuitar.Make} {selectedGuitar.Model}?", "Yes", "No");
                if (confirm)
                {
                    await _viewModel.DeleteGuitarCommand.ExecuteAsync(selectedGuitar);
                }
                break;
        }

    // Clear selection to allow re-selecting the same item
    ((CollectionView)sender).SelectedItem = null;
    }

    public async void OnAddGuitarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AddGuitarPage");

    }
}