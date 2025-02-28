using CardGameCorner.Models;
using SQLite;

public class SQLiteService
{
    private SQLiteAsyncConnection _database;
    string databasePath = Path.Combine(FileSystem.AppDataDirectory, "mylistdatabase.db3");

    public SQLiteService()
    {
        _database = new SQLiteAsyncConnection(databasePath);
    }

    // Initialize the database and apply migration if necessary
    public async Task Init()
    {
        // Ensure the table is created or migrated
        var tableInfo = await _database.GetTableInfoAsync("ProductList");

        if (tableInfo.Count == 0)
        {
            // If the table doesn't exist, create it
            await _database.CreateTableAsync<ProductList>();
        }
        else
        {
            MigrateDatabaseAsync();
        }
    }

    // Perform the database migration
    private async Task MigrateDatabaseAsync()
    {
        //// Create a new temporary table with the new schema (including the new columns)
        //await _database.ExecuteAsync("CREATE TABLE IF NOT EXISTS ProductList_temp AS SELECT * FROM ProductList;");

        //// Add new columns if they don't exist
        //await _database.ExecuteAsync("ALTER TABLE ProductList ADD COLUMN Evalution TEXT;");
       
        await _database.ExecuteAsync("ALTER TABLE ProductList ADD COLUMN Variants TEXT;");

        //// Copy data from the temp table back to the original table
        //await _database.ExecuteAsync("INSERT INTO ProductList SELECT * FROM ProductList_temp;");

        //// Drop the temporary table
        //await _database.ExecuteAsync("DROP TABLE ProductList_temp;");

        Console.WriteLine("Database migration completed!");
    }

    // Add an item to the list
    public async Task AddItemToListAsync(ProductList item)
    {
        await Init();

        if (item.Id != null && item.Id != 0)
        {
            await _database.UpdateAsync(item);
        }
        else
        {
            await _database.InsertAsync(item);
        }
    }

    // Get all items in the list
    public async Task<List<ProductList>> GetAllItemsAsync()
    {
        await Init();

        return await _database.Table<ProductList>().ToListAsync();
    }

    // Delete an item
    public async Task<int> DeleteItemAsync(ProductList item)
    {
        await Init();
        return await _database.DeleteAsync(item);
    }
}
