
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
    public class DatabaseService
    {

        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "GuitarStore.db3");
            Console.WriteLine($"Database path: {dbPath}");
            _database = new SQLiteAsyncConnection(dbPath);
            

            Task.Run(async () => await InitializeDatabase());

        }
        private async Task InitializeDatabase()
        {
            try
            {
                await _database.CreateTableAsync<Product>();
                await _database.CreateTableAsync<Guitar>();
                await _database.CreateTableAsync<Amp>();
                await _database.CreateTableAsync<Pedal>();
                await _database.CreateTableAsync<Accessory>();
                await _database.CreateTableAsync<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        // product
        public async Task<List<Product>> GetProductAsync()
        {
            return await _database.Table<Product>().ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int Id)
        {
            return await _database.Table<Product>().Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task AddProductAsync(Product product)
        {
            try
            {
                await _database.InsertAsync(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _database.UpdateAsync(product);
        }
        public async Task DeleteProductAsync(Product product)
        {
            await _database.DeleteAsync(product);
        }

        // guitar

        public async Task<List<Guitar>> GetGuitarAsync()
        {
            var guitars = await _database.Table<Guitar>().ToListAsync();
            var sortGuitars = new List<Guitar>();
            sortGuitars.AddRange(guitars);
            return sortGuitars.OrderBy(item => item.Make).ToList();
        }
        public async Task<Guitar> GetGuitarByIdAsync(int Id)
        {
            return await _database.Table<Guitar>().Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task AddGuitarAsync(Guitar guitar)
        {
            try
            {
                await _database.InsertAsync(guitar);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Adding Guitar: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateGuitarAsync(Guitar guitar)
        {
            await _database.UpdateAsync(guitar);
        }
        public async Task DeleteGuitarAsync(Guitar guitar)
        {
            await _database.DeleteAsync(guitar);
        }

        // amp

        public async Task<List<Amp>> GetAmpAsync()
        {
            var amps = await _database.Table<Amp>().ToListAsync();
            var sortAmps = new List<Amp>();
            sortAmps.AddRange(amps);
            return sortAmps.OrderBy(item => item.Make).ToList();
        }
        public async Task<Amp> GetAmpByIdAsync(int Id)
        {
            return await _database.Table<Amp>().Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task AddAmpAsync(Amp amp)
        {
            try
            {
                await _database.InsertAsync(amp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Adding Amp: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAmpAsync(Amp amp)
        {
            await _database.UpdateAsync(amp);
        }
        public async Task DeleteAmpAsync(Amp amp)
        {
            await _database.DeleteAsync(amp);
        }

        // pedal

        public async Task<List<Pedal>> GetPedalAsync()
        {
            var pedals = await _database.Table<Pedal>().ToListAsync();
            var sortPedals = new List<Pedal>();
            sortPedals.AddRange(pedals);
            return sortPedals.OrderBy(item => item.Make).ToList();
        }
        public async Task<Pedal> GetPedalByIdAsync(int Id)
        {
            return await _database.Table<Pedal>().Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task AddPedalAsync(Pedal pedal)
        {
            try
            {
                await _database.InsertAsync(pedal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Adding Pedal: {ex.Message}");
                throw;
            }
        }

        public async Task UpdatePedalAsync(Pedal pedal)
        {
            await _database.UpdateAsync(pedal);
        }
        public async Task DeletePedalAsync(Pedal pedal)
        {
            await _database.DeleteAsync(pedal);
        }

        // accessory

        public async Task<List<Accessory>> GetAccessoryAsync()
        {
            var accessories = await _database.Table<Accessory>().ToListAsync();
            var sortAccessories = new List<Accessory>();
            sortAccessories.AddRange(accessories);
            return sortAccessories.OrderBy(item => item.Make).ToList();
        }
        public async Task<Accessory> GetAccessoryByIdAsync(int Id)
        {
            return await _database.Table<Accessory>().Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task AddAccessoryAsync(Accessory accessory)
        {
            try
            {
                await _database.InsertAsync(accessory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Adding Accessory: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAccessoryAsync(Accessory accessory)
        {
            await _database.UpdateAsync(accessory);
        }
        public async Task DeleteAccessoryAsync(Accessory accessory)
        {
            await _database.DeleteAsync(accessory);
        }

        // total inventory

        public async Task<List<Product>> GetInventoryAsync()
        {
            var guitars = await _database.Table<Guitar>().ToListAsync();
            var amps = await _database.Table<Amp>().ToListAsync();
            var pedals = await _database.Table<Pedal>().ToListAsync();
            var accessories = await _database.Table<Accessory>().ToListAsync();

            var allItems = new List<Product>();
            allItems.AddRange(guitars);
            allItems.AddRange(amps);
            allItems.AddRange(pedals);
            allItems.AddRange(accessories);

            return allItems.OrderBy(item => item.Make).ToList();
        }

        // user

        public Task<User> GetCurrentUserAsync()
        {
            var currentUserId = SessionManager.GetCurrentUserId();
            if (currentUserId.HasValue)
            {
                return _database.Table<User>().Where(u => u.Id == currentUserId.Value).FirstOrDefaultAsync();
            }
            return Task.FromResult(UserService.Instance.CurrentUser);
        }
        public Task<User> GetUserAsync(string username, string password)
        {
            return _database.Table<User>().Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();

            
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }
    }
}
