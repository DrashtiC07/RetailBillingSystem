using RetailBillingSystem.Forms;
using System;
using System.Windows.Forms;

namespace RetailBillingSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the login form as the starting point
            Application.Run(new LoginForm());
        }
    }
}