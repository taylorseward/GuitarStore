using GuitarStore.Models;
using GuitarStore.Services;
using GuitarStore.ViewModels;
using Microsoft.Maui.Media;
using System.Threading.Tasks;


namespace GuitarStore.Views;

[QueryProperty(nameof(AccessoryId), "accessoryId")]
public partial class AddAccessoryPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private FileResult _photoFile;

    private int _accessoryId;

    public int AccessoryId
    {
        get => _accessoryId;
        set
        {
            _accessoryId = value;
            Console.WriteLine($"DEBUG: Accessory Id set in setter = {_accessoryId}");
            if (_accessoryId != 0)
            {
                LoadAccessoryAsync(_accessoryId);
            }
        }
    }
    public AddAccessoryPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        BindingContext = new AddAccessoryViewModel(_databaseService);

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine($"DEBUG: AccessoryId received in AddAccessoryPage = {AccessoryId}");
        if (AccessoryId != 0)
        {
            await LoadAccessoryAsync(AccessoryId);
        }
    }

    private async Task LoadAccessoryAsync(int accessoryId)
    {
        var accessory = await _databaseService.GetAccessoryByIdAsync(accessoryId);
        if (accessory != null)
        {
            Console.WriteLine($"DEBUG: Loaded Accessory - Make: {accessory.Make}, Model: {accessory.Model}, Price: {accessory.Price}");
            var viewModel = (AddAccessoryViewModel)BindingContext;

            makeEntry.Text = accessory.Make;
            modelEntry.Text = accessory.Model;
            priceEntry.Text = accessory.Price.ToString();
            viewModel.SelectedAccessoryType = accessory.AccessoryType;

            if (!string.IsNullOrEmpty(accessory.PhotoPath))
            {
                accessoryPhoto.Source = ImageSource.FromFile(accessory.PhotoPath);
            }
            _photoFile = new FileResult(accessory.PhotoPath);  // Keep the photo reference
        }
        else
        {
            Console.WriteLine("DEBUG: No accessory found with that ID");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (double.TryParse(priceEntry.Text, out double price))
        {
            var viewModel = (AddAccessoryViewModel)BindingContext;
            var accessory = new Accessory
            {
                Id = AccessoryId,
                Make = makeEntry.Text,
                Model = modelEntry.Text,
                AccessoryType = viewModel.SelectedAccessoryType,
                Price = price,
                PhotoPath = _photoFile?.FullPath
            };
            if (AccessoryId == 0)
            {
                // Add 
                await _databaseService.AddAccessoryAsync(accessory);
                await DisplayAlert("Success", "Accessory Added!", "OK");
            }
            else
            {
                // Update 
                await _databaseService.UpdateAccessoryAsync(accessory);
                await DisplayAlert("Success", "Accessory Updated!", "OK");
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
                accessoryPhoto.Source = ImageSource.FromFile(_photoFile.FullPath);
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