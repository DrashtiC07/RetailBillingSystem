using Guna.UI2.WinForms;
using Npgsql;
using RetailBillingSystem.Database;
using RetailBillingSystem.Models;
using RetailBillingSystem.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Forms
{
    public partial class LoginForm : Form
    {
        // Guna2 Controls
        private Guna2ShadowPanel cardPanel;
        private Guna2TextBox txtUsername;
        private Guna2TextBox txtPassword;
        private Guna2Button btnLogin;
        private Guna2Button btnRegister;
        private Panel leftPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblWelcome;

        public LoginForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LoginForm
            // 
            this.ClientSize = new Size(900, 600);
            this.Name = "LoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Retail Billing System - Login";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ResumeLayout(false);
        }

        private void InitializeCustomComponents()
        {
            // Create left panel with solid color (not gradient for better text visibility)
            leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 380;
            leftPanel.BackColor = UIHelper.SidebarBackground;
            this.Controls.Add(leftPanel);

            // Add welcome content to left panel
            CreateLeftPanelContent();

            // Create login card panel (center-right)
            CreateLoginCard();
        }

        private void CreateLeftPanelContent()
        {
            // Logo container
            var logoPanel = new Panel();
            logoPanel.Size = new Size(80, 80);
            logoPanel.Location = new Point(40, 60);
            logoPanel.BackColor = Color.FromArgb(60, 255, 255, 255);
            logoPanel.Paint += (s, e) => {
                using (var brush = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(brush, 0, 0, 79, 79);
                }
            };
            leftPanel.Controls.Add(logoPanel);

            // Logo text
            var lblLogo = new Label();
            lblLogo.Text = "RBS";
            lblLogo.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(18, 24);
            logoPanel.Controls.Add(lblLogo);

            // Welcome label
            lblWelcome = new Label();
            lblWelcome.Text = "Welcome Back!";
            lblWelcome.Font = new Font("Segoe UI", 32, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(40, 180);
            leftPanel.Controls.Add(lblWelcome);

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Retail Billing System";
            lblSubtitle.Font = new Font("Segoe UI", 14, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(200, 255, 255, 255);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(40, 240);
            leftPanel.Controls.Add(lblSubtitle);

            // Additional subtitle line
            var lblSubtitle2 = new Label();
            lblSubtitle2.Text = "Manage your business with ease";
            lblSubtitle2.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblSubtitle2.ForeColor = Color.FromArgb(160, 255, 255, 255);
            lblSubtitle2.AutoSize = true;
            lblSubtitle2.Location = new Point(40, 270);
            leftPanel.Controls.Add(lblSubtitle2);

            // Decorative elements at bottom
            var decorPanel = new Panel();
            decorPanel.Size = new Size(200, 4);
            decorPanel.Location = new Point(40, 350);
            decorPanel.BackColor = Color.FromArgb(100, 255, 255, 255);
            leftPanel.Controls.Add(decorPanel);
        }

        private void CreateLoginCard()
        {
            // Card panel using Guna2ShadowPanel
            cardPanel = new Guna2ShadowPanel();
            cardPanel.FillColor = Color.White;
            cardPanel.ShadowColor = Color.Black;
            cardPanel.ShadowDepth = 30;
            cardPanel.ShadowShift = 6;
            cardPanel.Radius = 16;
            cardPanel.Padding = new Padding(35);
            cardPanel.Size = new Size(400, 480);
            cardPanel.Location = new Point(440, 60);
            this.Controls.Add(cardPanel);

            // Use TableLayoutPanel for proper layout
            var tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.RowCount = 6;
            tableLayout.ColumnCount = 1;

            // Row styles
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Title
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 75));  // Username
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 75));  // Password
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));  // Login button
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));  // Register button
            tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // Spacer

            cardPanel.Controls.Add(tableLayout);

            // Title
            lblTitle = new Label();
            lblTitle.Text = "Sign In";
            lblTitle.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.TextPrimary;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            tableLayout.Controls.Add(lblTitle, 0, 0);

            // Username textbox container
            var usernamePanel = new Panel();
            usernamePanel.Dock = DockStyle.Fill;
            usernamePanel.Padding = new Padding(0, 15, 0, 5);
            tableLayout.Controls.Add(usernamePanel, 0, 1);

            // Username label
            var lblUsername = new Label();
            lblUsername.Text = "Username";
            lblUsername.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblUsername.ForeColor = UIHelper.TextSecondary;
            lblUsername.Dock = DockStyle.Top;
            lblUsername.Height = 20;
            usernamePanel.Controls.Add(lblUsername);

            // Username textbox
            txtUsername = new Guna2TextBox();
            txtUsername.PlaceholderText = "Enter your username";
            txtUsername.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            txtUsername.BorderRadius = 10;
            txtUsername.BorderColor = UIHelper.BorderColor;
            txtUsername.ForeColor = UIHelper.TextPrimary;
            txtUsername.Height = 45;
            txtUsername.Dock = DockStyle.Bottom;
            usernamePanel.Controls.Add(txtUsername);

            // Password container
            var passwordPanel = new Panel();
            passwordPanel.Dock = DockStyle.Fill;
            passwordPanel.Padding = new Padding(0, 10, 0, 5);
            tableLayout.Controls.Add(passwordPanel, 0, 2);

            // Password label
            var lblPassword = new Label();
            lblPassword.Text = "Password";
            lblPassword.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblPassword.ForeColor = UIHelper.TextSecondary;
            lblPassword.Dock = DockStyle.Top;
            lblPassword.Height = 20;
            passwordPanel.Controls.Add(lblPassword);

            // Password textbox
            txtPassword = new Guna2TextBox();
            txtPassword.PlaceholderText = "Enter your password";
            txtPassword.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            txtPassword.BorderRadius = 10;
            txtPassword.BorderColor = UIHelper.BorderColor;
            txtPassword.ForeColor = UIHelper.TextPrimary;
            txtPassword.Height = 45;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Dock = DockStyle.Bottom;
            passwordPanel.Controls.Add(txtPassword);

            // Login button
            btnLogin = new Guna2Button();
            btnLogin.Text = "Sign In";
            btnLogin.FillColor = UIHelper.PrimaryButton;
            btnLogin.HoverState.FillColor = Color.FromArgb(37, 99, 235);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogin.BorderRadius = 10;
            btnLogin.Height = 50;
            btnLogin.Dock = DockStyle.Fill;
            btnLogin.Margin = new Padding(0, 10, 0, 5);
            btnLogin.Click += BtnLogin_Click;
            tableLayout.Controls.Add(btnLogin, 0, 3);

            // Register button
            btnRegister = new Guna2Button();
            btnRegister.Text = "Don't have an account? Register";
            btnRegister.FillColor = Color.Transparent;
            btnRegister.HoverState.FillColor = Color.Transparent;
            btnRegister.ForeColor = UIHelper.PrimaryButton;
            btnRegister.HoverState.ForeColor = Color.FromArgb(37, 99, 235);
            btnRegister.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btnRegister.BorderRadius = 8;
            btnRegister.Height = 40;
            btnRegister.Dock = DockStyle.Fill;
            btnRegister.Margin = new Padding(0, 5, 0, 0);
            btnRegister.Click += BtnRegister_Click;
            tableLayout.Controls.Add(btnRegister, 0, 4);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validate inputs
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

            try
            {
                // Authenticate user
                var user = AuthenticateUser(txtUsername.Text.Trim(), txtPassword.Text);

                if (user != null)
                {
                    // Set current user session
                    CurrentUser.UserID = user.UserID;
                    CurrentUser.Username = user.Username;
                    CurrentUser.Role = user.Role;

                    // If customer, get customer ID
                    if (user.Role == "Customer")
                    {
                        CurrentUser.CustomerID = GetCustomerId(user.UserID);
                    }

                    // Update last login
                    UpdateLastLogin(user.UserID);

                    UIHelper.ShowSuccess($"Welcome, {user.Username}!");

                    // Open main dashboard
                    this.Hide();
                    var mainDashboard = new MainDashboard();
                    mainDashboard.FormClosed += (s, args) => this.Close();
                    mainDashboard.Show();
                }
                else
                {
                    UIHelper.ShowError("Invalid username or password.");
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Login error: {ex.Message}");
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registerForm = new RegisterForm();
            registerForm.FormClosed += (s, args) => this.Show();
            registerForm.Show();
        }

        /// <summary>
        /// Authenticates user against database
        /// </summary>
        private User AuthenticateUser(string username, string password)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT UserID, Username, Password, Role, CreatedAt, LastLogin FROM Users WHERE Username = @Username AND Password = @Password";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                Role = reader.GetString(3),
                                CreatedAt = reader.GetDateTime(4),
                                LastLogin = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets customer ID for a user
        /// </summary>
        private int? GetCustomerId(int userId)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT CustomerID FROM Customers WHERE UserID = @UserID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
        }

        /// <summary>
        /// Updates last login timestamp
        /// </summary>
        private void UpdateLastLogin(int userId)
        {
            using (var conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Users SET LastLogin = CURRENT_TIMESTAMP WHERE UserID = @UserID";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}