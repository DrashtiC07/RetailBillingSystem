using Guna.UI2.WinForms;
using RetailBillingSystem.Models;
using RetailBillingSystem.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Forms
{
    public partial class MainDashboard : Form
    {
        // Panel containers
        private Guna2Panel sidebarPanel;
        private Guna2Panel topHeaderPanel;
        private Panel mainContentPanel;

        // Sidebar buttons
        private Guna2Button btnDashboard;
        private Guna2Button btnProducts;
        private Guna2Button btnCustomers;
        private Guna2Button btnBilling;
        private Guna2Button btnSalesHistory;
        private Guna2Button btnLogout;

        // Header controls
        private Label lblWelcome;
        private Label lblRole;
        private Guna2CircleButton btnProfile;

        // Currently active button
        private Guna2Button activeButton;

        public MainDashboard()
        {
            InitializeComponent();
            InitializeDashboard();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainDashboard
            // 
            this.ClientSize = new Size(1280, 800);
            this.Name = "MainDashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Retail Billing System - Dashboard";
            this.BackColor = UIHelper.MainBackground;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1000, 700);
            this.ResumeLayout(false);
        }

        private void InitializeDashboard()
        {
            // IMPORTANT: In Windows Forms with Docking, controls must be added in reverse order
            // The Fill-docked control must be added FIRST, then the edge-docked panels
            // This ensures proper layout without overlapping
            
            // Step 1: Create all panels first
            CreateMainContentPanel();  // Fill - must be added to Controls first
            CreateTopHeaderPanel();    // Top - added second
            CreateSidebarPanel();      // Left - added last

            // Step 2: Add controls in correct order for docking
            // Clear and re-add in proper order: Fill first, then edges
            this.Controls.Clear();
            this.Controls.Add(mainContentPanel);  // Fill docked - added first
            this.Controls.Add(topHeaderPanel);    // Top docked
            this.Controls.Add(sidebarPanel);      // Left docked - added last so it appears on left

            // Load dashboard form by default
            LoadForm(new DashboardForm());
            SetActiveButton(btnDashboard);
        }

        private void CreateSidebarPanel()
        {
            sidebarPanel = new Guna2Panel();
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Width = 220;
            sidebarPanel.FillColor = UIHelper.SidebarBackground;
            sidebarPanel.Padding = new Padding(0);
            sidebarPanel.Margin = new Padding(0);
            this.Controls.Add(sidebarPanel);

            // Create sidebar content
            CreateSidebarContent();
        }

        private void CreateSidebarContent()
        {
            // Logo area at top
            var logoPanel = new Guna2Panel();
            logoPanel.Dock = DockStyle.Top;
            logoPanel.Height = 80;
            logoPanel.FillColor = Color.Transparent;
            sidebarPanel.Controls.Add(logoPanel);

            var lblLogo = new Label();
            lblLogo.Text = "RBS";
            lblLogo.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(20, 15);
            logoPanel.Controls.Add(lblLogo);

            var lblLogoText = new Label();
            lblLogoText.Text = "Retail Billing";
            lblLogoText.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblLogoText.ForeColor = Color.FromArgb(180, 255, 255, 255);
            lblLogoText.AutoSize = true;
            lblLogoText.Location = new Point(20, 48);
            logoPanel.Controls.Add(lblLogoText);

            // Menu container panel with proper spacing
            var menuPanel = new Guna2Panel();
            menuPanel.Dock = DockStyle.Fill;
            menuPanel.FillColor = Color.Transparent;
            menuPanel.Padding = new Padding(10, 10, 10, 10);
            sidebarPanel.Controls.Add(menuPanel);

            // Create menu buttons
            btnDashboard = CreateMenuButton("Dashboard");
            btnDashboard.Click += (s, e) => { LoadForm(new DashboardForm()); SetActiveButton(btnDashboard); };

            btnBilling = CreateMenuButton("Billing");
            btnBilling.Click += (s, e) => { LoadForm(new BillingForm()); SetActiveButton(btnBilling); };

            btnSalesHistory = CreateMenuButton("Sales History");
            btnSalesHistory.Click += (s, e) => { LoadForm(new SalesHistoryForm()); SetActiveButton(btnSalesHistory); };

            btnLogout = CreateMenuButton("Logout");
            btnLogout.Click += BtnLogout_Click;

            // Add buttons based on role - use FlowLayoutPanel for proper ordering
            var buttonContainer = new FlowLayoutPanel();
            buttonContainer.Dock = DockStyle.Fill;
            buttonContainer.FlowDirection = FlowDirection.TopDown;
            buttonContainer.WrapContents = false;
            buttonContainer.AutoScroll = false;
            buttonContainer.Padding = new Padding(0);
            menuPanel.Controls.Add(buttonContainer);

            if (CurrentUser.IsAdmin)
            {
                btnProducts = CreateMenuButton("Products");
                btnProducts.Click += (s, e) => { LoadForm(new ProductForm()); SetActiveButton(btnProducts); };

                btnCustomers = CreateMenuButton("Customers");
                btnCustomers.Click += (s, e) => { LoadForm(new CustomerForm()); SetActiveButton(btnCustomers); };

                // Admin menu order
                buttonContainer.Controls.Add(btnDashboard);
                buttonContainer.Controls.Add(btnProducts);
                buttonContainer.Controls.Add(btnCustomers);
                buttonContainer.Controls.Add(btnBilling);
                buttonContainer.Controls.Add(btnSalesHistory);
                buttonContainer.Controls.Add(btnLogout);
            }
            else
            {
                // Customer menu order
                buttonContainer.Controls.Add(btnDashboard);
                buttonContainer.Controls.Add(btnBilling);
                buttonContainer.Controls.Add(btnSalesHistory);
                buttonContainer.Controls.Add(btnLogout);
            }
        }

        private Guna2Button CreateMenuButton(string text)
        {
            var button = new Guna2Button();
            button.Text = text;
            button.ImageAlign = HorizontalAlignment.Left;
            button.TextAlign = HorizontalAlignment.Left;
            button.ImageOffset = new Point(10, 0);
            button.TextOffset = new Point(15, 0);
            button.FillColor = UIHelper.SidebarBackground;
            button.HoverState.FillColor = UIHelper.SidebarHover;
            button.ForeColor = Color.White;
            button.HoverState.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            button.BorderRadius = 8;
            button.Height = 48;
            button.Width = 190;
            button.Margin = new Padding(0, 4, 0, 4);
            button.Tag = text;
            return button;
        }

        private void CreateTopHeaderPanel()
        {
            topHeaderPanel = new Guna2Panel();
            topHeaderPanel.Dock = DockStyle.Top;
            topHeaderPanel.Height = 60;
            topHeaderPanel.FillColor = Color.White;
            this.Controls.Add(topHeaderPanel);

            // Welcome message on left
            lblWelcome = new Label();
            lblWelcome.Text = $"Welcome, {CurrentUser.Username}!";
            lblWelcome.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblWelcome.ForeColor = UIHelper.TextPrimary;
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(20, 18);
            topHeaderPanel.Controls.Add(lblWelcome);

            // User info on right
            var rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 200;
            rightPanel.BackColor = Color.Transparent;
            topHeaderPanel.Controls.Add(rightPanel);

            // Role label
            lblRole = new Label();
            lblRole.Text = CurrentUser.Role;
            lblRole.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblRole.ForeColor = UIHelper.TextSecondary;
            lblRole.AutoSize = true;
            lblRole.Location = new Point(80, 20);
            rightPanel.Controls.Add(lblRole);

            // Profile button
            btnProfile = new Guna2CircleButton();
            btnProfile.Text = CurrentUser.Username.Substring(0, 1).ToUpper();
            btnProfile.FillColor = UIHelper.PrimaryButton;
            btnProfile.ForeColor = Color.White;
            btnProfile.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnProfile.Size = new Size(38, 38);
            btnProfile.Location = new Point(140, 11);
            rightPanel.Controls.Add(btnProfile);
        }

        private void CreateMainContentPanel()
        {
            // Use a Panel (not Guna2Panel) for main content to avoid rendering issues
            mainContentPanel = new Panel();
            mainContentPanel.Dock = DockStyle.Fill;
            mainContentPanel.BackColor = UIHelper.MainBackground;
            mainContentPanel.Padding = new Padding(25);
            mainContentPanel.Margin = new Padding(0);
            mainContentPanel.AutoScroll = true;
            this.Controls.Add(mainContentPanel);
        }

        /// <summary>
        /// Loads a form into the main content panel
        /// </summary>
        public void LoadForm(Form form)
        {
            // Clear existing controls
            foreach (Control ctrl in mainContentPanel.Controls)
            {
                ctrl.Dispose();
            }
            mainContentPanel.Controls.Clear();

            // Configure the form to be hosted
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.BackColor = UIHelper.MainBackground;

            // Add to panel
            mainContentPanel.Controls.Add(form);
            mainContentPanel.Tag = form;

            // Show the form
            form.Show();
        }

        /// <summary>
        /// Sets the active sidebar button
        /// </summary>
        private void SetActiveButton(Guna2Button button)
        {
            // Reset previous active button
            if (activeButton != null)
            {
                UIHelper.SetSidebarButtonInactive(activeButton);
            }

            // Set new active button
            activeButton = button;
            UIHelper.SetSidebarButtonActive(activeButton);
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (UIHelper.ShowConfirm("Are you sure you want to logout?"))
            {
                // Clear current user session
                CurrentUser.Clear();

                // Close main dashboard and show login form
                this.Hide();
                var loginForm = new LoginForm();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }
    }
}
