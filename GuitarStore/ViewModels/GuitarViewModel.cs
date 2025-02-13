using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GuitarStore.Models;
using GuitarStore.Services;
using CommunityToolkit.Mvvm.Input;
using GuitarStore.Views;

namespace GuitarStore.ViewModels
{
    public class GuitarViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Guitar> Guitars { get; set; } = new();

        public IAsyncRelayCommand LoadGuitarsCommand { get; }
        public IAsyncRelayCommand<Guitar> DeleteGuitarCommand { get; }
        public ICommand SaveGuitarCommand { get; }
        public IAsyncRelayCommand<Guitar> EditGuitarCommand { get; }


        private Guitar _selectedGuitar;
        public Guitar SelectedGuitar
        {
            get => _selectedGuitar;
            set
            {
                if (_selectedGuitar != value)
                {
                    _selectedGuitar = value;
                    OnPropertyChanged();  
                }
            }
        }

        public GuitarViewModel (DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadGuitarsCommand = new AsyncRelayCommand(LoadGuitarsAsync);
            DeleteGuitarCommand = new AsyncRelayCommand<Guitar>(DeleteGuitarAsync);
            SaveGuitarCommand = new Command(async () => await SaveGuitarAsync());
            EditGuitarCommand = new AsyncRelayCommand<Guitar>(EditGuitarAsync);

        }

        private async Task LoadGuitarsAsync()
        {
            var guitars = await _databaseService.GetGuitarAsync();
            if (guitars != null)
            {
                Guitars.Clear();
                foreach (var guitar in guitars)
                {
                    Guitars.Add(guitar);
                }
            }
        }

        private async Task DeleteGuitarAsync(Guitar guitar)
        {
            if (guitar != null)
            {
                await _databaseService.DeleteGuitarAsync(guitar);
                Guitars.Remove(guitar);
            }
        }
        private async Task SaveGuitarAsync()
        {
            if (SelectedGuitar != null)
            {
                if (SelectedGuitar.Id == 0)
                {
                    // Adding a new guitar
                    await _databaseService.AddGuitarAsync(SelectedGuitar);
                    Guitars.Add(SelectedGuitar);
                }
                else
                {
                    // Updating an existing guitar
                    await _databaseService.UpdateGuitarAsync(SelectedGuitar);
                    await LoadGuitarsAsync(); // Refresh list
                }

                SelectedGuitar = null;  // Reset selection
            }

        }
        private async Task EditGuitarAsync(Guitar guitar)
        {
            if (guitar != null)
            {
                await Shell.Current.GoToAsync($"{nameof(AddGuitarPage)}?guitarId={guitar.Id}");
            }
            
        }
    }
}
