using System;

namespace RetailBillingSystem.Models
{
    /// <summary>
    /// Represents a customer in the system
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }

        // Related user information
        public string Username { get; set; }

        public Customer()
        {
            CreatedAt = DateTime.Now;
        }
    }
}