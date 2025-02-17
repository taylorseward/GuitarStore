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
    public class AddPedalViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string PhotoPath { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }

        private string _selectedPedalType;
        public string SelectedPedalType
        {
            get => _selectedPedalType;
            set
            {
                if (_selectedPedalType != value)
                {
                    _selectedPedalType = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> PedalTypes { get; } = new ObservableCollection<string>
        {
            "Distortion & Overdrive",
            "Multi Effects",
            "Chorus",
            "Phaser",
            "Flanger",
            "Wah",
            "Reverb",
            "Delay",
            "Tremolo",
            "Compressor & EQ",
            "Harmony",
            "Looper",
            "Volume"
        };




        public ICommand SaveCommand { get; }

        public AddPedalViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            SaveCommand = new Command(async () => await SavePedalAsync());
        }

        private async Task SavePedalAsync()
        {
            if (string.IsNullOrWhiteSpace(Make) || string.IsNullOrWhiteSpace(Model))
            {
                await Shell.Current.DisplayAlert("Error", "Make and Model are required.", "OK");
                return;
            }
            var newPedal = new Pedal
            {
                PhotoPath = PhotoPath,
                Make = Make,
                Model = Model,
                PedalType = SelectedPedalType,
                Price = Price
            };

            await _databaseService.AddPedalAsync(newPedal);

            await Shell.Current.GoToAsync("..");
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

    }

}
