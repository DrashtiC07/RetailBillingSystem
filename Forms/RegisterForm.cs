using Guna.UI2.WinForms;
using Npgsql;
using RetailBillingSystem.Database;
using RetailBillingSystem.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Forms
{
    public partial class RegisterForm : Form
    {
        // Guna2 Controls
        private Guna2ShadowPanel cardPanel;
        private Guna2TextBox txtUsername;
        private Guna2TextBox txtPassword;
        private Guna2TextBox txtConfirmPassword;
        private Guna2TextBox txtFullName;
        private Guna2TextBox txtPhone;
        private Guna2TextBox txtEmail;
        private Guna2Button btnRegister;
        private Guna2Button btnBack;
        private Guna2GradientPanel gradientPanel;
        private Label lblTitle;
        private Label lblSubtitle;

        public RegisterForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RegisterForm
            // 
            this.ClientSize = new Size(1000, 700);
            this.Name = "RegisterForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Retail Billing System - Register";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = UIHelper.MainBackground;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ResumeLayout(false);
        }

        private void InitializeCustomComponents()
        {
            // Create green accent panel (left side) - matching login
            gradientPanel = new Guna2GradientPanel();
            gradientPanel.Dock = DockStyle.Left;
            gradientPanel.Width = 400;
            gradientPanel.FillColor = UIHelper.AccentColor;
            gradientPanel.FillColor2 = UIHelper.AccentColor;
            gradientPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.Controls.Add(gradientPanel);

            // Add content to gradient panel
            CreateGradientPanelContent();

            // Create registration card panel
            CreateRegisterCard();
        }

        private void CreateGradientPanelContent()
        {
            // Logo icon
            var logoCircle = new Guna2CircleButton();
            logoCircle.FillColor = Color.White;
            logoCircle.ForeColor = UIHelper.AccentColor;
            logoCircle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            logoCircle.Text = "M";
            logoCircle.Size = new Size(60, 60);
            logoCircle.Location = new Point(50, 60);
            logoCircle.Enabled = false;
            gradientPanel.Controls.Add(logoCircle);

            // Logo text
            var lblLogo = new Label();
            lblLogo.Text = "Maple";
            lblLogo.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(120, 72);
            gradientPanel.Controls.Add(lblLogo);

            // Welcome label
            var lblWelcome = new Label();
            lblWelcome.Text = "Create Account";
            lblWelcome.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(50, 180);
            gradientPanel.Controls.Add(lblWelcome);

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Join our retail billing system\nManage your purchases easily";
            lblSubtitle.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(220, 255, 255, 255);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(50, 235);
            gradientPanel.Controls.Add(lblSubtitle);
        }

        private void CreateRegisterCard()
        {
            // Card panel - Clean minimal shadow
            cardPanel = new Guna2ShadowPanel();
            cardPanel.FillColor = Color.White;
            cardPanel.ShadowColor = Color.FromArgb(40, 0, 0, 0);
            cardPanel.ShadowDepth = 20;
            cardPanel.ShadowShift = 4;
            cardPanel.Radius = 16;
            cardPanel.Padding = new Padding(30);
            cardPanel.Size = new Size(500, 540);
            cardPanel.Location = new Point(460, 80);
            this.Controls.Add(cardPanel);

            // Use TableLayoutPanel for proper layout
            var tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.RowCount = 9;
            tableLayout.ColumnCount = 2;
            tableLayout.Padding = new Padding(25);

            // Column styles - labels and inputs
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            // Row styles
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));  // Title
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Username
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Password
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Confirm Password
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Full Name
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Phone
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Email
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));  // Register button
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));  // Back button

            cardPanel.Controls.Add(tableLayout);

            // Title
            lblTitle = new Label();
            lblTitle.Text = "Register";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.TextPrimary;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            tableLayout.SetColumnSpan(lblTitle, 2);
            tableLayout.Controls.Add(lblTitle, 0, 0);

            // Username
            AddFormRow(tableLayout, "Username:", CreateTextBox("Enter username", out txtUsername), 1);

            // Password
            txtPassword = new Guna2TextBox();
            txtPassword.UseSystemPasswordChar = true;
            AddFormRow(tableLayout, "Password:", CreateTextBox("Enter password", out txtPassword, true), 2);

            // Confirm Password
            txtConfirmPassword = new Guna2TextBox();
            txtConfirmPassword.UseSystemPasswordChar = true;
            AddFormRow(tableLayout, "Confirm Password:", CreateTextBox("Confirm password", out txtConfirmPassword, true), 3);

            // Full Name
            AddFormRow(tableLayout, "Full Name:", CreateTextBox("Enter full name", out txtFullName), 4);

            // Phone
            AddFormRow(tableLayout, "Phone:", CreateTextBox("Enter phone number", out txtPhone), 5);

            // Email
            AddFormRow(tableLayout, "Email:", CreateTextBox("Enter email address", out txtEmail), 6);

            // Register button
            btnRegister = new Guna2Button();
            UIHelper.StyleSuccessButton(btnRegister, "Create Account");
            btnRegister.Dock = DockStyle.Fill;
            btnRegister.Margin = new Padding(5, 10, 5, 5);
            btnRegister.Click += BtnRegister_Click;
            tableLayout.SetColumnSpan(btnRegister, 2);
            tableLayout.Controls.Add(btnRegister, 0, 7);

            // Back to login button
            btnBack = new Guna2Button();
            btnBack.Text = "Already have an account? Sign In";
            btnBack.FillColor = Color.Transparent;
            btnBack.HoverState.FillColor = Color.Transparent;
            btnBack.ForeColor = UIHelper.PrimaryButton;
            btnBack.HoverState.ForeColor = UIHelper.SidebarActive;
            btnBack.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnBack.BorderRadius = 8;
            btnBack.Dock = DockStyle.Fill;
            btnBack.Margin = new Padding(5, 5, 5, 0);
            btnBack.Click += BtnBack_Click;
            tableLayout.SetColumnSpan(btnBack, 2);
            tableLayout.Controls.Add(btnBack, 0, 8);
        }

        private Guna2TextBox CreateTextBox(string placeholder, out Guna2TextBox textBox, bool isPassword = false)
        {
            textBox = new Guna2TextBox();
            UIHelper.StyleTextBox(textBox, placeholder);
            textBox.Dock = DockStyle.Fill;
            textBox.Margin = new Padding(5, 8, 5, 8);
            if (isPassword)
            {
                textBox.UseSystemPasswordChar = true;
            }
            return textBox;
        }

        private void AddFormRow(TableLayoutPanel table, string labelText, Guna2TextBox textBox, int row)
        {
            // Label
            var label = new Label();
            label.Text = labelText;
            label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label.ForeColor = UIHelper.TextPrimary;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            table.Controls.Add(label, 0, row);

            // TextBox
            table.Controls.Add(textBox, 1, row);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // Validate all fields
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                UIHelper.ShowError("Please enter username.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                UIHelper.ShowError("Please enter password.");
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text.Length < 6)
            {
                UIHelper.ShowError("Password must be at least 6 characters long.");
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                UIHelper.ShowError("Passwords do not match.");
                txtConfirmPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                UIHelper.ShowError("Please enter your full name.");
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                UIHelper.ShowError("Please enter your phone number.");
                txtPhone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                UIHelper.ShowError("Please enter your email address.");
                txtEmail.Focus();
                return;
            }

            // Validate email format
            if (!IsValidEmail(txtEmail.Text))
            {
                UIHelper.ShowError("Please enter a valid email address.");
                txtEmail.Focus();
                return;
            }

            try
            {
                // Check if username already exists
                if (UsernameExists(txtUsername.Text.Trim()))
                {
                    UIHelper.ShowError("Username already exists. Please choose a different username.");
                    txtUsername.Focus();
                    return;
                }

                // Register the user
                if (RegisterUser())
                {
                    UIHelper.ShowSuccess("Registration successful! Please sign in.");
                    this.Close();
                }
                else
                {
                    UIHelper.ShowError("Registration failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Registration error: {ex.Message}");
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if username already exists
        /// </summary>
        private bool UsernameExists(string username)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        /// <summary>
        /// Registers a new user with customer role
        /// </summary>
        private bool RegisterUser()
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert user
                        string userQuery = @"INSERT INTO Users (Username, Password, Role) 
                                            VALUES (@Username, @Password, 'Customer') 
                                            RETURNING UserID";

                        int userId;
                        using (var cmd = new NpgsqlCommand(userQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text); // In production, hash the password!
                            userId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Insert customer details
                        string customerQuery = @"INSERT INTO Customers (UserID, FullName, Phone, Email, Address) 
                                                VALUES (@UserID, @FullName, @Phone, @Email, @Address)";

                        using (var cmd = new NpgsqlCommand(customerQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@Address", "Not provided");
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
    }
}
