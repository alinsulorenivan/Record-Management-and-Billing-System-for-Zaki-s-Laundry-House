using System.Drawing;
using System.Windows.Forms;

namespace ZakiLaundryHouse
{
    public static class DataGridViewStyler
    {
        // ✅ Version with selection (used for Orders, Customers, etc.)
        public static void ApplySelectableStyle(DataGridView dgv)
        {
            BaseStyle(dgv);

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.SelectionBackColor = Color.Gainsboro; // subtle gray
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        // ❌ Version without selection highlight (used for Pricing, Reports, etc.)
        public static void ApplyNonSelectableStyle(DataGridView dgv)
        {
            BaseStyle(dgv);

            // Disable selection look
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.BackColor;
            dgv.DefaultCellStyle.SelectionForeColor = dgv.DefaultCellStyle.ForeColor;
        }

        // 🔹 Common base design
        private static void BaseStyle(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = true;
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.FixedSingle;

            // ✅ Clean vertical and horizontal lines between cells
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.GridColor = Color.LightGray;

            dgv.BackgroundColor = Color.White;

            // Header styling
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 35;

            // Cell styling
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgv.RowTemplate.Height = 30;

            // ✅ Ensure vertical dividers are visible
            dgv.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.Single;
            dgv.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.Single;
        }
        // 🟦 Version with blue selection (default Windows highlight)
        public static void ApplyBlueSelectableStyle(DataGridView dgv)
        {
            BaseStyle(dgv);

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            dgv.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
        }


    }
}
