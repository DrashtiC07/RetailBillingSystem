using System;

namespace RetailBillingSystem.Models
{
    /// <summary>
    /// Represents a product in the inventory
    /// </summary>
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Computed property to check if stock is low
        public bool IsLowStock => Stock < 10;

        // Formatted price for display
        public string FormattedPrice => Price.ToString("C2");

        public Product()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}