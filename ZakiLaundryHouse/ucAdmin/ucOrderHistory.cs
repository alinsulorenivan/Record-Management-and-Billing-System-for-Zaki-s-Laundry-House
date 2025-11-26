using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucOrderHistory : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        private string previousForm;

        public ucOrderHistory(dashboardAdmin form, Form popupForm, string cameFrom)
        {
            InitializeComponent();
            mainForm = form;
            popup = popupForm;
            previousForm = cameFrom;
            DataGridViewStyler.ApplyNonSelectableStyle(dtgOrderHistory);
        }

        public void LoadOrderHistory(string customerCode, string customerName)
        {
            string connectionString = DbConnection.ConnectionString;
            string query = @"
                SELECT 
                    o.OrderCode AS 'Order ID', 
                    o.OrderDate AS 'Order Date', 
                    o.Status, 
                    o.TotalAmount AS 'Total Amount',
                    GROUP_CONCAT(i.ItemName || ' (x' || od.Quantity || ')', ', ') AS 'Items Summary'
                FROM Orders o
                INNER JOIN Customers c ON o.CustomerID = c.CustomerID
                INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
                INNER JOIN Items i ON od.ItemID = i.ItemID
                WHERE c.CustomerCode = @CustomerCode
                GROUP BY o.OrderCode, o.OrderDate, o.Status, o.TotalAmount
                ORDER BY o.OrderDate DESC;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerCode", customerCode);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dtgOrderHistory.DataSource = dt;
                dtgOrderHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                if (dtgOrderHistory.Columns.Contains("Items Summary"))
                {
                    var col = dtgOrderHistory.Columns["Items Summary"];
                    col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                dtgOrderHistory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                lblCustomerInfo.Text = $"Order History for: {customerName}";
                SetupDataGridView();
            }
        }

        private void SetupDataGridView()
        {
            dtgOrderHistory.AllowUserToAddRows = false;
            dtgOrderHistory.ReadOnly = true;
            dtgOrderHistory.RowHeadersVisible = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (popup != null && previousForm == "customers")
            {
                popup.Close();
            }
        }

        private void dtgOrderHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
