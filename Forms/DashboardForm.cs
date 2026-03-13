using Guna.UI2.WinForms;
using RetailBillingSystem.Models;
using RetailBillingSystem.Services;
using RetailBillingSystem.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Forms
{
    public partial class DashboardForm : Form
    {
        private ProductService _productService;
        private BillingService _billingService;

        public DashboardForm()
        {
            _productService = new ProductService();
            _billingService = new BillingService();
            InitializeComponent();
            InitializeDashboard();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DashboardForm
            // 
            this.ClientSize = new Size(950, 700);
            this.Name = "DashboardForm";
            this.Text = "Dashboard";
            this.BackColor = UIHelper.MainBackground;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ResumeLayout(false);
        }

        private void InitializeDashboard()
        {
            // Main container panel with proper padding
            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = UIHelper.MainBackground;
            mainPanel.Padding = new Padding(10, 10, 10, 10);
            mainPanel.AutoScroll = true;
            this.Controls.Add(mainPanel);

            // Title section
            var titlePanel = new Panel();
            titlePanel.Dock = DockStyle.Top;
            titlePanel.Height = 70;
            titlePanel.BackColor = Color.Transparent;
            mainPanel.Controls.Add(titlePanel);

            // Title
            var lblTitle = new Label();
            lblTitle.Text = CurrentUser.IsAdmin ? "Admin Dashboard" : "My Dashboard";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(0, 5);
            titlePanel.Controls.Add(lblTitle);

            // Subtitle
            var lblSubtitle = new Label();
            lblSubtitle.Text = CurrentUser.IsAdmin
                ? "Overview of your retail business"
                : "Overview of your purchase history";
            lblSubtitle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblSubtitle.ForeColor = UIHelper.TextSecondary;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(0, 38);
            titlePanel.Controls.Add(lblSubtitle);

            // Stats cards container - FlowLayoutPanel for proper wrapping
            var cardsPanel = new FlowLayoutPanel();
            cardsPanel.Dock = DockStyle.Top;
            cardsPanel.Height = CurrentUser.IsAdmin ? 320 : 200;
            cardsPanel.FlowDirection = FlowDirection.LeftToRight;
            cardsPanel.WrapContents = true;
            cardsPanel.BackColor = Color.Transparent;
            cardsPanel.Padding = new Padding(0, 10, 0, 10);
            cardsPanel.AutoSize = false;
            mainPanel.Controls.Add(cardsPanel);

            if (CurrentUser.IsAdmin)
            {
                CreateAdminDashboard(cardsPanel, mainPanel);
            }
            else
            {
                CreateCustomerDashboard(cardsPanel, mainPanel);
            }
        }

        private void CreateAdminDashboard(FlowLayoutPanel cardsPanel, Panel mainPanel)
        {
            try
            {
                // Get statistics
                int totalProducts = _productService.GetTotalProductCount();
                int lowStockCount = _productService.GetLowStockCount();
                var (todaySales, todayRevenue) = _billingService.GetTodaySales();
                int totalCustomers = GetTotalCustomerCount();

                // Total Products Card
                cardsPanel.Controls.Add(CreateStatCard("Total Products", totalProducts.ToString(),
                    UIHelper.PrimaryButton, "Products in inventory", "P"));

                // Total Customers Card
                cardsPanel.Controls.Add(CreateStatCard("Total Customers", totalCustomers.ToString(),
                    UIHelper.SuccessButton, "Registered customers", "C"));

                // Today's Sales Card
                cardsPanel.Controls.Add(CreateStatCard("Today's Sales", todaySales.ToString(),
                    UIHelper.WarningButton, "Transactions today", "S"));

                // Today's Revenue Card
                cardsPanel.Controls.Add(CreateStatCard("Today's Revenue", $"₹{todayRevenue:N0}",
                    Color.FromArgb(139, 92, 246), "Revenue today", "R"));

                // Low Stock Card
                cardsPanel.Controls.Add(CreateStatCard("Low Stock", lowStockCount.ToString(),
                    lowStockCount > 0 ? UIHelper.DangerButton : UIHelper.SuccessButton,
                    lowStockCount > 0 ? "Items need restocking" : "Stock levels good", "L"));

                // Add quick actions section below cards
                CreateQuickActionsSection(mainPanel);
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error loading dashboard: {ex.Message}");
            }
        }

        private void CreateCustomerDashboard(FlowLayoutPanel cardsPanel, Panel mainPanel)
        {
            try
            {
                if (CurrentUser.CustomerID.HasValue)
                {
                    int customerId = CurrentUser.CustomerID.Value;

                    // Get statistics
                    int totalOrders = _billingService.GetCustomerOrderCount(customerId);
                    int totalItems = _billingService.GetCustomerTotalItems(customerId);
                    decimal totalSpent = _billingService.GetCustomerTotalSpent(customerId);

                    // Total Orders Card
                    cardsPanel.Controls.Add(CreateStatCard("Total Orders", totalOrders.ToString(),
                        UIHelper.PrimaryButton, "Orders placed", "O"));

                    // Total Items Card
                    cardsPanel.Controls.Add(CreateStatCard("Items Purchased", totalItems.ToString(),
                        UIHelper.SuccessButton, "Products bought", "I"));

                    // Total Spent Card
                    cardsPanel.Controls.Add(CreateStatCard("Total Spent", $"₹{totalSpent:N0}",
                        UIHelper.WarningButton, "Money spent", "₹"));

                    // Recent Purchases Preview
                    CreateRecentPurchasesPreview(mainPanel);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error loading dashboard: {ex.Message}");
            }
        }

        private Guna2ShadowPanel CreateStatCard(string title, string value, Color accentColor, string subtitle, string iconText)
        {
            var card = new Guna2ShadowPanel();
            card.FillColor = Color.White;
            card.ShadowColor = Color.Black;
            card.ShadowDepth = 15;
            card.ShadowShift = 3;
            card.Radius = 12; // For stat cards
            card.Size = new Size(260, 130);
            card.Margin = new Padding(8);
            card.Padding = new Padding(15);

            // Use TableLayoutPanel for proper layout
            var tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.RowCount = 3;
            tableLayout.ColumnCount = 2;

            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            // Icon panel
            var iconPanel = new Panel();
            iconPanel.Dock = DockStyle.Fill;
            iconPanel.BackColor = Color.FromArgb(25, accentColor);
            iconPanel.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Color.FromArgb(25, accentColor)))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, 45, 45);
                }
            };
            tableLayout.Controls.Add(iconPanel, 0, 0);
            tableLayout.SetRowSpan(iconPanel, 2);

            // Icon text
            var lblIcon = new Label();
            lblIcon.Text = iconText;
            lblIcon.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblIcon.ForeColor = accentColor;
            lblIcon.Dock = DockStyle.Fill;
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            iconPanel.Controls.Add(lblIcon);

            // Value label
            var lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblValue.ForeColor = UIHelper.TextPrimary;
            lblValue.Dock = DockStyle.Fill;
            lblValue.TextAlign = ContentAlignment.BottomLeft;
            tableLayout.Controls.Add(lblValue, 1, 0);

            // Title label
            var lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblTitle.ForeColor = UIHelper.TextSecondary;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.TopLeft;
            tableLayout.Controls.Add(lblTitle, 1, 1);

            // Subtitle label
            var lblSubtitle = new Label();
            lblSubtitle.Text = subtitle;
            lblSubtitle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblSubtitle.ForeColor = UIHelper.TextSecondary;
            lblSubtitle.Dock = DockStyle.Fill;
            lblSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            tableLayout.SetColumnSpan(lblSubtitle, 2);
            tableLayout.Controls.Add(lblSubtitle, 0, 2);

            card.Controls.Add(tableLayout);
            return card;
        }

        private void CreateQuickActionsSection(Panel mainPanel)
        {
            var sectionPanel = new Panel();
            sectionPanel.Dock = DockStyle.Top;
            sectionPanel.Height = 200;
            sectionPanel.BackColor = Color.Transparent;
            sectionPanel.Padding = new Padding(0, 20, 0, 0);
            mainPanel.Controls.Add(sectionPanel);

            // Section title
            var lblSectionTitle = new Label();
            lblSectionTitle.Text = "Quick Actions";
            lblSectionTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblSectionTitle.ForeColor = UIHelper.TextPrimary;
            lblSectionTitle.AutoSize = true;
            lblSectionTitle.Location = new Point(0, 25);
            sectionPanel.Controls.Add(lblSectionTitle);

            // Buttons container
            var buttonsPanel = new FlowLayoutPanel();
            buttonsPanel.Location = new Point(0, 60);
            buttonsPanel.Size = new Size(900, 120);
            buttonsPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonsPanel.WrapContents = true;
            buttonsPanel.BackColor = Color.Transparent;
            sectionPanel.Controls.Add(buttonsPanel);

            // Quick action buttons
            var btnNewSale = CreateQuickActionButton("New Sale", UIHelper.PrimaryButton, "Create a new bill");
            btnNewSale.Click += (s, e) => {
                if (this.Parent is Panel mainContentPanel && mainContentPanel.Parent is MainDashboard dashboard)
                {
                    dashboard.LoadForm(new BillingForm());
                }
            };
            buttonsPanel.Controls.Add(btnNewSale);

            var btnAddProduct = CreateQuickActionButton("Add Product", UIHelper.SuccessButton, "Add new product");
            btnAddProduct.Click += (s, e) => {
                if (this.Parent is Panel mainContentPanel && mainContentPanel.Parent is MainDashboard dashboard)
                {
                    dashboard.LoadForm(new ProductForm());
                }
            };
            buttonsPanel.Controls.Add(btnAddProduct);

            var btnViewSales = CreateQuickActionButton("View Sales", UIHelper.WarningButton, "View sales history");
            btnViewSales.Click += (s, e) => {
                if (this.Parent is Panel mainContentPanel && mainContentPanel.Parent is MainDashboard dashboard)
                {
                    dashboard.LoadForm(new SalesHistoryForm());
                }
            };
            buttonsPanel.Controls.Add(btnViewSales);
        }

        private Guna2ShadowPanel CreateQuickActionButton(string title, Color color, string description)
        {
            var card = new Guna2ShadowPanel();
            card.FillColor = Color.White;
            card.ShadowColor = Color.Black;
            card.ShadowDepth = 10;
            card.ShadowShift = 2;
            card.Radius = 10; // Use Radius instead of BorderRadius
            card.Size = new Size(180, 90);
            card.Margin = new Padding(8);
            card.Padding = new Padding(12);
            card.Cursor = Cursors.Hand;

            var tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.RowCount = 2;
            tableLayout.ColumnCount = 1;

            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));

            var lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblTitle.ForeColor = color;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.BottomLeft;
            tableLayout.Controls.Add(lblTitle, 0, 0);

            var lblDesc = new Label();
            lblDesc.Text = description;
            lblDesc.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblDesc.ForeColor = UIHelper.TextSecondary;
            lblDesc.Dock = DockStyle.Fill;
            lblDesc.TextAlign = ContentAlignment.TopLeft;
            tableLayout.Controls.Add(lblDesc, 0, 1);

            card.Controls.Add(tableLayout);
            return card;
        }

        private void CreateRecentPurchasesPreview(Panel mainPanel)
        {
            var sectionPanel = new Panel();
            sectionPanel.Dock = DockStyle.Fill;
            sectionPanel.BackColor = Color.Transparent;
            sectionPanel.Padding = new Padding(0, 20, 0, 0);
            mainPanel.Controls.Add(sectionPanel);

            // Section title
            var lblSectionTitle = new Label();
            lblSectionTitle.Text = "Recent Purchases";
            lblSectionTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblSectionTitle.ForeColor = UIHelper.TextPrimary;
            lblSectionTitle.AutoSize = true;
            lblSectionTitle.Location = new Point(0, 25);
            sectionPanel.Controls.Add(lblSectionTitle);

            // DataGridView for recent purchases
            var dgv = new Guna2DataGridView();
            dgv.Location = new Point(0, 60);
            dgv.Size = new Size(900, 280);
            UIHelper.StyleDataGridView(dgv);
            sectionPanel.Controls.Add(dgv);

            // Add columns
            dgv.Columns.Add("SaleID", "Sale ID");
            dgv.Columns.Add("Date", "Date");
            dgv.Columns.Add("Total", "Total Amount");

            dgv.Columns["SaleID"].FillWeight = 20;
            dgv.Columns["Date"].FillWeight = 40;
            dgv.Columns["Total"].FillWeight = 40;

            try
            {
                if (CurrentUser.CustomerID.HasValue)
                {
                    var recentSales = _billingService.GetRecentPurchases(CurrentUser.CustomerID.Value, 5);
                    foreach (var sale in recentSales)
                    {
                        dgv.Rows.Add(sale.SaleID, sale.FormattedSaleDate, sale.FormattedTotal);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error loading recent purchases: {ex.Message}");
            }
        }

        private int GetTotalCustomerCount()
        {
            using (var conn = Database.DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Customers";
                using (var cmd = new Npgsql.NpgsqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}