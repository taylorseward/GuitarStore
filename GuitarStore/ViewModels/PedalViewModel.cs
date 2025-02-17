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
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace GuitarStore.ViewModels
{
    public class PedalViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Pedal> Pedals { get; set; } = new();

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
                    SortPedals();
                }
            }
        }
        public ObservableCollection<Pedal> SearchedPedals { get; } = new();
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
                    SearchPedals();
                }
            }
        }

        public IAsyncRelayCommand LoadPedalsCommand { get; }
        public IAsyncRelayCommand<Pedal> DeletePedalCommand { get; }
        public ICommand SavePedalCommand { get; }
        public IAsyncRelayCommand<Pedal> EditPedalCommand { get; }


        private Pedal _selectedPedal;
        public Pedal SelectedPedal
        {
            get => _selectedPedal;
            set
            {
                if (_selectedPedal != value)
                {
                    _selectedPedal = value;
                    OnPropertyChanged();
                }
            }
        }

        public PedalViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadPedalsCommand = new AsyncRelayCommand(LoadPedalsAsync);
            DeletePedalCommand = new AsyncRelayCommand<Pedal>(DeletePedalAsync);
            SavePedalCommand = new Command(async () => await SavePedalAsync());
            EditPedalCommand = new AsyncRelayCommand<Pedal>(EditPedalAsync);

        }

        private async Task LoadPedalsAsync()
        {
            var pedals = await _databaseService.GetPedalAsync();
            if (pedals != null)
            {
                Pedals.Clear();
                foreach (var pedal in pedals)
                {
                    Pedals.Add(pedal);
                }
                SearchedPedals.Clear();
                foreach (var pedal in pedals)
                {
                    SearchedPedals.Add(pedal);
                }
            }
        }
        public void SortPedals()
        {
            var sortedList = Pedals.ToList();

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

            Pedals.Clear();
            foreach (var pedal in sortedList)
            {
                Pedals.Add(pedal);
            }

            SearchPedals();
        }

        private void SearchPedals()
        {
            SearchedPedals.Clear();

            var searched = string.IsNullOrWhiteSpace(SearchQuery)
                ? Pedals
                : Pedals.Where
                (a =>
                a.Make.ToLower().Contains(SearchQuery.ToLower()) ||
                a.Model.ToLower().Contains(SearchQuery.ToLower()) ||
                a.PedalType.ToLower().Contains(SearchQuery.ToLower())
                );

            foreach (var pedal in searched)
            {
                SearchedPedals.Add(pedal);
            }
        }

        private async Task DeletePedalAsync(Pedal pedal)
        {
            if (pedal != null)
            {
                await _databaseService.DeletePedalAsync(pedal);
                Pedals.Remove(pedal);
            }
        }
        private async Task SavePedalAsync()
        {
            if (SelectedPedal != null)
            {
                if (SelectedPedal.Id == 0)
                {
                    // Add
                    await _databaseService.AddPedalAsync(SelectedPedal);
                    Pedals.Add(SelectedPedal);
                }
                else
                {
                    // Update
                    await _databaseService.UpdatePedalAsync(SelectedPedal);
                    await LoadPedalsAsync(); // Refresh 
                }

                SelectedPedal = null;  // Reset 
            }

        }
        private async Task EditPedalAsync(Pedal pedal)
        {
            if (pedal != null)
            {
                await Shell.Current.GoToAsync($"{nameof(AddPedalPage)}?pedalId={pedal.Id}");
            }

        }
    }
}
