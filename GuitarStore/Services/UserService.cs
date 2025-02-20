using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GuitarStore.Models;

namespace GuitarStore.Services
{
    public class UserService
    {
        private static UserService _instance;
        public static UserService Instance => _instance ??= new UserService();

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value; // You might want to implement INotifyPropertyChanged for this property
        }
    }
}
