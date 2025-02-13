using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GuitarStore.Models;
using GuitarStore.Services;

namespace GuitarStore.ViewModels
{
    public class AddGuitarViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string PhotoPath { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }

        public ICommand SaveCommand { get; }

        public AddGuitarViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            SaveCommand = new Command(async () => await SaveGuitarAsync());
        }

        private async Task SaveGuitarAsync()
        {
            var newGuitar = new Guitar
            {
                PhotoPath = PhotoPath,
                Make = Make,
                Model = Model,
                Price = Price
            };

            await _databaseService.AddGuitarAsync(newGuitar);

            await Shell.Current.GoToAsync("..");
        }

    }
    
}
