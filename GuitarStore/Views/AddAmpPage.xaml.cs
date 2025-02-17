using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.ViewModels;
using Microsoft.Maui.Media;
using System.Threading.Tasks;


namespace GuitarStore.Views;

[QueryProperty(nameof(AmpId), "ampId")]
public partial class AddAmpPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private FileResult _photoFile;

    private int _ampId;

    public int AmpId
    {
        get => _ampId;
        set
        {
            _ampId = value;
            Console.WriteLine($"DEBUG: AmpId set in setter = {_ampId}");
            if (_ampId != 0)
            {
                LoadAmpAsync(_ampId);
            }
        }
    }
    public AddAmpPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        BindingContext = new AddAmpViewModel(_databaseService);

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine($"DEBUG: AmpId received in AddAmpPage = {AmpId}");
        if (AmpId != 0)
        {
            await LoadAmpAsync(AmpId);
        }
    }

    private async Task LoadAmpAsync(int ampId)
    {
        var amp = await _databaseService.GetAmpByIdAsync(ampId);
        if (amp != null)
        {
            Console.WriteLine($"DEBUG: Loaded Amp - Make: {amp.Make}, Model: {amp.Model}, Price: {amp.Price}");
            var viewModel = (AddAmpViewModel)BindingContext;

            makeEntry.Text = amp.Make;
            modelEntry.Text = amp.Model;
            priceEntry.Text = amp.Price.ToString();
            viewModel.SelectedAmpType = amp.AmpType;

            if (!string.IsNullOrEmpty(amp.PhotoPath))
            {
                ampPhoto.Source = ImageSource.FromFile(amp.PhotoPath);
            }
            _photoFile = new FileResult(amp.PhotoPath);  // Keep the photo reference
        }
        else
        {
            Console.WriteLine("DEBUG: No amp found with that ID");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (double.TryParse(priceEntry.Text, out double price))
        {
            var viewModel = (AddAmpViewModel)BindingContext;
            var amp = new Amp
            {
                Id = AmpId,
                Make = makeEntry.Text,
                Model = modelEntry.Text,
                AmpType = viewModel.SelectedAmpType,
                Price = price,
                PhotoPath = _photoFile?.FullPath
            };
            if (AmpId == 0)
            {
                // Add 
                await _databaseService.AddAmpAsync(amp);
                await DisplayAlert("Success", "Amp Added!", "OK");
            }
            else
            {
                // Update 
                await _databaseService.UpdateAmpAsync(amp);
                await DisplayAlert("Success", "Amp Updated!", "OK");
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
                ampPhoto.Source = ImageSource.FromFile(_photoFile.FullPath);
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