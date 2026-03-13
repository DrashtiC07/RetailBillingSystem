using Guna.UI2.WinForms;
using RetailBillingSystem.Models;
using RetailBillingSystem.Services;
using RetailBillingSystem.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Forms
{
    public partial class SalesHistoryForm : Form
    {
        private BillingService _billingService;
        private Guna2DataGridView dgvSales;
        private Guna2TextBox txtSearch;
        private Guna2Button btnViewDetails;
        private Guna2Button btnRefresh;
        private int _selectedSaleId = 0;

        public SalesHistoryForm()
        {
            _billingService = new BillingService();
            InitializeComponent();
            InitializeSalesHistoryForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SalesHistoryForm
            // 
            this.ClientSize = new Size(950, 700);
            this.Name = "SalesHistoryForm";
            this.Text = "Sales History";
            this.BackColor = UIHelper.MainBackground;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ResumeLayout(false);
        }

        private void InitializeSalesHistoryForm()
        {
            // Main container panel
            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = UIHelper.MainBackground;
            mainPanel.Padding = new Padding(0);
            mainPanel.AutoScroll = true;
            this.Controls.Add(mainPanel);

            // Title section - Clean header card
            var titleCard = new Guna2ShadowPanel();
            titleCard.Dock = DockStyle.Top;
            titleCard.Height = 70;
            titleCard.FillColor = Color.White;
            titleCard.ShadowColor = Color.FromArgb(20, 0, 0, 0);
            titleCard.ShadowDepth = 8;
            titleCard.ShadowShift = 2;
            titleCard.Radius = 12;
            titleCard.Padding = new Padding(24, 0, 24, 0);
            mainPanel.Controls.Add(titleCard);

            // Title
            var lblTitle = new Label();
            lblTitle.Text = CurrentUser.IsAdmin ? "Sales History" : "My Purchases";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(24, 14);
            titleCard.Controls.Add(lblTitle);

            // Subtitle
            var lblSubtitle = new Label();
            lblSubtitle.Text = CurrentUser.IsAdmin
                ? "View all sales transactions"
                : "View your purchase history";
            lblSubtitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblSubtitle.ForeColor = UIHelper.TextSecondary;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(24, 40);
            titleCard.Controls.Add(lblSubtitle);

            // Content panel
            var contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.Transparent;
            contentPanel.Padding = new Padding(20, 20, 20, 20);
            mainPanel.Controls.Add(contentPanel);

            // Top action bar using FlowLayoutPanel for responsive layout
            var actionPanel = new FlowLayoutPanel();
            actionPanel.Dock = DockStyle.Top;
            actionPanel.Height = 55;
            actionPanel.BackColor = Color.Transparent;
            actionPanel.FlowDirection = FlowDirection.LeftToRight;
            actionPanel.WrapContents = false;
            actionPanel.Padding = new Padding(0, 0, 0, 10);
            contentPanel.Controls.Add(actionPanel);

            // Search textbox (admin only)
            if (CurrentUser.IsAdmin)
            {
                txtSearch = new Guna2TextBox();
                UIHelper.StyleTextBox(txtSearch, "Search by customer name...");
                txtSearch.Size = new Size(300, 42);
                txtSearch.Margin = new Padding(0, 0, 15, 0);
                txtSearch.TextChanged += TxtSearch_TextChanged;
                actionPanel.Controls.Add(txtSearch);
            }

            // Refresh button
            btnRefresh = new Guna2Button();
            UIHelper.StylePrimaryButton(btnRefresh, "Refresh");
            btnRefresh.Size = new Size(120, 42);
            btnRefresh.Margin = new Padding(0, 0, 15, 0);
            btnRefresh.Click += BtnRefresh_Click;
            actionPanel.Controls.Add(btnRefresh);

            // View Details button
            btnViewDetails = new Guna2Button();
            UIHelper.StyleSuccessButton(btnViewDetails, "View Details");
            btnViewDetails.Size = new Size(150, 42);
            btnViewDetails.Margin = new Padding(0);
            btnViewDetails.Click += BtnViewDetails_Click;
            btnViewDetails.Enabled = false;
            actionPanel.Controls.Add(btnViewDetails);

            // Sales DataGridView
            dgvSales = new Guna2DataGridView();
            dgvSales.Dock = DockStyle.Fill;
            UIHelper.StyleDataGridView(dgvSales);
            dgvSales.SelectionChanged += DgvSales_SelectionChanged;
            contentPanel.Controls.Add(dgvSales);

            // Configure columns
            dgvSales.Columns.Add("SaleID", "Sale ID");
            dgvSales.Columns.Add("SaleDate", "Date");

            if (CurrentUser.IsAdmin)
            {
                dgvSales.Columns.Add("CustomerName", "Customer");
            }

            dgvSales.Columns.Add("Subtotal", "Subtotal");
            dgvSales.Columns.Add("TaxAmount", "Tax");
            dgvSales.Columns.Add("TotalAmount", "Total");

            dgvSales.Columns["SaleID"].FillWeight = 10;
            dgvSales.Columns["SaleDate"].FillWeight = 20;

            if (CurrentUser.IsAdmin)
            {
                dgvSales.Columns["CustomerName"].FillWeight = 20;
            }

            dgvSales.Columns["Subtotal"].FillWeight = 15;
            dgvSales.Columns["TaxAmount"].FillWeight = 15;
            dgvSales.Columns["TotalAmount"].FillWeight = 20;

            // Load sales
            LoadSales();
        }

        private void LoadSales()
        {
            try
            {
                dgvSales.Rows.Clear();

                var sales = CurrentUser.IsAdmin
                    ? _billingService.GetAllSales()
                    : _billingService.GetSalesByCustomer(CurrentUser.CustomerID ?? 0);

                foreach (var sale in sales)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(dgvSales);
                    row.Cells[0].Value = sale.SaleID;
                    row.Cells[1].Value = sale.FormattedSaleDate;

                    int colIndex = 2;
                    if (CurrentUser.IsAdmin)
                    {
                        row.Cells[colIndex].Value = sale.CustomerName;
                        colIndex++;
                    }

                    row.Cells[colIndex].Value = sale.FormattedSubtotal;
                    row.Cells[colIndex + 1].Value = sale.FormattedTax;
                    row.Cells[colIndex + 2].Value = sale.FormattedTotal;

                    dgvSales.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error loading sales: {ex.Message}");
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!CurrentUser.IsAdmin) return;

            try
            {
                dgvSales.Rows.Clear();
                var sales = _billingService.GetAllSales();

                foreach (var sale in sales)
                {
                    if (string.IsNullOrWhiteSpace(txtSearch.Text) ||
                        sale.CustomerName.ToLower().Contains(txtSearch.Text.ToLower()))
                    {
                        dgvSales.Rows.Add(
                            sale.SaleID,
                            sale.FormattedSaleDate,
                            sale.CustomerName,
                            sale.FormattedSubtotal,
                            sale.FormattedTax,
                            sale.FormattedTotal
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error searching sales: {ex.Message}");
            }
        }

        private void DgvSales_SelectionChanged(object sender, EventArgs e)
        {
            // Prevent NullReferenceException
            if (dgvSales.SelectedRows.Count == 0 || dgvSales.Rows.Count == 0)
            {
                _selectedSaleId = 0;
                btnViewDetails.Enabled = false;
                return;
            }

            try
            {
                var row = dgvSales.SelectedRows[0];
                if (row.Cells["SaleID"].Value != null)
                {
                    _selectedSaleId = Convert.ToInt32(row.Cells["SaleID"].Value);
                    btnViewDetails.Enabled = true;
                }
                else
                {
                    _selectedSaleId = 0;
                    btnViewDetails.Enabled = false;
                }
            }
            catch (Exception)
            {
                _selectedSaleId = 0;
                btnViewDetails.Enabled = false;
            }
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (_selectedSaleId == 0) return;

            try
            {
                var items = _billingService.GetSaleItems(_selectedSaleId);
                ShowSaleDetailsDialog(_selectedSaleId, items);
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error loading sale details: {ex.Message}");
            }
        }

        private void ShowSaleDetailsDialog(int saleId, System.Collections.Generic.List<SaleItem> items)
        {
            var dialog = new Form();
            dialog.Text = $"Sale Details - #{saleId}";
            dialog.Size = new Size(600, 500);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = UIHelper.MainBackground;

            // Title
            var lblTitle = new Label();
            lblTitle.Text = $"Sale #{saleId} - Items";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 20);
            dialog.Controls.Add(lblTitle);

            // Items DataGridView
            var dgvItems = new Guna2DataGridView();
            dgvItems.Location = new Point(20, 60);
            dgvItems.Size = new Size(540, 320);
            UIHelper.StyleDataGridView(dgvItems);
            dialog.Controls.Add(dgvItems);

            dgvItems.Columns.Add("ProductName", "Product");
            dgvItems.Columns.Add("UnitPrice", "Unit Price");
            dgvItems.Columns.Add("Quantity", "Quantity");
            dgvItems.Columns.Add("TotalPrice", "Total");

            foreach (var item in items)
            {
                dgvItems.Rows.Add(
                    item.ProductName,
                    item.FormattedUnitPrice,
                    item.Quantity,
                    item.FormattedTotalPrice
                );
            }

            // Close button
            var btnClose = new Guna2Button();
            UIHelper.StylePrimaryButton(btnClose, "Close");
            btnClose.Location = new Point(230, 400);
            btnClose.Size = new Size(120, 40);
            btnClose.Click += (s, e) => dialog.Close();
            dialog.Controls.Add(btnClose);

            dialog.ShowDialog(this);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (txtSearch != null)
            {
                txtSearch.Clear();
            }
            LoadSales();
            btnViewDetails.Enabled = false;
        }
    }
}
