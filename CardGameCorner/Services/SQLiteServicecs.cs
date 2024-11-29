using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameCorner.Models;
using SQLite;

namespace CardGameCorner.Services
{
    public class SQLiteService
    {

        private readonly SQLiteAsyncConnection _database;

        public SQLiteService()
        {
            // Path to save the SQLite database in the app's local folder
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "mylist.db3");
            _database = new SQLiteAsyncConnection(databasePath);

            // Create the table if it doesn't exist
            _database.CreateTableAsync<ProductList>().Wait();
        }

        // Add a new item to the list in SQLite
        public async Task AddItemToListAsync(ProductList item)
        {
            await _database.InsertAsync(item);
        }

        // Get all items in the list from SQLite
        public async Task<List<ProductList>> GetAllItemsAsync()
        {
            return await _database.Table<ProductList>().ToListAsync();
        }
    }
}
