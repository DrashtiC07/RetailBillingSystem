using System;

namespace RetailBillingSystem.Models
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Static class to hold current logged-in user session
    /// </summary>
    public static class CurrentUser
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }
        public static int? CustomerID { get; set; }

        public static bool IsAdmin => Role == "Admin";
        public static bool IsCustomer => Role == "Customer";

        public static void Clear()
        {
            UserID = 0;
            Username = null;
            Role = null;
            CustomerID = null;
        }
    }
}