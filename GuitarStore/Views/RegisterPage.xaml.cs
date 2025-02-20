using GuitarStore.Services;
using GuitarStore.ViewModels;
using Microsoft.Maui.Controls;

namespace GuitarStore.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(DatabaseService databaseService)
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel(databaseService);
        }
    }
}