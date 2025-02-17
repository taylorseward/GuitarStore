using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.ViewModels;
using Microsoft.Maui.Media;
using System.Threading.Tasks;


namespace GuitarStore.Views;

[QueryProperty(nameof(GuitarId), "guitarId")]
public partial class AddGuitarPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private FileResult _photoFile;

    private int _guitarId;

    public int GuitarId
    {
        get => _guitarId;
        set
        {
            _guitarId = value;
            Console.WriteLine($"DEBUG: GuitarId set in setter = {_guitarId}");
            if (_guitarId != 0)
            {
                LoadGuitarAsync(_guitarId);
            }
        }
    }
    public AddGuitarPage(DatabaseService databaseService)
	{
		InitializeComponent();
        _databaseService = databaseService;
        BindingContext = new AddGuitarViewModel(_databaseService);

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine($"DEBUG: GuitarId received in AddGuitarPage = {GuitarId}");
        if (GuitarId != 0)
        {
            await LoadGuitarAsync(GuitarId);
        }
    }

    private async Task LoadGuitarAsync(int guitarId)
    {
        var guitar = await _databaseService.GetGuitarByIdAsync(guitarId);
        if (guitar != null)
        {
            Console.WriteLine($"DEBUG: Loaded Guitar - Make: {guitar.Make}, Model: {guitar.Model}, Price: {guitar.Price}");
            var viewModel = (AddGuitarViewModel)BindingContext;

            makeEntry.Text = guitar.Make;
            modelEntry.Text = guitar.Model;
            priceEntry.Text = guitar.Price.ToString();
            viewModel.SelectedGuitarType = guitar.GuitarType;
            viewModel.NumberOfStrings = guitar.NumberOfStrings;

            if (!string.IsNullOrEmpty(guitar.PhotoPath))
            {
                guitarPhoto.Source = ImageSource.FromFile(guitar.PhotoPath);
            }
            _photoFile = new FileResult(guitar.PhotoPath);  // Keep the photo reference
        }
        else
        {
            Console.WriteLine("DEBUG: No guitar found with that ID");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (double.TryParse(priceEntry.Text, out double price))
        {
            var viewModel = (AddGuitarViewModel)BindingContext;
            var guitar = new Guitar
            {
                Id = GuitarId,
                Make = makeEntry.Text,
                Model = modelEntry.Text,
                GuitarType = viewModel.SelectedGuitarType,
                NumberOfStrings = viewModel.NumberOfStrings,
                Price = price,
                PhotoPath = _photoFile?.FullPath
            };
            if (GuitarId == 0)
            {
                // Add new guitar
                await _databaseService.AddGuitarAsync(guitar);
                await DisplayAlert("Success", "Guitar added!", "OK");
            }
            else
            {
                // Update existing guitar
                await _databaseService.UpdateGuitarAsync(guitar);
                await DisplayAlert("Success", "Guitar updated!", "OK");
            }

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Invalid Input", "Please enter valid price", "OK");
        }

    }



    private async void OnAddPhotoClicked (object sender, EventArgs e)
    {
        try
        {
            _photoFile = await MediaPicker.PickPhotoAsync();

            if (_photoFile != null)
            {
                guitarPhoto.Source = ImageSource.FromFile(_photoFile.FullPath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Photo selection failed: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked (object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}