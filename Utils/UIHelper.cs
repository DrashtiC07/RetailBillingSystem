using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RetailBillingSystem.Utils
{
    /// <summary>
    /// Helper class for UI styling and common operations - Modern SaaS Dashboard Theme
    /// </summary>
    public static class UIHelper
    {
        // Modern SaaS Color Palette - Clean & Professional (inspired by Maple invoice dashboard)
        public static readonly Color SidebarBackground = Color.FromArgb(255, 255, 255);    // Clean white sidebar
        public static readonly Color SidebarHover = Color.FromArgb(243, 244, 246);         // Light gray hover
        public static readonly Color SidebarActive = Color.FromArgb(16, 185, 129);         // Green accent for active
        public static readonly Color SidebarText = Color.FromArgb(55, 65, 81);             // Dark gray text
        public static readonly Color SidebarTextActive = Color.FromArgb(16, 185, 129);     // Green active text
        public static readonly Color SidebarBorder = Color.FromArgb(229, 231, 235);        // Subtle border
        public static readonly Color MainBackground = Color.FromArgb(249, 250, 251);       // Light gray background
        public static readonly Color CardBackground = Color.White;
        public static readonly Color PrimaryButton = Color.FromArgb(16, 185, 129);         // Green primary (like Maple)
        public static readonly Color PrimaryButtonHover = Color.FromArgb(5, 150, 105);     // Darker green
        public static readonly Color SuccessButton = Color.FromArgb(16, 185, 129);         // Green
        public static readonly Color SuccessButtonHover = Color.FromArgb(5, 150, 105);     // Darker green
        public static readonly Color DangerButton = Color.FromArgb(239, 68, 68);           // Red
        public static readonly Color DangerButtonHover = Color.FromArgb(220, 38, 38);      // Darker red
        public static readonly Color WarningButton = Color.FromArgb(245, 158, 11);         // Amber
        public static readonly Color SecondaryButton = Color.FromArgb(107, 114, 128);      // Gray
        public static readonly Color TextPrimary = Color.FromArgb(17, 24, 39);             // Near black
        public static readonly Color TextSecondary = Color.FromArgb(107, 114, 128);        // Gray text
        public static readonly Color TextMuted = Color.FromArgb(156, 163, 175);            // Light gray text
        public static readonly Color BorderColor = Color.FromArgb(229, 231, 235);          // Light border
        public static readonly Color InputBackground = Color.White;                         // White input bg
        public static readonly Color AccentColor = Color.FromArgb(16, 185, 129);           // Green accent
        public static readonly Color HighlightRow = Color.FromArgb(236, 253, 245);         // Light green for selection
        public static readonly Color HeaderBackground = Color.White;                        // White header
        public static readonly Color BadgeGreen = Color.FromArgb(209, 250, 229);           // Light green badge bg
        public static readonly Color BadgeGreenText = Color.FromArgb(6, 95, 70);           // Dark green badge text
        public static readonly Color BadgeBlue = Color.FromArgb(219, 234, 254);            // Light blue badge bg
        public static readonly Color BadgeBlueText = Color.FromArgb(30, 64, 175);          // Dark blue badge text

        /// <summary>
        /// Styles a Guna2Button as a sidebar menu item - Modern clean design
        /// </summary>
        public static void StyleSidebarButton(Guna2Button button, string text, Image icon)
        {
            button.Text = text;
            button.Image = icon;
            button.ImageAlign = HorizontalAlignment.Left;
            button.TextAlign = HorizontalAlignment.Left;
            button.ImageOffset = new Point(12, 0);
            button.TextOffset = new Point(8, 0);
            button.FillColor = Color.Transparent;
            button.HoverState.FillColor = SidebarHover;
            button.ForeColor = SidebarText;
            button.HoverState.ForeColor = TextPrimary;
            button.Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular);
            button.BorderRadius = 8;
            button.Height = 44;
            button.Dock = DockStyle.Top;
            button.Margin = new Padding(8, 2, 8, 2);
        }

        /// <summary>
        /// Sets a sidebar button as active - with green accent background
        /// </summary>
        public static void SetSidebarButtonActive(Guna2Button button)
        {
            button.FillColor = Color.FromArgb(236, 253, 245);  // Light green background
            button.HoverState.FillColor = Color.FromArgb(209, 250, 229);
            button.ForeColor = SidebarActive;  // Green text
            button.HoverState.ForeColor = SidebarActive;
            button.BorderColor = Color.Transparent;
            button.BorderThickness = 0;
        }

        /// <summary>
        /// Sets a sidebar button as inactive
        /// </summary>
        public static void SetSidebarButtonInactive(Guna2Button button)
        {
            button.FillColor = Color.Transparent;
            button.HoverState.FillColor = SidebarHover;
            button.ForeColor = SidebarText;
            button.HoverState.ForeColor = TextPrimary;
            button.BorderThickness = 0;
        }

        /// <summary>
        /// Styles a Guna2Button as a primary action button - Modern rounded style
        /// </summary>
        public static void StylePrimaryButton(Guna2Button button, string text)
        {
            button.Text = text;
            button.FillColor = PrimaryButton;
            button.HoverState.FillColor = PrimaryButtonHover;
            button.PressedColor = Color.FromArgb(4, 120, 87);  // Dark green
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular);
            button.BorderRadius = 10;
            button.Height = 44;
            button.Animated = true;
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Styles a Guna2Button as a success button
        /// </summary>
        public static void StyleSuccessButton(Guna2Button button, string text)
        {
            button.Text = text;
            button.FillColor = SuccessButton;
            button.HoverState.FillColor = SuccessButtonHover;
            button.PressedColor = Color.FromArgb(4, 120, 87);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular);
            button.BorderRadius = 10;
            button.Height = 44;
            button.Animated = true;
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Styles a Guna2Button as a danger button
        /// </summary>
        public static void StyleDangerButton(Guna2Button button, string text)
        {
            button.Text = text;
            button.FillColor = DangerButton;
            button.HoverState.FillColor = DangerButtonHover;
            button.PressedColor = Color.FromArgb(185, 28, 28);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular);
            button.BorderRadius = 10;
            button.Height = 44;
            button.Animated = true;
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Styles a Guna2Button as a secondary/outline button
        /// </summary>
        public static void StyleSecondaryButton(Guna2Button button, string text)
        {
            button.Text = text;
            button.FillColor = Color.Transparent;
            button.HoverState.FillColor = Color.FromArgb(243, 244, 246);
            button.PressedColor = Color.FromArgb(229, 231, 235);
            button.ForeColor = TextPrimary;
            button.HoverState.ForeColor = TextPrimary;
            button.Font = new Font("Segoe UI Semibold", 10, FontStyle.Regular);
            button.BorderRadius = 10;
            button.BorderColor = BorderColor;
            button.BorderThickness = 1;
            button.Height = 44;
            button.Animated = true;
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Styles a Guna2TextBox for form input - Clean modern style
        /// </summary>
        public static void StyleTextBox(Guna2TextBox textBox, string placeholder)
        {
            textBox.PlaceholderText = placeholder;
            textBox.PlaceholderForeColor = TextMuted;
            textBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            textBox.BorderRadius = 8;
            textBox.BorderColor = BorderColor;
            textBox.BorderThickness = 1;
            textBox.FocusedState.BorderColor = AccentColor;
            textBox.FocusedState.BorderThickness = 2;
            textBox.HoverState.BorderColor = Color.FromArgb(156, 163, 175);
            textBox.FillColor = Color.White;
            textBox.ForeColor = TextPrimary;
            textBox.Height = 44;
            textBox.Margin = new Padding(0, 0, 0, 8);
            textBox.Padding = new Padding(12, 0, 12, 0);
            textBox.Cursor = Cursors.IBeam;
        }

        /// <summary>
        /// Styles a Guna2ShadowPanel as a card - Subtle shadow, clean borders
        /// </summary>
        public static void StyleCardPanel(Guna2ShadowPanel panel)
        {
            panel.FillColor = CardBackground;
            panel.ShadowColor = Color.FromArgb(30, 0, 0, 0);  // Subtle shadow
            panel.ShadowDepth = 8;
            panel.ShadowShift = 2;
            panel.Radius = 12;
            panel.Padding = new Padding(24);
        }

        /// <summary>
        /// Creates a modern page header with title and subtitle
        /// </summary>
        public static Panel CreatePageHeader(string title, string subtitle)
        {
            var headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.BackColor = Color.Transparent;
            headerPanel.Padding = new Padding(0, 0, 0, 16);

            var lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTitle.ForeColor = TextPrimary;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(0, 0);
            headerPanel.Controls.Add(lblTitle);

            var lblSubtitle = new Label();
            lblSubtitle.Text = subtitle;
            lblSubtitle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblSubtitle.ForeColor = TextSecondary;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(0, 38);
            headerPanel.Controls.Add(lblSubtitle);

            return headerPanel;
        }

        /// <summary>
        /// Styles a Guna2DataGridView with modern SaaS dashboard appearance - Clean minimal table
        /// </summary>
        public static void StyleDataGridView(Guna2DataGridView dgv)
        {
            // General settings
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = CardBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            
            // Header styling - Clean light header (like Maple)
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);  // Light gray
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextSecondary;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9, FontStyle.Regular);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(16, 12, 8, 12);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.EnableHeadersVisualStyles = false;
            
            // Row styling
            dgv.DefaultCellStyle.BackColor = CardBackground;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgv.DefaultCellStyle.Padding = new Padding(16, 10, 8, 10);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            
            // Selection styling - Subtle green highlight
            dgv.DefaultCellStyle.SelectionBackColor = HighlightRow;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            
            // Alternating rows - subtle stripe
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = HighlightRow;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextPrimary;
            
            // Grid styling - subtle borders
            dgv.GridColor = Color.FromArgb(243, 244, 246);
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 56;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            
            // Behavior
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.ScrollBars = ScrollBars.Vertical;
        }

        /// <summary>
        /// Styles a Guna2ComboBox
        /// </summary>
        public static void StyleComboBox(Guna2ComboBox comboBox, string placeholder)
        {
            // Guna2ComboBox does not support PlaceholderText, so use a default item as placeholder
            comboBox.Items.Clear();
            comboBox.Items.Add(placeholder);
            comboBox.SelectedIndex = 0;
            comboBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            comboBox.BorderRadius = 8;
            comboBox.BorderColor = BorderColor;
            comboBox.ForeColor = TextPrimary;
            comboBox.Height = 40;
        }

        /// <summary>
        /// Shows a success message
        /// </summary>
        public static void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a warning message
        /// </summary>
        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows a confirmation dialog
        /// </summary>
        public static bool ShowConfirm(string message)
        {
            return MessageBox.Show(message, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Creates a statistics card panel
        /// </summary>
        public static Guna2ShadowPanel CreateStatCard(string title, string value, Color iconColor, Image icon)
        {
            var panel = new Guna2ShadowPanel();
            StyleCardPanel(panel);
            panel.Size = new Size(240, 130);
            panel.Margin = new Padding(15);

            var tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.ColumnCount = 2;
            tableLayout.RowCount = 2;
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            // Icon panel
            var iconPanel = new Guna2Panel();
            iconPanel.FillColor = Color.FromArgb(30, iconColor);
            iconPanel.BorderRadius = 10;
            iconPanel.Size = new Size(50, 50);
            iconPanel.Dock = DockStyle.Fill;
            iconPanel.Margin = new Padding(5);

            if (icon != null)
            {
                var picBox = new PictureBox();
                picBox.Image = icon;
                picBox.SizeMode = PictureBoxSizeMode.Zoom;
                picBox.Dock = DockStyle.Fill;
                picBox.Padding = new Padding(10);
                iconPanel.Controls.Add(picBox);
            }

            tableLayout.Controls.Add(iconPanel, 0, 0);
            tableLayout.SetRowSpan(iconPanel, 2);

            // Title label
            var lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblTitle.ForeColor = TextSecondary;
            lblTitle.Dock = DockStyle.Bottom;
            tableLayout.Controls.Add(lblTitle, 1, 0);

            // Value label
            var lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblValue.ForeColor = TextPrimary;
            lblValue.Dock = DockStyle.Top;
            tableLayout.Controls.Add(lblValue, 1, 1);

            panel.Controls.Add(tableLayout);
            return panel;
        }
    }
}
