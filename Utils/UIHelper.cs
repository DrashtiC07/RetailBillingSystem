using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RetailBillingSystem.Utils
{
    /// <summary>
    /// Helper class for UI styling and common operations
    /// </summary>
    public static class UIHelper
    {
        // Color Palette
        public static readonly Color SidebarBackground = Color.FromArgb(31, 42, 64);      // #1F2A40
        public static readonly Color SidebarHover = Color.FromArgb(44, 62, 80);          // #2C3E50
        public static readonly Color SidebarActive = Color.FromArgb(59, 130, 246);       // #3B82F6
        public static readonly Color MainBackground = Color.FromArgb(243, 244, 246);     // #F3F4F6
        public static readonly Color CardBackground = Color.White;
        public static readonly Color PrimaryButton = Color.FromArgb(59, 130, 246);       // #3B82F6
        public static readonly Color SuccessButton = Color.FromArgb(34, 197, 94);        // #22C55E
        public static readonly Color DangerButton = Color.FromArgb(239, 68, 68);         // #EF4444
        public static readonly Color WarningButton = Color.FromArgb(245, 158, 11);       // #F59E0B
        public static readonly Color TextPrimary = Color.FromArgb(31, 41, 55);           // #1F2937
        public static readonly Color TextSecondary = Color.FromArgb(107, 114, 128);      // #6B7280
        public static readonly Color BorderColor = Color.FromArgb(229, 231, 235);        // #E5E7EB

        /// <summary>
        /// Styles a Guna2Button as a sidebar menu item
        /// </summary>
        public static void StyleSidebarButton(Guna2Button button, string text, Image icon)
        {
            button.Text = text;
            button.Image = icon;
            button.ImageAlign = HorizontalAlignment.Left;
            button.TextAlign = HorizontalAlignment.Left;
            button.ImageOffset = new Point(10, 0);
            button.TextOffset = new Point(10, 0);
            button.FillColor = SidebarBackground;
            button.HoverState.FillColor = SidebarHover;
            button.ForeColor = Color.White;
            button.HoverState.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            button.BorderRadius = 0;
            button.Height = 50;
            button.Dock = DockStyle.Top;
        }

        /// <summary>
        /// Sets a sidebar button as active
        /// </summary>
        public static void SetSidebarButtonActive(Guna2Button button)
        {
            button.FillColor = SidebarActive;
            button.HoverState.FillColor = SidebarActive;
        }

        /// <summary>
        /// Sets a sidebar button as inactive
        /// </summary>
        public static void SetSidebarButtonInactive(Guna2Button button)
        {
            button.FillColor = SidebarBackground;
            button.HoverState.FillColor = SidebarHover;
        }

        /// <summary>
        /// Styles a Guna2Button as a primary action button
        /// </summary>
        public static void StylePrimaryButton(Guna2Button button, string text)
        {
            button.Text = text;
            button.FillColor = PrimaryButton;
            button.HoverState.FillColor = Color.FromArgb(37, 99, 235);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            button.BorderRadius = 8;
            button.Height = 40;
        }

        /// <summary>
        /// Styles a Guna2Button as a success button
        /// </summary>
        public static void StyleSuccessButton(Guna2Button button, string text)
        {
            button.Text = text;
            button.FillColor = SuccessButton;
            button.HoverState.FillColor = Color.FromArgb(22, 163, 74);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            button.BorderRadius = 8;
            button.Height = 40;
        }

        /// <summary>
        /// Styles a Guna2Button as a danger button
        /// </summary>
        public static void StyleDangerButton(Guna2Button button, string text)
        {
            button.Text = text;
            button.FillColor = DangerButton;
            button.HoverState.FillColor = Color.FromArgb(220, 38, 38);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            button.BorderRadius = 8;
            button.Height = 40;
        }

        /// <summary>
        /// Styles a Guna2TextBox for form input
        /// </summary>
        public static void StyleTextBox(Guna2TextBox textBox, string placeholder)
        {
            textBox.PlaceholderText = placeholder;
            textBox.PlaceholderForeColor = TextSecondary;
            textBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            textBox.BorderRadius = 6;  // Reduced to prevent overlapping
            textBox.BorderColor = BorderColor;
            textBox.FocusedState.BorderColor = PrimaryButton;
            textBox.ForeColor = TextPrimary;
            textBox.Height = 42;
            textBox.Margin = new Padding(0, 0, 0, 10);
        }

        /// <summary>
        /// Styles a Guna2ShadowPanel as a card
        /// </summary>
        public static void StyleCardPanel(Guna2ShadowPanel panel)
        {
            panel.FillColor = CardBackground;
            panel.ShadowColor = Color.Black;
            panel.ShadowDepth = 20;
            panel.ShadowShift = 4;
            panel.Radius = 12; // Use Radius property instead of BorderRadius
            panel.Padding = new Padding(20);
        }

        /// <summary>
        /// Styles a Guna2DataGridView with modern dashboard appearance
        /// </summary>
        public static void StyleDataGridView(Guna2DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = CardBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SidebarBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgv.ColumnHeadersHeight = 45;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.DefaultCellStyle.BackColor = CardBackground;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = SidebarActive;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = BorderColor;
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 40;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
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
