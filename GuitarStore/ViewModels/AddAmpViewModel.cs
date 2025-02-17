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
    public class AddAmpViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string PhotoPath { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }

        private string _selectedAmpType;
        public string SelectedAmpType
        {
            get => _selectedAmpType;
            set
            {
                if (_selectedAmpType != value)
                {
                    _selectedAmpType = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> AmpTypes { get; } = new ObservableCollection<string>
        {
            "Head",
            "Cabinet",
            "Combo"
        };
        
        


        public ICommand SaveCommand { get; }

        public AddAmpViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            SaveCommand = new Command(async () => await SaveAmpAsync());
        }

        private async Task SaveAmpAsync()
        {
            if (string.IsNullOrWhiteSpace(Make) || string.IsNullOrWhiteSpace(Model))
            {
                await Shell.Current.DisplayAlert("Error", "Make and Model are required.", "OK");
                return;
            }
            var newAmp = new Amp
            {
                PhotoPath = PhotoPath,
                Make = Make,
                Model = Model,
                AmpType = SelectedAmpType,
                Price = Price
            };

            await _databaseService.AddAmpAsync(newAmp);

            await Shell.Current.GoToAsync("..");
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

    }

}
