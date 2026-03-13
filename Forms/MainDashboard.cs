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

            // Load default form based on role
            if (CurrentUser.IsAdmin)
            {
                LoadForm(new ProductForm());
                SetActiveButton(btnProducts);
            }
            else
            {
                LoadForm(new BillingForm());
                SetActiveButton(btnBilling);
            }
        }

        private void CreateSidebarPanel()
        {
            sidebarPanel = new Guna2Panel();
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Width = 240;
            sidebarPanel.FillColor = UIHelper.SidebarBackground;
            sidebarPanel.Padding = new Padding(0);
            sidebarPanel.Margin = new Padding(0);
            sidebarPanel.BorderColor = UIHelper.SidebarBorder;
            sidebarPanel.BorderThickness = 1;
            this.Controls.Add(sidebarPanel);

            // Create sidebar content
            CreateSidebarContent();
        }

        private void CreateSidebarContent()
        {
            // Logo area at top - Clean modern style
            var logoPanel = new Guna2Panel();
            logoPanel.Dock = DockStyle.Top;
            logoPanel.Height = 70;
            logoPanel.FillColor = Color.Transparent;
            sidebarPanel.Controls.Add(logoPanel);

            // Logo icon circle
            var logoCircle = new Guna2CircleButton();
            logoCircle.FillColor = UIHelper.AccentColor;
            logoCircle.ForeColor = Color.White;
            logoCircle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            logoCircle.Text = "M";
            logoCircle.Size = new Size(36, 36);
            logoCircle.Location = new Point(20, 17);
            logoCircle.Enabled = false;
            logoPanel.Controls.Add(logoCircle);

            var lblLogo = new Label();
            lblLogo.Text = "Maple";
            lblLogo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblLogo.ForeColor = UIHelper.TextPrimary;
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(62, 22);
            logoPanel.Controls.Add(lblLogo);

            // Section label for main navigation
            var lblMenuSection = new Label();
            lblMenuSection.Text = "MENU";
            lblMenuSection.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblMenuSection.ForeColor = UIHelper.TextMuted;
            lblMenuSection.AutoSize = true;
            lblMenuSection.Location = new Point(20, 0);
            
            var sectionPanel = new Panel();
            sectionPanel.Dock = DockStyle.Top;
            sectionPanel.Height = 30;
            sectionPanel.BackColor = Color.Transparent;
            sectionPanel.Controls.Add(lblMenuSection);
            sidebarPanel.Controls.Add(sectionPanel);

            // Menu container panel with proper spacing
            var menuPanel = new Guna2Panel();
            menuPanel.Dock = DockStyle.Fill;
            menuPanel.FillColor = Color.Transparent;
            menuPanel.Padding = new Padding(12, 0, 12, 12);
            sidebarPanel.Controls.Add(menuPanel);

            // Create menu buttons - add buttons based on role
            var buttonContainer = new FlowLayoutPanel();
            buttonContainer.Dock = DockStyle.Fill;
            buttonContainer.FlowDirection = FlowDirection.TopDown;
            buttonContainer.WrapContents = false;
            buttonContainer.AutoScroll = false;
            buttonContainer.Padding = new Padding(0);
            buttonContainer.BackColor = Color.Transparent;
            menuPanel.Controls.Add(buttonContainer);

            if (CurrentUser.IsAdmin)
            {
                // Admin sees: Products, Customers, Billing, Sales History, Logout
                btnProducts = CreateMenuButton("Products");
                btnProducts.Click += (s, e) => { LoadForm(new ProductForm()); SetActiveButton(btnProducts); };
                buttonContainer.Controls.Add(btnProducts);

                btnCustomers = CreateMenuButton("Customers");
                btnCustomers.Click += (s, e) => { LoadForm(new CustomerForm()); SetActiveButton(btnCustomers); };
                buttonContainer.Controls.Add(btnCustomers);

                btnBilling = CreateMenuButton("Billing");
                btnBilling.Click += (s, e) => { LoadForm(new BillingForm()); SetActiveButton(btnBilling); };
                buttonContainer.Controls.Add(btnBilling);

                btnSalesHistory = CreateMenuButton("Sales History");
                btnSalesHistory.Click += (s, e) => { LoadForm(new SalesHistoryForm()); SetActiveButton(btnSalesHistory); };
                buttonContainer.Controls.Add(btnSalesHistory);

                // Separator
                var separator = new Panel();
                separator.Height = 1;
                separator.Width = 200;
                separator.BackColor = UIHelper.BorderColor;
                separator.Margin = new Padding(0, 12, 0, 12);
                buttonContainer.Controls.Add(separator);

                btnLogout = CreateMenuButton("Logout");
                btnLogout.ForeColor = UIHelper.DangerButton;
                btnLogout.Click += BtnLogout_Click;
                buttonContainer.Controls.Add(btnLogout);
            }
            else
            {
                // Customer sees: Billing, Sales History, Logout
                btnBilling = CreateMenuButton("Billing");
                btnBilling.Click += (s, e) => { LoadForm(new BillingForm()); SetActiveButton(btnBilling); };
                buttonContainer.Controls.Add(btnBilling);

                btnSalesHistory = CreateMenuButton("Sales History");
                btnSalesHistory.Click += (s, e) => { LoadForm(new SalesHistoryForm()); SetActiveButton(btnSalesHistory); };
                buttonContainer.Controls.Add(btnSalesHistory);

                // Separator
                var separator = new Panel();
                separator.Height = 1;
                separator.Width = 200;
                separator.BackColor = UIHelper.BorderColor;
                separator.Margin = new Padding(0, 12, 0, 12);
                buttonContainer.Controls.Add(separator);

                btnLogout = CreateMenuButton("Logout");
                btnLogout.ForeColor = UIHelper.DangerButton;
                btnLogout.Click += BtnLogout_Click;
                buttonContainer.Controls.Add(btnLogout);
            }
        }

        private Guna2Button CreateMenuButton(string text)
        {
            var button = new Guna2Button();
            button.Text = text;
            button.ImageAlign = HorizontalAlignment.Left;
            button.TextAlign = HorizontalAlignment.Left;
            button.ImageOffset = new Point(8, 0);
            button.TextOffset = new Point(12, 0);
            button.FillColor = Color.Transparent;
            button.HoverState.FillColor = UIHelper.SidebarHover;
            button.ForeColor = UIHelper.SidebarText;
            button.HoverState.ForeColor = UIHelper.TextPrimary;
            button.Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular);
            button.BorderRadius = 8;
            button.Height = 44;
            button.Width = 210;
            button.Margin = new Padding(0, 2, 0, 2);
            button.Tag = text;
            button.Cursor = Cursors.Hand;
            return button;
        }

        private void CreateTopHeaderPanel()
        {
            topHeaderPanel = new Guna2Panel();
            topHeaderPanel.Dock = DockStyle.Top;
            topHeaderPanel.Height = 64;
            topHeaderPanel.FillColor = UIHelper.HeaderBackground;
            topHeaderPanel.BorderColor = UIHelper.BorderColor;
            topHeaderPanel.BorderThickness = 1;
            this.Controls.Add(topHeaderPanel);

            // Search bar on left (optional - placeholder for future search)
            var searchPanel = new Guna2Panel();
            searchPanel.Dock = DockStyle.Left;
            searchPanel.Width = 350;
            searchPanel.FillColor = Color.Transparent;
            searchPanel.Padding = new Padding(20, 12, 20, 12);
            topHeaderPanel.Controls.Add(searchPanel);

            // Page title
            lblWelcome = new Label();
            lblWelcome.Text = $"Welcome back, {CurrentUser.Username}";
            lblWelcome.Font = new Font("Segoe UI Semibold", 11, FontStyle.Regular);
            lblWelcome.ForeColor = UIHelper.TextPrimary;
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(20, 20);
            searchPanel.Controls.Add(lblWelcome);

            // User info on right
            var rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 220;
            rightPanel.BackColor = Color.Transparent;
            topHeaderPanel.Controls.Add(rightPanel);

            // Role badge
            var roleBadge = new Guna2Panel();
            roleBadge.FillColor = CurrentUser.IsAdmin ? UIHelper.BadgeBlue : UIHelper.BadgeGreen;
            roleBadge.BorderRadius = 12;
            roleBadge.Size = new Size(70, 24);
            roleBadge.Location = new Point(60, 20);
            rightPanel.Controls.Add(roleBadge);

            lblRole = new Label();
            lblRole.Text = CurrentUser.Role;
            lblRole.Font = new Font("Segoe UI Semibold", 8, FontStyle.Regular);
            lblRole.ForeColor = CurrentUser.IsAdmin ? UIHelper.BadgeBlueText : UIHelper.BadgeGreenText;
            lblRole.AutoSize = true;
            lblRole.Location = new Point(CurrentUser.IsAdmin ? 12 : 6, 4);
            roleBadge.Controls.Add(lblRole);

            // Profile button
            btnProfile = new Guna2CircleButton();
            btnProfile.Text = CurrentUser.Username.Substring(0, 1).ToUpper();
            btnProfile.FillColor = UIHelper.AccentColor;
            btnProfile.ForeColor = Color.White;
            btnProfile.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnProfile.Size = new Size(36, 36);
            btnProfile.Location = new Point(150, 14);
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
