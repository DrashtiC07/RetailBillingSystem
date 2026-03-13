using Guna.UI2.WinForms;
using RetailBillingSystem.Models;
using RetailBillingSystem.Services;
using RetailBillingSystem.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Forms
{
    public partial class ProductForm : Form
    {
        private ProductService _productService;
        private Guna2DataGridView dgvProducts;
        private Guna2TextBox txtSearch;
        private Guna2TextBox txtProductName;
        private Guna2TextBox txtPrice;
        private Guna2TextBox txtStock;
        private Guna2TextBox txtDescription;
        private Guna2Button btnAdd;
        private Guna2Button btnUpdate;
        private Guna2Button btnDelete;
        private Guna2Button btnClear;
        private int _selectedProductId = 0;

        public ProductForm()
        {
            _productService = new ProductService();
            InitializeComponent();
            InitializeProductForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProductForm
            // 
            this.ClientSize = new Size(950, 700);
            this.Name = "ProductForm";
            this.Text = "Product Management";
            this.BackColor = UIHelper.MainBackground;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.ResumeLayout(false);
        }

        private void InitializeProductForm()
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
            lblTitle.Text = "Product Management";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(0, 5);
            titlePanel.Controls.Add(lblTitle);

            // Subtitle
            var lblSubtitle = new Label();
            lblSubtitle.Text = "Add, edit, and manage your products";
            lblSubtitle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblSubtitle.ForeColor = UIHelper.TextSecondary;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(0, 38);
            titlePanel.Controls.Add(lblSubtitle);

            // Content area - use TableLayoutPanel for responsive two-column layout
            var contentPanel = new TableLayoutPanel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.Transparent;
            contentPanel.Padding = new Padding(0, 10, 0, 0);
            contentPanel.ColumnCount = 2;
            contentPanel.RowCount = 1;
            contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F)); // Left panel 60%
            contentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F)); // Right panel 40%
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.Controls.Add(contentPanel);

            // Left panel - Product List
            var leftPanel = CreateProductListPanel();
            leftPanel.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(leftPanel, 0, 0);

            // Right panel - Product Form
            var rightPanel = CreateProductFormPanel();
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Padding = new Padding(20, 0, 0, 0);
            contentPanel.Controls.Add(rightPanel, 1, 0);

            // Load products
            LoadProducts();
        }

        private Panel CreateProductListPanel()
        {
            var panel = new Panel();
            panel.BackColor = Color.Transparent;

            // Search bar
            var searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Top;
            searchPanel.Height = 50;
            searchPanel.BackColor = Color.Transparent;
            panel.Controls.Add(searchPanel);

            txtSearch = new Guna2TextBox();
            UIHelper.StyleTextBox(txtSearch, "Search products...");
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            searchPanel.Controls.Add(txtSearch);

            // Products DataGridView
            dgvProducts = new Guna2DataGridView();
            dgvProducts.Dock = DockStyle.Fill;
            UIHelper.StyleDataGridView(dgvProducts);
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
            panel.Controls.Add(dgvProducts);

            // Configure columns
            dgvProducts.Columns.Add("ProductID", "ID");
            dgvProducts.Columns.Add("ProductName", "Product Name");
            dgvProducts.Columns.Add("Price", "Price");
            dgvProducts.Columns.Add("Stock", "Stock");
            dgvProducts.Columns.Add("Description", "Description");

            dgvProducts.Columns["ProductID"].Visible = false;
            dgvProducts.Columns["ProductName"].FillWeight = 30;
            dgvProducts.Columns["Price"].FillWeight = 15;
            dgvProducts.Columns["Stock"].FillWeight = 10;
            dgvProducts.Columns["Description"].FillWeight = 45;

            return panel;
        }

        private Panel CreateProductFormPanel()
        {
            var panel = new Panel();
            panel.BackColor = Color.Transparent;

            // Form card
            var formCard = new Guna2ShadowPanel();
            formCard.FillColor = Color.White;
            formCard.ShadowColor = Color.Black;
            formCard.ShadowDepth = 15;
            formCard.ShadowShift = 3;
            formCard.Radius = 12;
            formCard.Dock = DockStyle.Fill;
            formCard.Padding = new Padding(20);
            panel.Controls.Add(formCard);

            // Form title
            var lblFormTitle = new Label();
            lblFormTitle.Text = "Product Details";
            lblFormTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblFormTitle.ForeColor = UIHelper.TextPrimary;
            lblFormTitle.AutoSize = true;
            lblFormTitle.Location = new Point(20, 20);
            formCard.Controls.Add(lblFormTitle);

            int yPos = 60;

            // Product Name
            var lblProductName = new Label();
            lblProductName.Text = "Product Name *";
            lblProductName.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblProductName.ForeColor = UIHelper.TextSecondary;
            lblProductName.AutoSize = true;
            lblProductName.Location = new Point(20, yPos);
            formCard.Controls.Add(lblProductName);

            yPos += 22;
            txtProductName = new Guna2TextBox();
            UIHelper.StyleTextBox(txtProductName, "");
            txtProductName.Size = new Size(290, 42);
            txtProductName.Location = new Point(20, yPos);
            formCard.Controls.Add(txtProductName);

            yPos += 60;

            // Price
            var lblPrice = new Label();
            lblPrice.Text = "Price *";
            lblPrice.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblPrice.ForeColor = UIHelper.TextSecondary;
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(20, yPos);
            formCard.Controls.Add(lblPrice);

            yPos += 22;
            txtPrice = new Guna2TextBox();
            UIHelper.StyleTextBox(txtPrice, "");
            txtPrice.Size = new Size(290, 42);
            txtPrice.Location = new Point(20, yPos);
            txtPrice.KeyPress += TxtPrice_KeyPress;
            formCard.Controls.Add(txtPrice);

            yPos += 60;

            // Stock
            var lblStock = new Label();
            lblStock.Text = "Stock Quantity *";
            lblStock.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblStock.ForeColor = UIHelper.TextSecondary;
            lblStock.AutoSize = true;
            lblStock.Location = new Point(20, yPos);
            formCard.Controls.Add(lblStock);

            yPos += 22;
            txtStock = new Guna2TextBox();
            UIHelper.StyleTextBox(txtStock, "");
            txtStock.Size = new Size(290, 42);
            txtStock.Location = new Point(20, yPos);
            txtStock.KeyPress += TxtStock_KeyPress;
            formCard.Controls.Add(txtStock);

            yPos += 60;

            // Description
            var lblDescription = new Label();
            lblDescription.Text = "Description";
            lblDescription.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblDescription.ForeColor = UIHelper.TextSecondary;
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(20, yPos);
            formCard.Controls.Add(lblDescription);

            yPos += 22;
            txtDescription = new Guna2TextBox();
            UIHelper.StyleTextBox(txtDescription, "");
            txtDescription.Size = new Size(290, 70);
            txtDescription.Location = new Point(20, yPos);
            txtDescription.Multiline = true;
            formCard.Controls.Add(txtDescription);

            yPos += 90;

            // Buttons
            btnAdd = new Guna2Button();
            UIHelper.StyleSuccessButton(btnAdd, "Add Product");
            btnAdd.Size = new Size(290, 45);
            btnAdd.Location = new Point(20, yPos);
            btnAdd.Click += BtnAdd_Click;
            formCard.Controls.Add(btnAdd);

            yPos += 55;

            btnUpdate = new Guna2Button();
            UIHelper.StylePrimaryButton(btnUpdate, "Update Product");
            btnUpdate.Size = new Size(290, 45);
            btnUpdate.Location = new Point(20, yPos);
            btnUpdate.Click += BtnUpdate_Click;
            btnUpdate.Enabled = false;
            formCard.Controls.Add(btnUpdate);

            yPos += 55;

            btnDelete = new Guna2Button();
            UIHelper.StyleDangerButton(btnDelete, "Delete Product");
            btnDelete.Size = new Size(290, 45);
            btnDelete.Location = new Point(20, yPos);
            btnDelete.Click += BtnDelete_Click;
            btnDelete.Enabled = false;
            formCard.Controls.Add(btnDelete);

            yPos += 55;

            btnClear = new Guna2Button();
            btnClear.Text = "Clear Form";
            btnClear.FillColor = Color.Gray;
            btnClear.HoverState.FillColor = Color.DimGray;
            btnClear.ForeColor = Color.White;
            btnClear.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btnClear.BorderRadius = 8;
            btnClear.Size = new Size(290, 45);
            btnClear.Location = new Point(20, yPos);
            btnClear.Click += BtnClear_Click;
            formCard.Controls.Add(btnClear);

            return panel;
        }

        private void LoadProducts()
        {
            try
            {
                dgvProducts.Rows.Clear();
                var products = _productService.GetAllProducts();

                foreach (var product in products)
                {
                    dgvProducts.Rows.Add(
                        product.ProductID,
                        product.ProductName,
                        product.FormattedPrice,
                        product.Stock,
                        product.Description
                    );

                    int rowIndex = dgvProducts.Rows.Count - 1;
                    if (product.IsLowStock)
                    {
                        dgvProducts.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
                        dgvProducts.Rows[rowIndex].DefaultCellStyle.ForeColor = UIHelper.DangerButton;
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error loading products: {ex.Message}");
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dgvProducts.Rows.Clear();
                var products = string.IsNullOrWhiteSpace(txtSearch.Text)
                    ? _productService.GetAllProducts()
                    : _productService.SearchProducts(txtSearch.Text);

                foreach (var product in products)
                {
                    dgvProducts.Rows.Add(
                        product.ProductID,
                        product.ProductName,
                        product.FormattedPrice,
                        product.Stock,
                        product.Description
                    );

                    int rowIndex = dgvProducts.Rows.Count - 1;
                    if (product.IsLowStock)
                    {
                        dgvProducts.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
                        dgvProducts.Rows[rowIndex].DefaultCellStyle.ForeColor = UIHelper.DangerButton;
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error searching products: {ex.Message}");
            }
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            // Prevent NullReferenceException by checking for valid selection
            if (dgvProducts.SelectedRows.Count == 0 || dgvProducts.Rows.Count == 0)
            {
                ClearForm();
                return;
            }

            try
            {
                var row = dgvProducts.SelectedRows[0];
                
                // Additional null checks for cell values
                if (row.Cells["ProductID"].Value == null)
                {
                    ClearForm();
                    return;
                }

                _selectedProductId = Convert.ToInt32(row.Cells["ProductID"].Value);
                txtProductName.Text = row.Cells["ProductName"].Value?.ToString() ?? "";
                txtPrice.Text = (row.Cells["Price"].Value?.ToString() ?? "0").Replace("₹", "").Replace(",", "").Trim();
                txtStock.Text = row.Cells["Stock"].Value?.ToString() ?? "0";
                txtDescription.Text = row.Cells["Description"].Value?.ToString() ?? "";

                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Enabled = false;
            }
            catch (Exception)
            {
                ClearForm();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var product = new Product
                {
                    ProductName = txtProductName.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text),
                    Stock = int.Parse(txtStock.Text),
                    Description = txtDescription.Text.Trim()
                };

                if (_productService.AddProduct(product))
                {
                    UIHelper.ShowSuccess("Product added successfully!");
                    ClearForm();
                    LoadProducts();
                }
                else
                {
                    UIHelper.ShowError("Failed to add product.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error adding product: {ex.Message}");
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                var product = new Product
                {
                    ProductID = _selectedProductId,
                    ProductName = txtProductName.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text),
                    Stock = int.Parse(txtStock.Text),
                    Description = txtDescription.Text.Trim()
                };

                if (_productService.UpdateProduct(product))
                {
                    UIHelper.ShowSuccess("Product updated successfully!");
                    ClearForm();
                    LoadProducts();
                }
                else
                {
                    UIHelper.ShowError("Failed to update product.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error updating product: {ex.Message}");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedProductId == 0) return;

            if (UIHelper.ShowConfirm("Are you sure you want to delete this product?"))
            {
                try
                {
                    if (_productService.DeleteProduct(_selectedProductId))
                    {
                        UIHelper.ShowSuccess("Product deleted successfully!");
                        ClearForm();
                        LoadProducts();
                    }
                    else
                    {
                        UIHelper.ShowError("Failed to delete product.");
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowError($"Error deleting product: {ex.Message}");
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedProductId = 0;
            txtProductName.Clear();
            txtPrice.Clear();
            txtStock.Clear();
            txtDescription.Clear();
            dgvProducts.ClearSelection();
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                UIHelper.ShowError("Please enter product name.");
                txtProductName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out _))
            {
                UIHelper.ShowError("Please enter a valid price.");
                txtPrice.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStock.Text) || !int.TryParse(txtStock.Text, out _))
            {
                UIHelper.ShowError("Please enter a valid stock quantity.");
                txtStock.Focus();
                return false;
            }

            return true;
        }

        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && txtPrice.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void TxtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
