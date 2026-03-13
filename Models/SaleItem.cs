namespace RetailBillingSystem.Models
{
    /// <summary>
    /// Represents an item in a sale transaction
    /// </summary>
    public class SaleItem
    {
        public int SaleItemID { get; set; }
        public int SaleID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // Product information
        public string ProductName { get; set; }

        // Formatted values for display
        public string FormattedUnitPrice => UnitPrice.ToString("C2");
        public string FormattedTotalPrice => TotalPrice.ToString("C2");

        public SaleItem()
        {
        }

        public SaleItem(Product product, int quantity)
        {
            ProductID = product.ProductID;
            ProductName = product.ProductName;
            UnitPrice = product.Price;
            Quantity = quantity;
            CalculateTotal();
        }

        public void CalculateTotal()
        {
            TotalPrice = UnitPrice * Quantity;
        }
    }
}