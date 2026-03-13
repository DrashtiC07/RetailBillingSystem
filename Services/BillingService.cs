using Npgsql;
using RetailBillingSystem.Database;
using RetailBillingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace RetailBillingSystem.Services
{
    /// <summary>
    /// Service class for billing and sales-related database operations
    /// </summary>
    public class BillingService
    {
        private readonly ProductService _productService;

        public BillingService()
        {
            _productService = new ProductService();
        }

        /// <summary>
        /// Gets all sales (for admin)
        /// </summary>
        public List<Sale> GetAllSales()
        {
            var sales = new List<Sale>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT s.SaleID, s.CustomerID, s.SaleDate, s.Subtotal, s.TaxAmount, s.TotalAmount, 
                                COALESCE(c.FullName, 'Walk-in Customer') as CustomerName
                                FROM Sales s
                                LEFT JOIN Customers c ON s.CustomerID = c.CustomerID
                                ORDER BY s.SaleDate DESC";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sales.Add(MapSaleFromReader(reader));
                    }
                }
            }

            return sales;
        }

        /// <summary>
        /// Gets sales for a specific customer
        /// </summary>
        public List<Sale> GetSalesByCustomer(int customerId)
        {
            var sales = new List<Sale>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT s.SaleID, s.CustomerID, s.SaleDate, s.Subtotal, s.TaxAmount, s.TotalAmount, 
                                c.FullName as CustomerName
                                FROM Sales s
                                JOIN Customers c ON s.CustomerID = c.CustomerID
                                WHERE s.CustomerID = @CustomerID
                                ORDER BY s.SaleDate DESC";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sales.Add(MapSaleFromReader(reader));
                        }
                    }
                }
            }

            return sales;
        }

        /// <summary>
        /// Gets today's sales statistics
        /// </summary>
        public (int SaleCount, decimal TotalRevenue) GetTodaySales()
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT COUNT(*), COALESCE(SUM(TotalAmount), 0) 
                                FROM Sales 
                                WHERE DATE(SaleDate) = CURRENT_DATE";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int count = reader.GetInt32(0);
                        decimal revenue = reader.GetDecimal(1);
                        return (count, revenue);
                    }
                }
            }
            return (0, 0);
        }

        /// <summary>
        /// Gets total sales count for a customer
        /// </summary>
        public int GetCustomerOrderCount(int customerId)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Sales WHERE CustomerID = @CustomerID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Gets total items purchased by a customer
        /// </summary>
        public int GetCustomerTotalItems(int customerId)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT COALESCE(SUM(si.Quantity), 0) 
                                FROM SaleItems si
                                JOIN Sales s ON si.SaleID = s.SaleID
                                WHERE s.CustomerID = @CustomerID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Gets total money spent by a customer
        /// </summary>
        public decimal GetCustomerTotalSpent(int customerId)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COALESCE(SUM(TotalAmount), 0) FROM Sales WHERE CustomerID = @CustomerID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Gets sale items for a specific sale
        /// </summary>
        public List<SaleItem> GetSaleItems(int saleId)
        {
            var items = new List<SaleItem>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT si.SaleItemID, si.SaleID, si.ProductID, si.Quantity, si.UnitPrice, si.TotalPrice,
                                p.ProductName
                                FROM SaleItems si
                                JOIN Products p ON si.ProductID = p.ProductID
                                WHERE si.SaleID = @SaleID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SaleID", saleId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(MapSaleItemFromReader(reader));
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Creates a new sale with items (transaction-based)
        /// </summary>
        public bool CreateSale(Sale sale, List<SaleItem> items)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert sale
                        string saleQuery = @"INSERT INTO Sales (CustomerID, Subtotal, TaxAmount, TotalAmount, CreatedBy) 
                                            VALUES (@CustomerID, @Subtotal, @TaxAmount, @TotalAmount, @CreatedBy) 
                                            RETURNING SaleID";

                        int saleId;
                        using (var cmd = new NpgsqlCommand(saleQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", (object)sale.CustomerID ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Subtotal", sale.Subtotal);
                            cmd.Parameters.AddWithValue("@TaxAmount", sale.TaxAmount);
                            cmd.Parameters.AddWithValue("@TotalAmount", sale.TotalAmount);
                            cmd.Parameters.AddWithValue("@CreatedBy", (object)CurrentUser.UserID ?? DBNull.Value);

                            saleId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Insert sale items and update stock
                        string itemQuery = @"INSERT INTO SaleItems (SaleID, ProductID, Quantity, UnitPrice, TotalPrice) 
                                            VALUES (@SaleID, @ProductID, @Quantity, @UnitPrice, @TotalPrice)";

                        string stockQuery = "UPDATE Products SET Stock = Stock - @Quantity WHERE ProductID = @ProductID AND Stock >= @Quantity";

                        foreach (var item in items)
                        {
                            // Insert sale item
                            using (var cmd = new NpgsqlCommand(itemQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@SaleID", saleId);
                                cmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                                cmd.Parameters.AddWithValue("@TotalPrice", item.TotalPrice);
                                cmd.ExecuteNonQuery();
                            }

                            // Update product stock
                            using (var cmd = new NpgsqlCommand(stockQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected == 0)
                                {
                                    throw new Exception($"Insufficient stock for product ID: {item.ProductID}");
                                }
                            }
                        }

                        transaction.Commit();
                        sale.SaleID = saleId;
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Gets recent purchases for a customer
        /// </summary>
        public List<Sale> GetRecentPurchases(int customerId, int limit = 5)
        {
            var sales = new List<Sale>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT s.SaleID, s.CustomerID, s.SaleDate, s.Subtotal, s.TaxAmount, s.TotalAmount, 
                                c.FullName as CustomerName
                                FROM Sales s
                                JOIN Customers c ON s.CustomerID = c.CustomerID
                                WHERE s.CustomerID = @CustomerID
                                ORDER BY s.SaleDate DESC
                                LIMIT @Limit";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.Parameters.AddWithValue("@Limit", limit);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sales.Add(MapSaleFromReader(reader));
                        }
                    }
                }
            }

            return sales;
        }

        /// <summary>
        /// Maps a database reader to a Sale object
        /// </summary>
        private Sale MapSaleFromReader(NpgsqlDataReader reader)
        {
            return new Sale
            {
                SaleID = reader.GetInt32(0),
                CustomerID = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                SaleDate = reader.GetDateTime(2),
                Subtotal = reader.GetDecimal(3),
                TaxAmount = reader.GetDecimal(4),
                TotalAmount = reader.GetDecimal(5),
                CustomerName = reader.IsDBNull(6) ? "Walk-in Customer" : reader.GetString(6)
            };
        }

        /// <summary>
        /// Maps a database reader to a SaleItem object
        /// </summary>
        private SaleItem MapSaleItemFromReader(NpgsqlDataReader reader)
        {
            return new SaleItem
            {
                SaleItemID = reader.GetInt32(0),
                SaleID = reader.GetInt32(1),
                ProductID = reader.GetInt32(2),
                Quantity = reader.GetInt32(3),
                UnitPrice = reader.GetDecimal(4),
                TotalPrice = reader.GetDecimal(5),
                ProductName = reader.GetString(6)
            };
        }
    }
}