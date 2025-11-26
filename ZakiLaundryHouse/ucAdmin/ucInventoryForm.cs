using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucInventoryForm : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;

        public ucInventoryForm(dashboardAdmin form, Form popupForm)
        {
            InitializeComponent();
            mainForm = form;
            popup = popupForm;
        }

        public ucInventoryForm(dashboardAdmin form)
        {
            InitializeComponent();
            mainForm = form;
            popup = null;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (popup != null)
            {
                popup.Close();
            }
            else
            {
                mainForm.addUserControl(new ucInventory(mainForm, null));
            }
        }

        private void ucInventoryForm_Load(object sender, EventArgs e)
        {
            cbxCategory.Items.Clear();
            cbxCategory.SelectedIndex = -1;
            LoadCategoriesFromDatabase();
            GenerateNextItemCode(); // ✅ Show next available code when form loads
        }

        private void LoadCategoriesFromDatabase()
        {
            cbxCategory.Items.Clear();

            string query = @"
        SELECT DISTINCT Category 
        FROM Items
        WHERE Category NOT IN ('Full Service', 'Self Service')
        ORDER BY Category ASC;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    conn.Open();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbxCategory.Items.Add(reader["Category"].ToString());
                        }
                    }
                }

                cbxCategory.SelectedIndex = -1;

                if (cbxCategory.Items.Count == 0)
                {
                    MessageBox.Show("No categories found in the database (excluding Full Service and Self Service).",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories:\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ PREVIEW next available ItemCode (just for display)
        private void GenerateNextItemCode()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    conn.Open();

                    // Get the last inserted rowid from sqlite_sequence table
                    string query = @"
                SELECT 'ITEM-' || printf('%04d', IFNULL(seq, 0) + 1) AS NextItemCode
                FROM sqlite_sequence
                WHERE name='Items';";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            tbxItemID.Text = result.ToString();
                        }
                        else
                        {
                            // If no entries yet, start from ITEM-0001
                            tbxItemID.Text = "ITEM-0001";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting next ItemCode:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dateLastRestock_ValueChanged(object sender, EventArgs e) { }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e) { }

        private void tbxUnit_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxItemID_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRestock_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxItemName.Text) &&
                cbxCategory.SelectedIndex == -1 &&
                string.IsNullOrWhiteSpace(tbxAvailableStock.Text) &&
                string.IsNullOrWhiteSpace(tbxLowStockLevel.Text) &&
                string.IsNullOrWhiteSpace(tbxUnit.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxItemName.Text))
            {
                MessageBox.Show("Please enter an Item Name.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbxCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Category.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(tbxAvailableStock.Text))
            {
                MessageBox.Show("Please enter Available Stock.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(tbxLowStockLevel.Text))
            {
                MessageBox.Show("Please enter a Low Stock Alert Level.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(tbxUnit.Text))
            {
                MessageBox.Show("Please enter a Unit (e.g., pcs, bottle).", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int availableStock = 0;
            if (!int.TryParse(tbxAvailableStock.Text, out availableStock))
            {
                MessageBox.Show("Available Stock must be a valid number.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int lowStockLevel = 0;
            if (!int.TryParse(tbxLowStockLevel.Text, out lowStockLevel))
            {
                MessageBox.Show("Low Stock Alert Level must be a valid number.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (lowStockLevel < 10)
            {
                MessageBox.Show("Stock alert level cannot be less than 10.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxLowStockLevel.Focus();
                return;
            }

            if (availableStock < 20 || availableStock > 1000)
            {
                MessageBox.Show("Available Stock must be between 20 and 1000.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxAvailableStock.Focus();
                return;
            }

            if (lowStockLevel >= availableStock)
            {
                MessageBox.Show("Low Stock Level cannot be greater than or equal to Available Stock.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxLowStockLevel.Focus();
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();

                string checkQuery = @"
            SELECT COUNT(*)
            FROM Items
            WHERE ItemName = @ItemName 
              AND Category = @Category
              AND IsArchived = 0;";

                using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@ItemName", tbxItemName.Text.Trim());
                    checkCmd.Parameters.AddWithValue("@Category", cbxCategory.Text.Trim());

                    long exists = (long)checkCmd.ExecuteScalar(); // SQLite returns long for COUNT(*)

                    if (exists > 0)
                    {
                        MessageBox.Show(
                            "An item with the same name and category already exists and is not archived.\n" +
                            "Please adjust the item name or category.",
                            "Duplicate Item",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                }

                // Insert new item
                string insertQuery = @"
INSERT INTO Items (Category, ItemName, Unit, LastRestock, StockAvailable, LowStockThreshold)
VALUES (@Category, @ItemName, @Unit, CURRENT_TIMESTAMP, @StockAvailable, @LowStockThreshold);

SELECT 'ITEM-' || printf('%04d', last_insert_rowid());";

                using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Category", cbxCategory.Text);
                    cmd.Parameters.AddWithValue("@ItemName", tbxItemName.Text);
                    cmd.Parameters.AddWithValue("@Unit", tbxUnit.Text);
                    cmd.Parameters.AddWithValue("@StockAvailable", availableStock);
                    cmd.Parameters.AddWithValue("@LowStockThreshold", lowStockLevel);

                    try
                    {
                        object generatedCode = cmd.ExecuteScalar();

                        MessageBox.Show(
                            $"Item added successfully!\nGenerated Code: {generatedCode}",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        tbxItemName.Clear();
                        tbxUnit.Clear();
                        tbxAvailableStock.Clear();
                        tbxLowStockLevel.Clear();
                        cbxCategory.SelectedIndex = -1;

                        GenerateNextItemCode();

                        if (popup == null)
                        {
                            var inventoryUC = new ucInventory(mainForm, null);
                            mainForm.addUserControl(inventoryUC);
                            inventoryUC.LoadItems();
                        }
                        else
                        {
                            popup.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving item:\n" + ex.Message,
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void tbxLowStockLevel_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxItemName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxItemName_Leave(object sender, EventArgs e)
        {
            string input = tbxItemName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
                // Format capitalization (Title Case)
                System.Globalization.TextInfo textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                tbxItemName.Text = textInfo.ToTitleCase(input.ToLower());
            }
        }

        private void tbxItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxItemName.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void tbxUnit_Leave(object sender, EventArgs e)
        {
            string input = tbxUnit.Text;

            if (!string.IsNullOrWhiteSpace(input))
            {
                // Remove digits only (numbers)
                input = new string(input.Where(c => !char.IsDigit(c)).ToArray());

                // Remove leading and trailing spaces
                input = input.Trim();

                // Replace multiple spaces with a single space
                input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");

                tbxUnit.Text = input;
            }
        }

        private void tbxUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow letters, space, and control keys (like Backspace)
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Number is blocked
            }

            // Optional: Prevent space at the start
            if (tbxUnit.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void tbxAvailableStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow Backspace, Delete, etc.
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Prevent first character from being 0
            if (tb.Text.Length == 0 && e.KeyChar == '0')
            {
                e.Handled = true;
                return;
            }

            // Simulate what the text will be after the key press
            string newValue = tb.Text.Insert(tb.SelectionStart, e.KeyChar.ToString());

            // Check if value exceeds 1000
            if (int.TryParse(newValue, out int numericValue))
            {
                if (numericValue > 1000)
                {
                    e.Handled = true;
                    return;
                }
            }
            else
            {
                e.Handled = true;
            }
        }


        private void tbxLowStockLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow control keys (Backspace, Delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            // Prevent first character from being a 0 if the text is empty
            if (tb.Text.Length == 0 && e.KeyChar == '0')
            {
                e.Handled = true;
                return;
            }

            // Prevent input of multiple leading zeros (e.g., "0000")
            if (tb.Text == "0" && e.KeyChar == '0')
            {
                e.Handled = true;
                return;
            }
            if (tb.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }

            // Limit to 4 digits
            if (tb.Text.Length >= 2)
            {
                e.Handled = true;
            }
            if (tbxLowStockLevel.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void tbxAvailableStock_TextChanged(object sender, EventArgs e)
        {

        }


        //private void CheckLowStockLevel(int currentStock, string itemName, int lowStockLevel)
        //{
        //    if (currentStock <= lowStockLevel)
        //    {
        //        MessageBox.Show(

        //            $"Item: {itemName}\n" +
        //            $"Current Stock: {currentStock}\n" +
        //            $"Reorder Level: {lowStockLevel}\n\n" +
        //            $"Please restock soon to avoid running out of this item.",
        //            "Low Stock Warning",
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Warning
        //        );
        //    }
        //}

    }
}
