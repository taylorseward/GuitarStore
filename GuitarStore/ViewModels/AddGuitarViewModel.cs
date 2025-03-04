﻿using System;
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
    public class AddGuitarViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string PhotoPath { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }

        private string _selectedGuitarType;
        public string SelectedGuitarType
        {
            get => _selectedGuitarType;
            set
            {
                if (_selectedGuitarType != value)
                {
                    _selectedGuitarType = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> GuitarTypes { get; } = new ObservableCollection<string>
        {
            "Electric",
            "Acoustic"
        };
        private string _numberOfStrings;
        public string NumberOfStrings
        {
            get => _numberOfStrings;
            set
            {
                if (_numberOfStrings != value)
                {
                    _numberOfStrings = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<string> StringNumberPicker { get; } = new ObservableCollection<string>
        {
            "6",
            "7",
            "8"
        };


        public ICommand SaveCommand { get; }

        public AddGuitarViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            SaveCommand = new Command(async () => await SaveGuitarAsync());
        }

        private async Task SaveGuitarAsync()
        {
            if (string.IsNullOrWhiteSpace(Make) || string.IsNullOrWhiteSpace(Model))
            {
                await Shell.Current.DisplayAlert("Error", "Make and Model are required.", "OK");
                return;
            }
            var newGuitar = new Guitar
            {
                PhotoPath = PhotoPath,
                Make = Make,
                Model = Model,
                GuitarType = SelectedGuitarType,
                NumberOfStrings = NumberOfStrings,
                Price = Price
            };

            await _databaseService.AddGuitarAsync(newGuitar);

            await Shell.Current.GoToAsync("..");
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

    }
    
}
