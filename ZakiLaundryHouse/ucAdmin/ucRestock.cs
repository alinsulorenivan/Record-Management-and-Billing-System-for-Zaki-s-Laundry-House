using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucRestock : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        private string previousForm;
        private string _itemId; // store item ID
        private int _currentStock;
        private int _originalLowStockLevel; // 🔹 Add this at the top with your other fields

        public ucRestock(dashboardAdmin form, Form popupForm, string cameFrom)
        {
            InitializeComponent();
            mainForm = form;
            popup = popupForm;
            previousForm = cameFrom;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (popup != null && previousForm == "inventory")
            {
                popup.Close();
            }
        }

        public void LoadRestockDetails(string itemId, string itemName, string category, int stockAvailable)
        {
            _itemId = itemId;
            _currentStock = stockAvailable;

            lblItemInfo.Text = "Restocking Item";
            tbxItemname.Text = itemName;
            tbxCategory.Text = category;
            tbxAvailableStock.Text = stockAvailable.ToString();
            tbxRestock.Clear();

            // 🔹 Fetch and display the current Low Stock Threshold
            try
            {
                using (var conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT LowStockThreshold FROM Items WHERE ItemCode = @ItemCode";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemCode", _itemId);

                        object result = cmd.ExecuteScalar();
                        _originalLowStockLevel = result != null && result != DBNull.Value
                            ? Convert.ToInt32(result)
                            : 0;

                        tbxLowStockLevel.Text = _originalLowStockLevel.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading low stock threshold: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbxRestock_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar)) { e.Handled = true; return; }
            if (tb.Text.Length == 0 && e.KeyChar == '0') { e.Handled = true; return; }

            string newValue = tb.Text.Insert(tb.SelectionStart, e.KeyChar.ToString());
            if (int.TryParse(newValue, out int numericValue))
            {
                if (numericValue > 1000) e.Handled = true;
            }
            else e.Handled = true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int restockQty = 0;
            int newLowStockLevel = 0;

            bool restockChanged = !string.IsNullOrWhiteSpace(tbxRestock.Text) &&
                                  int.TryParse(tbxRestock.Text, out restockQty) && restockQty > 0;

            bool lowStockChanged = int.TryParse(tbxLowStockLevel.Text, out newLowStockLevel) &&
                                   newLowStockLevel != _originalLowStockLevel;

            if (lowStockChanged)
            {
                if (newLowStockLevel < 0)
                {
                    MessageBox.Show("Please enter a valid low stock level.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (newLowStockLevel < 10)
                {
                    MessageBox.Show("Low stock level cannot be less than 10.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbxLowStockLevel.Focus();
                    return;
                }
            }

            if (restockChanged)
            {
                if (restockQty <= 0)
                {
                    MessageBox.Show("Please enter a valid restock quantity greater than 0.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (restockQty < 20 || restockQty > 1000)
                {
                    MessageBox.Show("Restock quantity must be between 20 and 1000.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbxRestock.Focus();
                    return;
                }
            }

            if (!restockChanged && !lowStockChanged)
            {
                MessageBox.Show("No changes were made.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                popup?.Close();
                return;
            }

            int newStock = _currentStock + (restockChanged ? restockQty : 0);

            try
            {
                using (var conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    conn.Open();

                    string query = "UPDATE Items SET ";
                    bool addComma = false;

                    if (restockChanged)
                    {
                        query += "StockAvailable = @NewStock, LastRestock = @LastRestock";
                        addComma = true;
                    }

                    if (lowStockChanged)
                    {
                        if (addComma) query += ", ";
                        query += "LowStockThreshold = @LowStockLevel";
                    }

                    query += " WHERE ItemCode = @ItemID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        if (restockChanged)
                        {
                            cmd.Parameters.AddWithValue("@NewStock", newStock);
                            cmd.Parameters.AddWithValue("@LastRestock", DateTime.Now);
                        }

                        if (lowStockChanged)
                            cmd.Parameters.AddWithValue("@LowStockLevel", newLowStockLevel);

                        cmd.Parameters.AddWithValue("@ItemID", _itemId);

                        cmd.ExecuteNonQuery();
                    }
                }

                if (restockChanged && lowStockChanged)
                    MessageBox.Show("Stock and low stock level updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (restockChanged)
                    MessageBox.Show("Stock updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (lowStockChanged)
                    MessageBox.Show("Low stock level updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                popup.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbxLowStockLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (char.IsControl(e.KeyChar)) return;
            if (!char.IsDigit(e.KeyChar)) { e.Handled = true; return; }
            if (tb.Text.Length == 0 && e.KeyChar == '0' && _currentStock != 0) { e.Handled = true; return; }
            if (tb.Text.Length >= 2) { e.Handled = true; return; }
        }

        private void tbxItemname_MouseDown(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).Enabled = false;
            ((TextBox)sender).Enabled = true;
            this.ActiveControl = null;
        }

        private void tbxCategory_MouseDown(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).Enabled = false;
            ((TextBox)sender).Enabled = true;
            this.ActiveControl = null;
        }

        private void tbxAvailableStock_MouseDown(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).Enabled = false;
            ((TextBox)sender).Enabled = true;
            this.ActiveControl = null;
        }
    }
}
