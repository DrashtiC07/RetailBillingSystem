using Npgsql;
using RetailBillingSystem.Database;
using RetailBillingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace RetailBillingSystem.Services
{
    /// <summary>
    /// Service class for product-related database operations
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// Gets all products from the database
        /// </summary>
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT ProductID, ProductName, Price, Stock, Description, CreatedAt, UpdatedAt FROM Products ORDER BY ProductName";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(MapProductFromReader(reader));
                    }
                }
            }

            return products;
        }

        /// <summary>
        /// Gets a product by ID
        /// </summary>
        public Product GetProductById(int productId)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT ProductID, ProductName, Price, Stock, Description, CreatedAt, UpdatedAt FROM Products WHERE ProductID = @ProductID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapProductFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets products with low stock (less than 10)
        /// </summary>
        public List<Product> GetLowStockProducts()
        {
            var products = new List<Product>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT ProductID, ProductName, Price, Stock, Description, CreatedAt, UpdatedAt FROM Products WHERE Stock < 10 ORDER BY Stock ASC";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(MapProductFromReader(reader));
                    }
                }
            }

            return products;
        }

        /// <summary>
        /// Gets the count of low stock products
        /// </summary>
        public int GetLowStockCount()
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Products WHERE Stock < 10";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Gets the total count of products
        /// </summary>
        public int GetTotalProductCount()
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Products";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Searches products by name
        /// </summary>
        public List<Product> SearchProducts(string searchTerm)
        {
            var products = new List<Product>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT ProductID, ProductName, Price, Stock, Description, CreatedAt, UpdatedAt FROM Products WHERE ProductName ILIKE @SearchTerm ORDER BY ProductName";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(MapProductFromReader(reader));
                        }
                    }
                }
            }

            return products;
        }

        /// <summary>
        /// Adds a new product
        /// </summary>
        public bool AddProduct(Product product)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Products (ProductName, Price, Stock, Description) 
                                VALUES (@ProductName, @Price, @Stock, @Description) 
                                RETURNING ProductID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    cmd.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        product.ProductID = Convert.ToInt32(result);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        public bool UpdateProduct(Product product)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Products 
                                SET ProductName = @ProductName, Price = @Price, Stock = @Stock, Description = @Description 
                                WHERE ProductID = @ProductID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    cmd.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        public bool DeleteProduct(int productId)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Products WHERE ProductID = @ProductID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Updates product stock after a sale
        /// </summary>
        public bool UpdateStock(int productId, int quantity)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Products SET Stock = Stock - @Quantity WHERE ProductID = @ProductID AND Stock >= @Quantity";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Maps a database reader to a Product object
        /// </summary>
        private Product MapProductFromReader(NpgsqlDataReader reader)
        {
            return new Product
            {
                ProductID = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Price = reader.GetDecimal(2),
                Stock = reader.GetInt32(3),
                Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5),
                UpdatedAt = reader.GetDateTime(6)
            };
        }
    }
}