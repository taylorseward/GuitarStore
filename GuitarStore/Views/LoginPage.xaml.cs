using GuitarStore.Services;
using GuitarStore.ViewModels;
using Microsoft.Maui.Controls;

namespace GuitarStore.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(DatabaseService databaseService)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(databaseService);
        }
    }
}