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
                    SortGuitars();
                }
            }
        }
        public ObservableCollection<Guitar> SearchedGuitars { get; } = new();
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
                    SearchGuitars();
                }
            }
        }

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

                SearchedGuitars.Clear();
                foreach (var guitar in guitars)
                {
                    SearchedGuitars.Add(guitar);
                }
            }
        }

        public void SortGuitars()
        {
            var sortedList = Guitars.ToList();

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

            Guitars.Clear();
            foreach (var guitar in sortedList)
            {
                Guitars.Add(guitar);
            }

            SearchGuitars();
        }

        private void SearchGuitars()
        {
            SearchedGuitars.Clear();

            var searched = string.IsNullOrWhiteSpace(SearchQuery)
                ? Guitars
                : Guitars.Where
                (a =>
                a.Make.ToLower().Contains(SearchQuery.ToLower()) ||
                a.Model.ToLower().Contains(SearchQuery.ToLower()) ||
                a.GuitarType.ToLower().Contains(SearchQuery.ToLower())
                );

            foreach (var guitar in searched)
            {
                SearchedGuitars.Add(guitar);
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
