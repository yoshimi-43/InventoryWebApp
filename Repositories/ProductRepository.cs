using System.Collections.Generic;
using Dapper;
using Microsoft.Data.Sqlite;
using InventoryWebApp.Models;
using System.Linq;

namespace InventoryWebApp.Repositories
{
    public class ProductRepository
    {
        private readonly string _connectionString = "Data Source=inventory.db";

        public ProductRepository()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            conn.Execute(@"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Price REAL NOT NULL
                );");
        }

        // ページング付き取得（検索ワード optional）
        public PagedResult<Product> GetPaged(string? search, int page, int pageSize)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var where = "";
            var parameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(search))
            {
                where = "WHERE Name LIKE @Search";
                parameters.Add("@Search", $"%{search}%");
            }

            var countSql = $"SELECT COUNT(1) FROM Products {where};";
            var total = conn.ExecuteScalar<int>(countSql, parameters);

            var offset = (page - 1) * pageSize;
            var listSql = $@"
                SELECT Id, Name, Quantity, Price
                FROM Products
                {where}
                ORDER BY Id DESC
                LIMIT @PageSize OFFSET @Offset;";

            parameters.Add("@PageSize", pageSize);
            parameters.Add("@Offset", offset);

            var items = conn.Query<Product>(listSql, parameters).ToList();

            return new PagedResult<Product>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public IEnumerable<Product> GetAll(string? search = null)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            if (string.IsNullOrWhiteSpace(search))
                return conn.Query<Product>("SELECT Id,Name,Quantity,Price FROM Products ORDER BY Id DESC");
            return conn.Query<Product>("SELECT Id,Name,Quantity,Price FROM Products WHERE Name LIKE @Search ORDER BY Id DESC",
                new { Search = $"%{search}%" });
        }

        public Product? GetById(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            return conn.QuerySingleOrDefault<Product>("SELECT Id,Name,Quantity,Price FROM Products WHERE Id = @Id", new { Id = id });
        }

        public void Add(Product p)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            conn.Execute("INSERT INTO Products (Name, Quantity, Price) VALUES (@Name, @Quantity, @Price);", p);
        }

        public void Update(Product p)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            conn.Execute("UPDATE Products SET Name=@Name, Quantity=@Quantity, Price=@Price WHERE Id=@Id;", p);
        }

        public void Delete(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            conn.Execute("DELETE FROM Products WHERE Id=@Id;", new { Id = id });
        }
    }
}
