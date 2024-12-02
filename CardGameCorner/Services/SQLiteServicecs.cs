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
        private SQLiteAsyncConnection _database;

        string databasePath = Path.Combine(FileSystem.AppDataDirectory, "mylistdatabase.db3");

        public SQLiteService()
        {
           
          

           
        }
        //async Task Init()
        //{
        //    if (_database is not null)
        //        return;

        //    _database = new SQLiteAsyncConnection(databasePath);
        //    var result = await _database.CreateTableAsync<ProductList>(); result = await _database.CreateTableAsync<ProductList>();
        //}

        async Task Init()
        {
            if (_database is not null)
                return;

            _database = new SQLiteAsyncConnection(databasePath);

            // Check if table exists before creating
            var tableInfo = await _database.GetTableInfoAsync("ProductList");
            if (tableInfo.Count == 0)
            {
                await _database.CreateTableAsync<ProductList>();
            }
            else
            {
                Console.WriteLine("Exist");
            }
        }


        // Ensure the table is created before performing any database operations
        //public async Task InitializeDatabaseAsync()
        //{
        //    bool tableExists = await CheckTableExistsAsync();

        //    if (tableExists)
        //    {
        //        await _database.CreateTableAsync<ProductList>();
        //    }
        //}

        //public async Task<bool> CheckTableExistsAsync()
        //{
        //    try
        //    {
        //        // Attempt to query the table
        //        var count = await _database.Table<ProductList>().CountAsync();
        //        return true;
        //    }
        //    catch (SQLiteException e)
        //    {
        //        // Table doesn't exist
        //        return false;
        //    }
        //}

        // Add a new item to the list in SQLite
        public async Task AddItemToListAsync(ProductList item)
        {
            // Ensure the table is created before adding an item
            await Init();
            if (item.Id != 0 && item.Id!=null)
            {
                 await _database.UpdateAsync(item);
            }
            else
            {
                 await _database.InsertAsync(item);
            }
        }
        

        // Get all items in the list from SQLite
        public async Task<List<ProductList>> GetAllItemsAsync()
        {
            await Init();

            return await _database.Table<ProductList>().ToListAsync();
        }

        public async Task<int> DeleteItemAsync(ProductList item)
        {
            await Init();
            return await _database.DeleteAsync(item);
        }
    }
}
