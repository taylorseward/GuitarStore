using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GuitarStore.Models;
using GuitarStore.Services;

namespace GuitarStore.ViewModels
{
    public class AddAccessoryViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string PhotoPath { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }

        private string _selectedAccessoryType;
        public string SelectedAccessoryType
        {
            get => _selectedAccessoryType;
            set
            {
                if (_selectedAccessoryType != value)
                {
                    _selectedAccessoryType = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> AccessoryTypes { get; } = new ObservableCollection<string>
        {
            "Strings",
            "Tuners",
            "Picks",
            "Stands",
            "Straps",
            "Cases",
            "Pickups",
            "Instrument Cables",
            "Power Cables",
            "Pedalboards"
        };




        public ICommand SaveCommand { get; }

        public AddAccessoryViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            SaveCommand = new Command(async () => await SaveAccessoryAsync());
        }

        private async Task SaveAccessoryAsync()
        {
            if (string.IsNullOrWhiteSpace(Make) || string.IsNullOrWhiteSpace(Model))
            {
                await Shell.Current.DisplayAlert("Error", "Make and Model are required.", "OK");
                return;
            }
            var newAccessory = new Accessory
            {
                PhotoPath = PhotoPath,
                Make = Make,
                Model = Model,
                AccessoryType = SelectedAccessoryType,
                Price = Price
            };

            await _databaseService.AddAccessoryAsync(newAccessory);

            await Shell.Current.GoToAsync("..");
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

    }

}
