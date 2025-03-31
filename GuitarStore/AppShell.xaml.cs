using GuitarStore.Views;
using GuitarStore.Models;
using GuitarStore.ViewModels;
using GuitarStore.Services;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System;


namespace GuitarStore
{

    public partial class AppShell : Shell
    {
        private readonly DatabaseService _databaseService;
        public AppShell()
        {
            InitializeComponent();

            _databaseService = new DatabaseService();

            // Guitar
            Routing.RegisterRoute(nameof(AddGuitarPage), typeof(AddGuitarPage));
            Routing.RegisterRoute(nameof(GuitarPage), typeof(GuitarPage));
            Routing.RegisterRoute(nameof(EditGuitarPage), typeof(EditGuitarPage));

            // Amp
            Routing.RegisterRoute(nameof(AddAmpPage), typeof(AddAmpPage));
            Routing.RegisterRoute(nameof(AmpPage), typeof(AmpPage));

            // Pedal
            Routing.RegisterRoute(nameof(AddPedalPage), typeof(AddPedalPage));
            Routing.RegisterRoute(nameof(PedalPage), typeof(PedalPage));

            // Accessory
            Routing.RegisterRoute(nameof(AddAccessoryPage), typeof(AddAccessoryPage));
            Routing.RegisterRoute(nameof(AccessoryPage), typeof(AccessoryPage));

            // Inventory
            Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));

            // Home / Login / Register
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }

        // Method for logout popup
        private async void OnLogoutClicked (object sender, EventArgs e)
        {
            var selectedUser = await _databaseService.GetCurrentUserAsync();

            if (selectedUser != null)
            {
                var logout = await DisplayAlert("Confirm Logout", $"Are you sure you want to logout {selectedUser.FirstName} {selectedUser.LastName}?", "Yes", "No");
                if (logout)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
            }
            else
            {
                await DisplayAlert("Error", "Unable to retrieve user info.", "OK");
            }
        }
    }
}
