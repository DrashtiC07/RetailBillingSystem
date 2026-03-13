using System;
using System.Collections.Generic;

namespace RetailBillingSystem.Models
{
    /// <summary>
    /// Represents a sale transaction
    /// </summary>
    public class Sale
    {
        public int SaleID { get; set; }
        public int? CustomerID { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int? CreatedBy { get; set; }

        // Customer information
        public string CustomerName { get; set; }

        // Sale items
        public List<SaleItem> Items { get; set; }

        // Formatted values for display
        public string FormattedSaleDate => SaleDate.ToString("dd MMM yyyy HH:mm");
        public string FormattedSubtotal => Subtotal.ToString("C2");
        public string FormattedTax => TaxAmount.ToString("C2");
        public string FormattedTotal => TotalAmount.ToString("C2");

        public Sale()
        {
            SaleDate = DateTime.Now;
            Items = new List<SaleItem>();
        }
    }
}