
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
            _database = new SQLiteAsyncConnection(dbPath);
            

            Task.Run(async () => await InitializeDatabase());

        }
        private async Task InitializeDatabase()
        {
            await _database.CreateTableAsync<Guitar>();
        }

        public async Task<List<Guitar>> GetGuitarAsync()
        {
            return await _database.Table<Guitar>().ToListAsync();
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
                Console.WriteLine($"Error adding guitar: {ex.Message}");
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
    }
}
