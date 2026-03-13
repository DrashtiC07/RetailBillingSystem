using Guna.UI2.WinForms;
using Npgsql;
using RetailBillingSystem.Database;
using RetailBillingSystem.Models;
using RetailBillingSystem.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Forms
{
    public partial class CustomerForm : Form
    {
        private Guna2DataGridView dgvCustomers;
        private Guna2TextBox txtSearch;
        private Guna2Button btnDelete;
        private Guna2Button btnRefresh;

        public CustomerForm()
        {
            InitializeComponent();
            InitializeCustomerForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomerForm
            // 
            this.ClientSize = new Size(950, 700);
            this.Name = "CustomerForm";
            this.Text = "Customer Management";
            this.BackColor = UIHelper.MainBackground;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ResumeLayout(false);
        }

        private void InitializeCustomerForm()
        {
            // Main container panel
            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = UIHelper.MainBackground;
            mainPanel.Padding = new Padding(10, 10, 10, 10);
            mainPanel.AutoScroll = true;
            this.Controls.Add(mainPanel);

            // Title section
            var titlePanel = new Panel();
            titlePanel.Dock = DockStyle.Top;
            titlePanel.Height = 60;
            titlePanel.BackColor = Color.Transparent;
            mainPanel.Controls.Add(titlePanel);

            // Title
            var lblTitle = new Label();
            lblTitle.Text = "Customer Management";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(0, 5);
            titlePanel.Controls.Add(lblTitle);

            // Subtitle
            var lblSubtitle = new Label();
            lblSubtitle.Text = "View and manage registered customers";
            lblSubtitle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblSubtitle.ForeColor = UIHelper.TextSecondary;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(0, 38);
            titlePanel.Controls.Add(lblSubtitle);

            // Content panel
            var contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.Transparent;
            contentPanel.Padding = new Padding(0, 70, 0, 0);
            mainPanel.Controls.Add(contentPanel);

            // Top action bar using FlowLayoutPanel for responsive layout
            var actionPanel = new FlowLayoutPanel();
            actionPanel.Dock = DockStyle.Top;
            actionPanel.Height = 55;
            actionPanel.BackColor = Color.Transparent;
            actionPanel.FlowDirection = FlowDirection.LeftToRight;
            actionPanel.WrapContents = false;
            actionPanel.Padding = new Padding(0, 5, 0, 5);
            contentPanel.Controls.Add(actionPanel);

            // Search textbox
            txtSearch = new Guna2TextBox();
            UIHelper.StyleTextBox(txtSearch, "Search customers by name, phone or email...");
            txtSearch.Size = new Size(400, 42);
            txtSearch.Margin = new Padding(0, 0, 15, 0);
            txtSearch.TextChanged += TxtSearch_TextChanged;
            actionPanel.Controls.Add(txtSearch);

            // Refresh button
            btnRefresh = new Guna2Button();
            UIHelper.StylePrimaryButton(btnRefresh, "Refresh");
            btnRefresh.Size = new Size(120, 42);
            btnRefresh.Margin = new Padding(0, 0, 15, 0);
            btnRefresh.Click += BtnRefresh_Click;
            actionPanel.Controls.Add(btnRefresh);

            // Delete button
            btnDelete = new Guna2Button();
            UIHelper.StyleDangerButton(btnDelete, "Delete Selected");
            btnDelete.Size = new Size(150, 42);
            btnDelete.Margin = new Padding(0);
            btnDelete.Click += BtnDelete_Click;
            btnDelete.Enabled = false;
            actionPanel.Controls.Add(btnDelete);

            // Customers DataGridView
            dgvCustomers = new Guna2DataGridView();
            dgvCustomers.Dock = DockStyle.Fill;
            UIHelper.StyleDataGridView(dgvCustomers);
            dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;
            contentPanel.Controls.Add(dgvCustomers);

            // Configure columns
            dgvCustomers.Columns.Add("CustomerID", "ID");
            dgvCustomers.Columns.Add("FullName", "Full Name");
            dgvCustomers.Columns.Add("Username", "Username");
            dgvCustomers.Columns.Add("Phone", "Phone");
            dgvCustomers.Columns.Add("Email", "Email");
            dgvCustomers.Columns.Add("Address", "Address");
            dgvCustomers.Columns.Add("CreatedAt", "Registered Date");

            dgvCustomers.Columns["CustomerID"].Visible = false;
            dgvCustomers.Columns["FullName"].FillWeight = 20;
            dgvCustomers.Columns["Username"].FillWeight = 15;
            dgvCustomers.Columns["Phone"].FillWeight = 15;
            dgvCustomers.Columns["Email"].FillWeight = 20;
            dgvCustomers.Columns["Address"].FillWeight = 20;
            dgvCustomers.Columns["CreatedAt"].FillWeight = 10;

            // Load customers
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                dgvCustomers.Rows.Clear();
                var customers = GetAllCustomers();

                foreach (var customer in customers)
                {
                    dgvCustomers.Rows.Add(
                        customer.CustomerID,
                        customer.FullName,
                        customer.Username,
                        customer.Phone,
                        customer.Email,
                        customer.Address,
                        customer.CreatedAt.ToString("dd MMM yyyy")
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error loading customers: {ex.Message}");
            }
        }

        private List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT c.CustomerID, c.UserID, c.FullName, c.Phone, c.Email, c.Address, c.CreatedAt,
                                u.Username
                                FROM Customers c
                                JOIN Users u ON c.UserID = u.UserID
                                ORDER BY c.FullName";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            FullName = reader.GetString(2),
                            Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                            CreatedAt = reader.GetDateTime(6),
                            Username = reader.GetString(7)
                        });
                    }
                }
            }

            return customers;
        }

        private List<Customer> SearchCustomers(string searchTerm)
        {
            var customers = new List<Customer>();

            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT c.CustomerID, c.UserID, c.FullName, c.Phone, c.Email, c.Address, c.CreatedAt,
                                u.Username
                                FROM Customers c
                                JOIN Users u ON c.UserID = u.UserID
                                WHERE c.FullName ILIKE @SearchTerm 
                                OR c.Phone ILIKE @SearchTerm 
                                OR c.Email ILIKE @SearchTerm
                                OR u.Username ILIKE @SearchTerm
                                ORDER BY c.FullName";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = reader.GetInt32(0),
                                UserID = reader.GetInt32(1),
                                FullName = reader.GetString(2),
                                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                                CreatedAt = reader.GetDateTime(6),
                                Username = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return customers;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dgvCustomers.Rows.Clear();
                var customers = string.IsNullOrWhiteSpace(txtSearch.Text)
                    ? GetAllCustomers()
                    : SearchCustomers(txtSearch.Text);

                foreach (var customer in customers)
                {
                    dgvCustomers.Rows.Add(
                        customer.CustomerID,
                        customer.FullName,
                        customer.Username,
                        customer.Phone,
                        customer.Email,
                        customer.Address,
                        customer.CreatedAt.ToString("dd MMM yyyy")
                    );
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error searching customers: {ex.Message}");
            }
        }

        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = dgvCustomers.SelectedRows.Count > 0;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0) return;

            var row = dgvCustomers.SelectedRows[0];
            int customerId = Convert.ToInt32(row.Cells["CustomerID"].Value);
            string customerName = row.Cells["FullName"].Value.ToString();

            if (UIHelper.ShowConfirm($"Are you sure you want to delete customer '{customerName}'?\n\nThis will also delete their user account and cannot be undone."))
            {
                try
                {
                    if (DeleteCustomer(customerId))
                    {
                        UIHelper.ShowSuccess("Customer deleted successfully!");
                        LoadCustomers();
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        UIHelper.ShowError("Failed to delete customer.");
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowError($"Error deleting customer: {ex.Message}");
                }
            }
        }

        private bool DeleteCustomer(int customerId)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string getUserQuery = "SELECT UserID FROM Customers WHERE CustomerID = @CustomerID";
                        int userId;
                        using (var cmd = new NpgsqlCommand(getUserQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", customerId);
                            userId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string deleteCustomerQuery = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                        using (var cmd = new NpgsqlCommand(deleteCustomerQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", customerId);
                            cmd.ExecuteNonQuery();
                        }

                        string deleteUserQuery = "DELETE FROM Users WHERE UserID = @UserID";
                        using (var cmd = new NpgsqlCommand(deleteUserQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
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

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadCustomers();
            btnDelete.Enabled = false;
        }
    }
}
