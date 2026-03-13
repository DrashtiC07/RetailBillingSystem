using Guna.UI2.WinForms;
using RetailBillingSystem.Models;
using RetailBillingSystem.Services;
using RetailBillingSystem.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Forms
{
    public partial class BillingForm : Form
    {
        private ProductService _productService;
        private BillingService _billingService;
        private List<SaleItem> _cartItems;

        // Controls
        private Guna2ComboBox cmbProducts;
        private Guna2TextBox txtQuantity;
        private Guna2TextBox txtPrice;
        private Guna2TextBox txtAvailableStock;
        private Guna2Button btnAddToCart;
        private Guna2Button btnRemoveFromCart;
        private Guna2Button btnGenerateBill;
        private Guna2Button btnClearCart;
        private Guna2DataGridView dgvCart;
        private Label lblSubtotal;
        private Label lblTax;
        private Label lblTotal;

        public BillingForm()
        {
            _productService = new ProductService();
            _billingService = new BillingService();
            _cartItems = new List<SaleItem>();
            InitializeComponent();
            InitializeBillingForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BillingForm
            // 
            this.ClientSize = new Size(950, 700);
            this.Name = "BillingForm";
            this.Text = "Billing";
            this.BackColor = UIHelper.MainBackground;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ResumeLayout(false);
        }

        private void InitializeBillingForm()
        {
            // Main container with proper padding
            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = UIHelper.MainBackground;
            mainPanel.Padding = new Padding(0);
            mainPanel.AutoScroll = true;
            this.Controls.Add(mainPanel);

            // Title section - Clean header card
            var titleCard = new Guna2ShadowPanel();
            titleCard.Dock = DockStyle.Top;
            titleCard.Height = 80;
            titleCard.FillColor = Color.White;
            titleCard.ShadowColor = Color.FromArgb(20, 0, 0, 0);
            titleCard.ShadowDepth = 8;
            titleCard.ShadowShift = 2;
            titleCard.Radius = 12;
            titleCard.Padding = new Padding(24, 0, 24, 0);
            mainPanel.Controls.Add(titleCard);

            // Title with invoice number style
            var lblTitle = new Label();
            lblTitle.Text = "New Invoice";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(24, 16);
            titleCard.Controls.Add(lblTitle);

            // Invoice number badge
            var invoiceBadge = new Guna2Panel();
            invoiceBadge.FillColor = UIHelper.BadgeGreen;
            invoiceBadge.BorderRadius = 12;
            invoiceBadge.Size = new Size(140, 26);
            invoiceBadge.Location = new Point(24, 44);

            var lblInvoiceNum = new Label();
            lblInvoiceNum.Text = $"INV-{DateTime.Now:yyyyMMdd}";
            lblInvoiceNum.Font = new Font("Segoe UI Semibold", 9, FontStyle.Regular);
            lblInvoiceNum.ForeColor = UIHelper.BadgeGreenText;
            lblInvoiceNum.AutoSize = true;
            lblInvoiceNum.Location = new Point(12, 5);
            invoiceBadge.Controls.Add(lblInvoiceNum);
            titleCard.Controls.Add(invoiceBadge);

            // Main content area - use TableLayoutPanel for responsive two-column layout
            var contentPanel = new TableLayoutPanel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.Transparent;
            contentPanel.Padding = new Padding(20, 20, 20, 20);
            contentPanel.ColumnCount = 2;
            contentPanel.RowCount = 1;
            contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F)); // Left panel 60%
            contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F)); // Right panel 40%
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.Controls.Add(contentPanel);

            // Left side - Product selection and Cart
            var leftPanel = CreateLeftPanel();
            leftPanel.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(leftPanel, 0, 0);

            // Right side - Bill Summary
            var rightPanel = CreateRightPanel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Padding = new Padding(20, 0, 0, 0);
            contentPanel.Controls.Add(rightPanel, 1, 0);

            // Load products
            LoadProducts();
        }

        private Panel CreateLeftPanel()
        {
            var panel = new Panel();
            panel.BackColor = Color.Transparent;

            // Product selection card - clean minimal shadow
            var selectionCard = new Guna2ShadowPanel();
            selectionCard.FillColor = Color.White;
            selectionCard.ShadowColor = Color.FromArgb(20, 0, 0, 0);
            selectionCard.ShadowDepth = 10;
            selectionCard.ShadowShift = 2;
            selectionCard.Radius = 12;
            selectionCard.Dock = DockStyle.Top;
            selectionCard.Height = 240;
            selectionCard.Padding = new Padding(24);
            panel.Controls.Add(selectionCard);

            // Card title with icon-style header
            var lblCardTitle = new Label();
            lblCardTitle.Text = "Item Details";
            lblCardTitle.Font = new Font("Segoe UI Semibold", 13, FontStyle.Regular);
            lblCardTitle.ForeColor = UIHelper.TextPrimary;
            lblCardTitle.AutoSize = true;
            lblCardTitle.Location = new Point(24, 16);
            selectionCard.Controls.Add(lblCardTitle);

            // Product ComboBox
            var lblProduct = new Label();
            lblProduct.Text = "Product:";
            lblProduct.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblProduct.ForeColor = UIHelper.TextSecondary;
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(20, 50);
            selectionCard.Controls.Add(lblProduct);

            cmbProducts = new Guna2ComboBox();
            UIHelper.StyleComboBox(cmbProducts, "Select a product");
            cmbProducts.Size = new Size(460, 40);
            cmbProducts.Location = new Point(20, 72);
            cmbProducts.SelectedIndexChanged += CmbProducts_SelectedIndexChanged;
            selectionCard.Controls.Add(cmbProducts);

            // Price display
            var lblPrice = new Label();
            lblPrice.Text = "Price:";
            lblPrice.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblPrice.ForeColor = UIHelper.TextSecondary;
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(20, 120);
            selectionCard.Controls.Add(lblPrice);

            txtPrice = new Guna2TextBox();
            UIHelper.StyleTextBox(txtPrice, "Price");
            txtPrice.Size = new Size(140, 40);
            txtPrice.Location = new Point(20, 142);
            txtPrice.ReadOnly = true;
            txtPrice.Text = "₹0.00";
            selectionCard.Controls.Add(txtPrice);

            // Available stock
            var lblStock = new Label();
            lblStock.Text = "Stock:";
            lblStock.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblStock.ForeColor = UIHelper.TextSecondary;
            lblStock.AutoSize = true;
            lblStock.Location = new Point(180, 120);
            selectionCard.Controls.Add(lblStock);

            txtAvailableStock = new Guna2TextBox();
            UIHelper.StyleTextBox(txtAvailableStock, "Stock");
            txtAvailableStock.Size = new Size(140, 40);
            txtAvailableStock.Location = new Point(180, 142);
            txtAvailableStock.ReadOnly = true;
            txtAvailableStock.Text = "0";
            selectionCard.Controls.Add(txtAvailableStock);

            // Quantity
            var lblQuantity = new Label();
            lblQuantity.Text = "Quantity:";
            lblQuantity.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblQuantity.ForeColor = UIHelper.TextSecondary;
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new Point(340, 120);
            selectionCard.Controls.Add(lblQuantity);

            txtQuantity = new Guna2TextBox();
            UIHelper.StyleTextBox(txtQuantity, "Qty");
            txtQuantity.Size = new Size(140, 40);
            txtQuantity.Location = new Point(340, 142);
            txtQuantity.KeyPress += TxtQuantity_KeyPress;
            selectionCard.Controls.Add(txtQuantity);

            // Add to cart button
            btnAddToCart = new Guna2Button();
            UIHelper.StyleSuccessButton(btnAddToCart, "Add to Cart");
            btnAddToCart.Size = new Size(460, 45);
            btnAddToCart.Location = new Point(20, 195);
            btnAddToCart.Click += BtnAddToCart_Click;
            selectionCard.Controls.Add(btnAddToCart);

            // Buttons row - dock to bottom first (reverse order for docking)
            var buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 55;
            buttonPanel.BackColor = Color.Transparent;
            panel.Controls.Add(buttonPanel);

            // Cart section title - dock below selection card
            var cartTitlePanel = new Panel();
            cartTitlePanel.Dock = DockStyle.Top;
            cartTitlePanel.Height = 40;
            cartTitlePanel.BackColor = Color.Transparent;
            panel.Controls.Add(cartTitlePanel);

            var lblCartTitle = new Label();
            lblCartTitle.Text = "Cart Items";
            lblCartTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblCartTitle.ForeColor = UIHelper.TextPrimary;
            lblCartTitle.AutoSize = true;
            lblCartTitle.Location = new Point(0, 10);
            cartTitlePanel.Controls.Add(lblCartTitle);

            // Cart DataGridView - fill remaining space
            dgvCart = new Guna2DataGridView();
            dgvCart.Dock = DockStyle.Fill;
            UIHelper.StyleDataGridView(dgvCart);
            dgvCart.SelectionChanged += DgvCart_SelectionChanged;
            panel.Controls.Add(dgvCart);

            // Configure columns
            dgvCart.Columns.Add("ProductID", "ID");
            dgvCart.Columns.Add("ProductName", "Product");
            dgvCart.Columns.Add("UnitPrice", "Price");
            dgvCart.Columns.Add("Quantity", "Qty");
            dgvCart.Columns.Add("TotalPrice", "Total");

            dgvCart.Columns["ProductID"].Visible = false;
            dgvCart.Columns["ProductName"].FillWeight = 35;
            dgvCart.Columns["UnitPrice"].FillWeight = 20;
            dgvCart.Columns["Quantity"].FillWeight = 15;
            dgvCart.Columns["TotalPrice"].FillWeight = 30;

            // Remove button
            btnRemoveFromCart = new Guna2Button();
            UIHelper.StyleDangerButton(btnRemoveFromCart, "Remove Selected");
            btnRemoveFromCart.Size = new Size(150, 45);
            btnRemoveFromCart.Location = new Point(0, 0);
            btnRemoveFromCart.Click += BtnRemoveFromCart_Click;
            btnRemoveFromCart.Enabled = false;
            buttonPanel.Controls.Add(btnRemoveFromCart);

            // Clear cart button
            btnClearCart = new Guna2Button();
            btnClearCart.Text = "Clear Cart";
            btnClearCart.FillColor = Color.Gray;
            btnClearCart.HoverState.FillColor = Color.DimGray;
            btnClearCart.ForeColor = Color.White;
            btnClearCart.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btnClearCart.BorderRadius = 8;
            btnClearCart.Size = new Size(120, 45);
            btnClearCart.Location = new Point(165, 0);
            btnClearCart.Click += BtnClearCart_Click;
            buttonPanel.Controls.Add(btnClearCart);

            return panel;
        }

        private Panel CreateRightPanel()
        {
            var panel = new Panel();
            panel.BackColor = Color.Transparent;

            // Summary card - clean minimal shadow like Maple
            var summaryCard = new Guna2ShadowPanel();
            summaryCard.FillColor = Color.White;
            summaryCard.ShadowColor = Color.FromArgb(20, 0, 0, 0);
            summaryCard.ShadowDepth = 10;
            summaryCard.ShadowShift = 2;
            summaryCard.Radius = 12;
            summaryCard.Dock = DockStyle.Top;
            summaryCard.Height = 340;
            summaryCard.Padding = new Padding(24);
            panel.Controls.Add(summaryCard);

            // Summary title with badge
            var lblSummaryTitle = new Label();
            lblSummaryTitle.Text = "Bill Summary";
            lblSummaryTitle.Font = new Font("Segoe UI Semibold", 14, FontStyle.Regular);
            lblSummaryTitle.ForeColor = UIHelper.TextPrimary;
            lblSummaryTitle.AutoSize = true;
            lblSummaryTitle.Location = new Point(24, 20);
            summaryCard.Controls.Add(lblSummaryTitle);

            // Status badge
            var statusBadge = new Guna2Panel();
            statusBadge.FillColor = UIHelper.BadgeGreen;
            statusBadge.BorderRadius = 10;
            statusBadge.Size = new Size(50, 22);
            statusBadge.Location = new Point(260, 20);
            var statusLabel = new Label();
            statusLabel.Text = "Draft";
            statusLabel.Font = new Font("Segoe UI Semibold", 8, FontStyle.Regular);
            statusLabel.ForeColor = UIHelper.BadgeGreenText;
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(8, 3);
            statusBadge.Controls.Add(statusLabel);
            summaryCard.Controls.Add(statusBadge);

            // Subtotal row
            var lblSubtotalTitle = new Label();
            lblSubtotalTitle.Text = "Subtotal";
            lblSubtotalTitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblSubtotalTitle.ForeColor = UIHelper.TextSecondary;
            lblSubtotalTitle.AutoSize = true;
            lblSubtotalTitle.Location = new Point(24, 70);
            summaryCard.Controls.Add(lblSubtotalTitle);

            lblSubtotal = new Label();
            lblSubtotal.Text = "0.00";
            lblSubtotal.Font = new Font("Segoe UI Semibold", 11, FontStyle.Regular);
            lblSubtotal.ForeColor = UIHelper.TextPrimary;
            lblSubtotal.AutoSize = true;
            lblSubtotal.Location = new Point(260, 70);
            summaryCard.Controls.Add(lblSubtotal);

            // Tax row
            var lblTaxTitle = new Label();
            lblTaxTitle.Text = "Tax (18% GST)";
            lblTaxTitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblTaxTitle.ForeColor = UIHelper.TextSecondary;
            lblTaxTitle.AutoSize = true;
            lblTaxTitle.Location = new Point(24, 100);
            summaryCard.Controls.Add(lblTaxTitle);

            lblTax = new Label();
            lblTax.Text = "0.00";
            lblTax.Font = new Font("Segoe UI Semibold", 11, FontStyle.Regular);
            lblTax.ForeColor = UIHelper.TextPrimary;
            lblTax.AutoSize = true;
            lblTax.Location = new Point(260, 100);
            summaryCard.Controls.Add(lblTax);

            // Separator line
            var separator = new Panel();
            separator.Location = new Point(24, 140);
            separator.Size = new Size(290, 1);
            separator.BackColor = UIHelper.BorderColor;
            summaryCard.Controls.Add(separator);

            // Total row - emphasized
            var lblTotalTitle = new Label();
            lblTotalTitle.Text = "Total Amount";
            lblTotalTitle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Regular);
            lblTotalTitle.ForeColor = UIHelper.TextPrimary;
            lblTotalTitle.AutoSize = true;
            lblTotalTitle.Location = new Point(24, 160);
            summaryCard.Controls.Add(lblTotalTitle);

            lblTotal = new Label();
            lblTotal.Text = "0.00";
            lblTotal.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTotal.ForeColor = UIHelper.AccentColor;
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(200, 155);
            summaryCard.Controls.Add(lblTotal);

            // Currency note
            var lblCurrency = new Label();
            lblCurrency.Text = "INR";
            lblCurrency.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblCurrency.ForeColor = UIHelper.TextMuted;
            lblCurrency.AutoSize = true;
            lblCurrency.Location = new Point(24, 185);
            summaryCard.Controls.Add(lblCurrency);

            // Generate Bill button - green accent
            btnGenerateBill = new Guna2Button();
            UIHelper.StylePrimaryButton(btnGenerateBill, "Send Invoice");
            btnGenerateBill.Size = new Size(290, 50);
            btnGenerateBill.Location = new Point(24, 220);
            btnGenerateBill.Font = new Font("Segoe UI Semibold", 11, FontStyle.Regular);
            btnGenerateBill.Click += BtnGenerateBill_Click;
            btnGenerateBill.Enabled = false;
            summaryCard.Controls.Add(btnGenerateBill);

            // Secondary action - Save Draft
            var btnSaveDraft = new Guna2Button();
            UIHelper.StyleSecondaryButton(btnSaveDraft, "Save Draft");
            btnSaveDraft.Size = new Size(290, 44);
            btnSaveDraft.Location = new Point(24, 280);
            btnSaveDraft.Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular);
            summaryCard.Controls.Add(btnSaveDraft);

            return panel;
        }

        private void LoadProducts()
        {
            try
            {
                cmbProducts.Items.Clear();
                cmbProducts.DisplayMember = "ProductName";
                cmbProducts.ValueMember = "ProductID";

                var products = _productService.GetAllProducts();
                foreach (var product in products)
                {
                    if (product.Stock > 0)
                    {
                        cmbProducts.Items.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error loading products: {ex.Message}");
            }
        }

        private void CmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem is Product product)
            {
                txtPrice.Text = $"₹{product.Price:N2}";
                txtAvailableStock.Text = product.Stock.ToString();
                txtQuantity.Text = "1";
            }
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem == null)
            {
                UIHelper.ShowError("Please select a product.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                UIHelper.ShowError("Please enter a valid quantity.");
                txtQuantity.Focus();
                return;
            }

            var product = cmbProducts.SelectedItem as Product;

            if (quantity > product.Stock)
            {
                UIHelper.ShowError($"Insufficient stock. Available: {product.Stock}");
                txtQuantity.Focus();
                return;
            }

            var existingItem = _cartItems.Find(item => item.ProductID == product.ProductID);
            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > product.Stock)
                {
                    UIHelper.ShowError($"Cannot add more. Total would exceed available stock.");
                    return;
                }
                existingItem.Quantity += quantity;
                existingItem.CalculateTotal();
            }
            else
            {
                _cartItems.Add(new SaleItem(product, quantity));
            }

            RefreshCart();
            ClearSelection();
        }

        private void BtnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0 || dgvCart.Rows.Count == 0) return;
            
            try
            {
                var row = dgvCart.SelectedRows[0];
                if (row.Cells["ProductID"].Value == null) return;
                
                int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                var item = _cartItems.Find(i => i.ProductID == productId);
                if (item != null)
                {
                    _cartItems.Remove(item);
                    RefreshCart();
                }
            }
            catch (Exception)
            {
                // Silently ignore selection errors
            }
        }

        private void BtnClearCart_Click(object sender, EventArgs e)
        {
            if (_cartItems.Count > 0 && UIHelper.ShowConfirm("Are you sure you want to clear the cart?"))
            {
                _cartItems.Clear();
                RefreshCart();
            }
        }

        private void BtnGenerateBill_Click(object sender, EventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                UIHelper.ShowError("Cart is empty.");
                return;
            }

            try
            {
                decimal subtotal = CalculateSubtotal();
                decimal tax = subtotal * 0.18m;
                decimal total = subtotal + tax;

                var sale = new Sale
                {
                    CustomerID = CurrentUser.IsCustomer ? CurrentUser.CustomerID : null,
                    Subtotal = subtotal,
                    TaxAmount = tax,
                    TotalAmount = total
                };

                if (_billingService.CreateSale(sale, _cartItems))
                {
                    UIHelper.ShowSuccess($"Bill generated successfully!\n\nSale ID: {sale.SaleID}\nTotal: ₹{total:N2}");
                    _cartItems.Clear();
                    RefreshCart();
                    LoadProducts();
                }
                else
                {
                    UIHelper.ShowError("Failed to generate bill.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error generating bill: {ex.Message}");
            }
        }

        private void DgvCart_SelectionChanged(object sender, EventArgs e)
        {
            // Prevent NullReferenceException
            btnRemoveFromCart.Enabled = dgvCart.SelectedRows.Count > 0 && dgvCart.Rows.Count > 0;
        }

        private void RefreshCart()
        {
            dgvCart.Rows.Clear();

            foreach (var item in _cartItems)
            {
                dgvCart.Rows.Add(
                    item.ProductID,
                    item.ProductName,
                    item.FormattedUnitPrice,
                    item.Quantity,
                    item.FormattedTotalPrice
                );
            }

            UpdateTotals();
            btnRemoveFromCart.Enabled = dgvCart.SelectedRows.Count > 0;
            btnGenerateBill.Enabled = _cartItems.Count > 0;
        }

        private void UpdateTotals()
        {
            decimal subtotal = CalculateSubtotal();
            decimal tax = subtotal * 0.18m;
            decimal total = subtotal + tax;

            lblSubtotal.Text = $"₹{subtotal:N2}";
            lblTax.Text = $"₹{tax:N2}";
            lblTotal.Text = $"₹{total:N2}";
        }

        private decimal CalculateSubtotal()
        {
            decimal subtotal = 0;
            foreach (var item in _cartItems)
            {
                subtotal += item.TotalPrice;
            }
            return subtotal;
        }

        private void ClearSelection()
        {
            cmbProducts.SelectedIndex = -1;
            txtPrice.Text = "₹0.00";
            txtAvailableStock.Text = "0";
            txtQuantity.Clear();
        }

        private void TxtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
