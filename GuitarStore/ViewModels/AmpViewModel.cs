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
    public class AmpViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Amp> Amps { get; set; } = new();
        public ObservableCollection<string> SortOptions { get; } = new()
        {
            "Make (A-Z)",
            "Make (Z-A)",
            "Price (Low-High)",
            "Price (High-Low)",
            "Recently Added"
        };

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (_selectedSortOption != value)
                {
                    _selectedSortOption = value;
                    OnPropertyChanged();
                    SortAmps();
                }
            }
        }
        public ObservableCollection<Amp> SearchedAmps { get; } = new();
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    SearchAmps();
                }
            }
        }

        public IAsyncRelayCommand LoadAmpsCommand { get; }
        public IAsyncRelayCommand<Amp> DeleteAmpCommand { get; }
        public ICommand SaveAmpCommand { get; }
        public IAsyncRelayCommand<Amp> EditAmpCommand { get; }


        private Amp _selectedAmp;
        public Amp SelectedAmp
        {
            get => _selectedAmp;
            set
            {
                if (_selectedAmp != value)
                {
                    _selectedAmp = value;
                    OnPropertyChanged();
                }
            }
        }

        public AmpViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadAmpsCommand = new AsyncRelayCommand(LoadAmpsAsync);
            DeleteAmpCommand = new AsyncRelayCommand<Amp>(DeleteAmpAsync);
            SaveAmpCommand = new Command(async () => await SaveAmpAsync());
            EditAmpCommand = new AsyncRelayCommand<Amp>(EditAmpAsync);

        }

        private async Task LoadAmpsAsync()
        {
            var amps = await _databaseService.GetAmpAsync();
            if (amps != null)
            {
                Amps.Clear();
                foreach (var amp in amps)
                {
                    Amps.Add(amp);
                }
                
                SearchedAmps.Clear();
                foreach (var amp in amps)
                {
                    SearchedAmps.Add(amp);
                }
            }
        }

        public void SortAmps()
        {
            var sortedList = Amps.ToList();

            switch (SelectedSortOption)
            {
                case "Make (A-Z)":
                    sortedList = sortedList.OrderBy(p => p.Make).ToList();
                    break;
                case "Make (Z-A)":
                    sortedList = sortedList.OrderByDescending(p => p.Make).ToList();
                    break;
                case "Price (Low-High)":
                    sortedList = sortedList.OrderBy(p => p.Price).ToList();
                    break;
                case "Price (High-Low)":
                    sortedList = sortedList.OrderByDescending(p => p.Price).ToList();
                    break;
                case "Recently Added":
                    sortedList = sortedList.OrderByDescending(p => p.Id).ToList();
                    break;
            }

            Amps.Clear();
            foreach (var amp in sortedList)
            {
                Amps.Add(amp);
            }

            SearchAmps();
        }

        private void SearchAmps()
        {
            SearchedAmps.Clear();

            var searched = string.IsNullOrWhiteSpace(SearchQuery)
                ? Amps
                : Amps.Where
                (a =>
                a.Make.ToLower().Contains(SearchQuery.ToLower()) ||
                a.Model.ToLower().Contains(SearchQuery.ToLower()) ||
                a.AmpType.ToLower().Contains(SearchQuery.ToLower())
                );

            foreach (var amp in searched)
            {
                SearchedAmps.Add(amp);
            }
        }
        private async Task DeleteAmpAsync(Amp amp)
        {
            if (amp != null)
            {
                await _databaseService.DeleteAmpAsync(amp);
                Amps.Remove(amp);
            }
        }
        private async Task SaveAmpAsync()
        {
            if (SelectedAmp != null)
            {
                if (SelectedAmp.Id == 0)
                {
                    // Add
                    await _databaseService.AddAmpAsync(SelectedAmp);
                    Amps.Add(SelectedAmp);
                }
                else
                {
                    // Update
                    await _databaseService.UpdateAmpAsync(SelectedAmp);
                    await LoadAmpsAsync(); // Refresh 
                }

                SelectedAmp = null;  // Reset 
            }

        }
        private async Task EditAmpAsync(Amp amp)
        {
            if (amp != null)
            {
                await Shell.Current.GoToAsync($"{nameof(AddAmpPage)}?ampId={amp.Id}");
            }

        }
    }
}
