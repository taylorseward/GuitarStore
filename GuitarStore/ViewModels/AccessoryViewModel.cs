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
    public class AccessoryViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Accessory> Accessories { get; set; } = new();
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
                    SortAccessories();
                }
            }
        }
        public ObservableCollection<Accessory> SearchedAccessories { get; } = new();
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
                    SearchAccessories();
                }
            }
        }

        public IAsyncRelayCommand LoadAccessoriesCommand { get; }
        public IAsyncRelayCommand<Accessory> DeleteAccessoryCommand { get; }
        public ICommand SaveAccessoryCommand { get; }
        public IAsyncRelayCommand<Accessory> EditAccessoryCommand { get; }


        private Accessory _selectedAccessory;
        public Accessory SelectedAccessory
        {
            get => _selectedAccessory;
            set
            {
                if (_selectedAccessory != value)
                {
                    _selectedAccessory = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccessoryViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadAccessoriesCommand = new AsyncRelayCommand(LoadAccessoriesAsync);
            DeleteAccessoryCommand = new AsyncRelayCommand<Accessory>(DeleteAccessoryAsync);
            SaveAccessoryCommand = new Command(async () => await SaveAccessoryAsync());
            EditAccessoryCommand = new AsyncRelayCommand<Accessory>(EditAccessoryAsync);

        }

        private async Task LoadAccessoriesAsync()
        {
            var accessories = await _databaseService.GetAccessoryAsync();
            if (accessories != null)
            {
                Accessories.Clear();
                foreach (var accessory in accessories)
                {
                    Accessories.Add(accessory);
                }
                SearchedAccessories.Clear();
                foreach (var accessory in accessories)
                {
                    SearchedAccessories.Add(accessory);
                }
            }
        }
        public void SortAccessories()
        {
            var sortedList = Accessories.ToList();

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

            Accessories.Clear();
            foreach (var accessory in sortedList)
            {
                Accessories.Add(accessory);
            }

            SearchAccessories();
        }

        private void SearchAccessories()
        {
            SearchedAccessories.Clear();

            var searched = string.IsNullOrWhiteSpace(SearchQuery)
                ? Accessories
                : Accessories.Where
                (a =>
                a.Make.ToLower().Contains(SearchQuery.ToLower()) ||
                a.Model.ToLower().Contains(SearchQuery.ToLower()) ||
                a.AccessoryType.ToLower().Contains(SearchQuery.ToLower())
                );

            foreach (var accessory in searched)
            {
                SearchedAccessories.Add(accessory);
            }
        }

        private async Task DeleteAccessoryAsync(Accessory accessory)
        {
            if (accessory != null)
            {
                await _databaseService.DeleteAccessoryAsync(accessory);
                Accessories.Remove(accessory);
            }
        }
        private async Task SaveAccessoryAsync()
        {
            if (SelectedAccessory != null)
            {
                if (SelectedAccessory.Id == 0)
                {
                    // Add
                    await _databaseService.AddAccessoryAsync(SelectedAccessory);
                    Accessories.Add(SelectedAccessory);
                }
                else
                {
                    // Update
                    await _databaseService.UpdateAccessoryAsync(SelectedAccessory);
                    await LoadAccessoriesAsync(); // Refresh 
                }

                SelectedAccessory = null;  // Reset 
            }

        }
        private async Task EditAccessoryAsync(Accessory accessory)
        {
            if (accessory != null)
            {
                await Shell.Current.GoToAsync($"{nameof(AddAccessoryPage)}?accessoryId={accessory.Id}");
            }

        }
    }
}
