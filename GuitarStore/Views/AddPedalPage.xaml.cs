using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.ViewModels;
using Microsoft.Maui.Media;
using System.Threading.Tasks;


namespace GuitarStore.Views;

[QueryProperty(nameof(PedalId), "pedalId")]
public partial class AddPedalPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private FileResult _photoFile;

    private int _pedalId;

    public int PedalId
    {
        get => _pedalId;
        set
        {
            _pedalId = value;
            Console.WriteLine($"DEBUG: PedalId set in setter = {_pedalId}");
            if (_pedalId != 0)
            {
                LoadPedalAsync(_pedalId);
            }
        }
    }
    public AddPedalPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        BindingContext = new AddPedalViewModel(_databaseService);

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine($"DEBUG: PedalId received in AddPedalPage = {PedalId}");
        if (PedalId != 0)
        {
            await LoadPedalAsync(PedalId);
        }
    }

    private async Task LoadPedalAsync(int pedalId)
    {
        var pedal = await _databaseService.GetPedalByIdAsync(pedalId);
        if (pedal != null)
        {
            Console.WriteLine($"DEBUG: Loaded Pedal - Make: {pedal.Make}, Model: {pedal.Model}, Price: {pedal.Price}");
            var viewModel = (AddPedalViewModel)BindingContext;

            makeEntry.Text = pedal.Make;
            modelEntry.Text = pedal.Model;
            priceEntry.Text = pedal.Price.ToString();
            viewModel.SelectedPedalType = pedal.PedalType;

            if (!string.IsNullOrEmpty(pedal.PhotoPath))
            {
                pedalPhoto.Source = ImageSource.FromFile(pedal.PhotoPath);
            }
            _photoFile = new FileResult(pedal.PhotoPath);  // Keep the photo reference
        }
        else
        {
            Console.WriteLine("DEBUG: No pedal found with that ID");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (double.TryParse(priceEntry.Text, out double price))
        {
            var viewModel = (AddPedalViewModel)BindingContext;
            var pedal = new Pedal
            {
                Id = PedalId,
                Make = makeEntry.Text,
                Model = modelEntry.Text,
                PedalType = viewModel.SelectedPedalType,
                Price = price,
                PhotoPath = _photoFile?.FullPath
            };
            if (PedalId == 0)
            {
                // Add 
                await _databaseService.AddPedalAsync(pedal);
                await DisplayAlert("Success", "Pedal Added!", "OK");
            }
            else
            {
                // Update 
                await _databaseService.UpdatePedalAsync(pedal);
                await DisplayAlert("Success", "Pedal Updated!", "OK");
            }

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Invalid Input", "Please enter valid price", "OK");
        }

    }



    private async void OnAddPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            _photoFile = await MediaPicker.PickPhotoAsync();

            if (_photoFile != null)
            {
                pedalPhoto.Source = ImageSource.FromFile(_photoFile.FullPath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Photo selection failed: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}