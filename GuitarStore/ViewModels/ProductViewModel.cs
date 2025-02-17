using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GuitarStore.Models;
using GuitarStore.Services;
using CommunityToolkit.Mvvm.Input;
using GuitarStore.Views;
using System.Linq;

namespace GuitarStore.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }
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
                    SortProducts();
                }
            }
        }
        public ObservableCollection<Product> SearchedProducts { get; } = new();
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
                    SearchProducts();
                }
            }
        }

        

        public Command LoadProductsCommand { get; }
        public IAsyncRelayCommand<Product> DeleteProductCommand { get; }

        public ProductViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Products = new ObservableCollection<Product>();
            LoadProductsCommand = new Command(async () => await LoadProductsAsync());
        }

        private async Task LoadProductsAsync()
        {
            var products = await _databaseService.GetInventoryAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }

            
            SearchedProducts.Clear();
            foreach (var product in Products)
            {
                SearchedProducts.Add(product);
            }
        }
        public void SortProducts()
        {
            var sortedList = Products.ToList();

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

            Products.Clear();
            foreach (var product in sortedList)
            {
                Products.Add(product);
            }
            SearchProducts();
        }

        private void SearchProducts()
        {
            SearchedProducts.Clear();

            var searched = string.IsNullOrWhiteSpace(SearchQuery)
                ? Products
                : Products.Where
                (a => 
                a.Make.ToLower().Contains(SearchQuery.ToLower()) ||
                a.Model.ToLower().Contains(SearchQuery.ToLower()) ||
                a.ProductType.ToLower().Contains(SearchQuery.ToLower()) ||
                (a is Guitar guitar && (
                guitar.GuitarType.ToLower().Contains(SearchQuery.ToLower()) ||
                guitar.NumberOfStrings.ToString().Contains(SearchQuery.ToString()) 
                )) ||
                (a is Amp amp && (
                amp.AmpType.ToLower().Contains(SearchQuery.ToLower())
                )) ||
                (a is Pedal pedal && (
                pedal.PedalType.ToLower().Contains(SearchQuery.ToLower())
                )) ||
                (a is Accessory accessory && (
                accessory.AccessoryType.ToLower().Contains(SearchQuery.ToLower())
                ))
                );

            foreach (var product in searched)
            {
                SearchedProducts.Add(product);
            }
        }
    }

}
