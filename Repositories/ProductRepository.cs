using Dapper;
using Microsoft.Data.Sqlite;
using InventoryWebApp.Models;

namespace InventoryWebApp.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString = "Data Source=inventory.db";

        public ProductRepository()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Price REAL NOT NULL
                )");
        }

        public IEnumerable<Product> GetAll(string? search = null)
        {
            using var connection = new SqliteConnection(_connectionString);
            if (string.IsNullOrWhiteSpace(search))
                return connection.Query<Product>("SELECT * FROM Products ORDER BY Id DESC");
            else
                return connection.Query<Product>(
                    "SELECT * FROM Products WHERE Name LIKE @search ORDER BY Id DESC",
                    new { search = "%" + search + "%" });
        }

        public Product? GetById(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            return connection.QueryFirstOrDefault<Product>(
                "SELECT * FROM Products WHERE Id = @id", new { id });
        }

        public void Add(Product product)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Execute("INSERT INTO Products (Name, Quantity, Price) VALUES (@Name, @Quantity, @Price)", product);
        }

        public void Update(Product product)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Execute("UPDATE Products SET Name=@Name, Quantity=@Quantity, Price=@Price WHERE Id=@Id", product);
        }

        public void Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Execute("DELETE FROM Products WHERE Id=@id", new { id });
        }
    }
}
