using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucInventory : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        string selectedItemCode;
        int currentStock;
        private string _role;
        private bool isPopupOpen = false;
        public event Action ItemArchived;


        public ucInventory(dashboardAdmin form, Form popupForm, string role = "")
        {
            InitializeComponent();
            this.Resize += (s, e) =>
            {
                // Resize all columns except the custom "Action" column
                foreach (DataGridViewColumn col in dtgInventory.Columns)
                {
                    if (col.Name != "Action")
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Keep Action column at a fixed width
                if (dtgInventory.Columns.Contains("Action"))
                {
                    dtgInventory.Columns["Action"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dtgInventory.Columns["Action"].Width = 150; // adjust width as needed
                }

                dtgInventory.Refresh();
            };

            _role = role;
            mainForm = form;
            popup = popupForm;

            LoadItems();
            // CheckLowStockItems();

            dtgInventory.CellClick += dtgInventory_CellClick_MultiButton;
            dtgInventory.CellPainting += dtgInventory_CellPainting;

            DataGridViewStyler.ApplyNonSelectableStyle(dtgInventory);
        }


        public void LoadItems()
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();
                string query = @"
            SELECT 
                ItemCode AS [Item ID], 
                ItemName AS [Item Name], 
                Category, 
                IFNULL(StockAvailable, 0) AS [Available Stock],
                IFNULL(Unit, '-') AS [Unit], 
                LastRestock,
                LowStockThreshold AS [Low Stock Level]
            FROM Items
            WHERE Category NOT IN ('Full Service', 'Self Service', 'Extra Charges')
                AND ItemName NOT IN ('Fold', 'Service Charge')
                AND (IsArchived = 0 OR IsArchived IS NULL)";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Convert LastRestock column to DateTime
                if (!dt.Columns.Contains("Last Restock"))
                {
                    dt.Columns.Add("Last Restock", typeof(DateTime));
                }

                foreach (DataRow row in dt.Rows)
                {
                    if (DateTime.TryParse(row["LastRestock"].ToString(), out DateTime dtValue))
                    {
                        row["Last Restock"] = dtValue.Date; // keep only date
                    }
                    else
                    {
                        row["Last Restock"] = DBNull.Value; // show blank if null
                    }
                }

                dt.Columns.Remove("LastRestock"); // remove original column if needed
                dtgInventory.DataSource = dt;

                // Format Last Restock column to show date only
                if (dtgInventory.Columns.Contains("Last Restock"))
                {
                    dtgInventory.Columns["Last Restock"].DefaultCellStyle.Format = "yyyy-MM-dd";
                }
            }

            // Add Action button column if not exists
            if (!dtgInventory.Columns.Contains("Action"))
            {
                DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn
                {
                    Name = "Action",
                    HeaderText = "Action",
                    UseColumnTextForButtonValue = false,
                    FlatStyle = FlatStyle.Flat
                };
                dtgInventory.Columns.Add(btnColumn);
            }

            SetupDataGridView();

            // Hide Action button and AddNewItem button for staff
            if (!string.IsNullOrEmpty(_role) && _role.ToLower() == "staff")
            {
                dtgInventory.Columns["Action"].Visible = false;
                btnAddNewItem.Visible = false;
            }
        }


        private void SetupDataGridView()
        {
            dtgInventory.AllowUserToAddRows = false;
            dtgInventory.ReadOnly = true;
            dtgInventory.RowHeadersVisible = false;
            dtgInventory.AllowUserToResizeRows = false;
            dtgInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgInventory.DefaultCellStyle.SelectionBackColor = dtgInventory.DefaultCellStyle.BackColor;
            dtgInventory.DefaultCellStyle.SelectionForeColor = dtgInventory.DefaultCellStyle.ForeColor;
        }

        // 🎨 Draw Restock & Delete buttons
        private void dtgInventory_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == dtgInventory.Columns["Action"].Index && e.RowIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);

                int buttonWidth = 60;
                int buttonHeight = 24;
                int spacing = 6;
                int leftPadding = 8;
                int centerY = e.CellBounds.Top + (e.CellBounds.Height - buttonHeight) / 2; ;


                Rectangle restockButton = new Rectangle(e.CellBounds.Left + leftPadding, centerY, buttonWidth, buttonHeight);
                Rectangle deleteButton = new Rectangle(restockButton.Right + spacing, centerY, buttonWidth, buttonHeight);

                // 🧺 Restock button (Lavender)
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(230, 230, 250)))
                    e.Graphics.FillRectangle(brush, restockButton);
                e.Graphics.DrawRectangle(Pens.Gray, restockButton);
                TextRenderer.DrawText(e.Graphics, "Restock", e.CellStyle.Font, restockButton, Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                // 🗑 Delete button (Light red)
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 204, 204)))
                    e.Graphics.FillRectangle(brush, deleteButton);
                e.Graphics.DrawRectangle(Pens.Gray, deleteButton);
                TextRenderer.DrawText(e.Graphics, "Delete", e.CellStyle.Font, deleteButton, Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        // 🖱 Detect button clicks
        private void dtgInventory_CellClick_MultiButton(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dtgInventory.Columns["Action"].Index)
                return;

            Rectangle cellRect = dtgInventory.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            int buttonWidth = 70;
            int buttonHeight = 25;
            int spacing = 5;

            Rectangle restockButton = new Rectangle(cellRect.Left + 10, cellRect.Top + 5, buttonWidth, buttonHeight);
            Rectangle deleteButton = new Rectangle(restockButton.Right + spacing, cellRect.Top + 5, buttonWidth, buttonHeight);

            Point clickPoint = dtgInventory.PointToClient(Cursor.Position);

            if (restockButton.Contains(clickPoint))
            {
                // 🧺 Restock
                string itemId = dtgInventory.Rows[e.RowIndex].Cells["Item ID"].Value.ToString();
                string itemName = dtgInventory.Rows[e.RowIndex].Cells["Item Name"].Value.ToString();
                string category = dtgInventory.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                int stockAvailable = Convert.ToInt32(dtgInventory.Rows[e.RowIndex].Cells["Available Stock"].Value);

                RestockItem(itemId, itemName, category, stockAvailable);
            }
            else if (deleteButton.Contains(clickPoint))
            {
                string itemId = dtgInventory.Rows[e.RowIndex].Cells["Item ID"].Value.ToString();
                string itemName = dtgInventory.Rows[e.RowIndex].Cells["Item Name"].Value.ToString();

                DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to Delete '{itemName}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
);


                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                        {
                            conn.Open();

                            // ✅ 1. Copy item details to ArchivedItems table
                            string archiveQuery = @"
                INSERT INTO ArchivedItems (ItemCode, ItemName, Category, StockAvailable, Unit, LastRestock)
                SELECT ItemCode, ItemName, Category, StockAvailable, Unit, LastRestock
                FROM Items WHERE ItemCode = @ItemCode";

                            using (SQLiteCommand cmdArchive = new SQLiteCommand(archiveQuery, conn))
                            {
                                cmdArchive.Parameters.AddWithValue("@ItemCode", itemId);
                                cmdArchive.ExecuteNonQuery();
                            }

                            // ✅ 2. Instead of deleting, mark as archived
                            string updateQuery = "UPDATE Items SET IsArchived = 1 WHERE ItemCode = @ItemCode";
                            using (SQLiteCommand cmdUpdate = new SQLiteCommand(updateQuery, conn))
                            {
                                cmdUpdate.Parameters.AddWithValue("@ItemCode", itemId);
                                cmdUpdate.ExecuteNonQuery();
                            }

                            // 🧩 3. NEW: Also archive or delete pricing related to this item
                            string archivePricingQuery = @"
                                UPDATE Pricing
                                SET IsArchived = 1
                                WHERE ItemID = (
                                    SELECT ItemID FROM Items WHERE ItemCode = @ItemCode LIMIT 1
                                )";

                            using (SQLiteCommand cmdArchivePricing = new SQLiteCommand(archivePricingQuery, conn))
                            {
                                cmdArchivePricing.Parameters.AddWithValue("@ItemCode", itemId);
                                cmdArchivePricing.ExecuteNonQuery();
                            }

                            // (Optional) If you prefer to DELETE pricing instead of archiving:
                            // string deletePricingQuery = "DELETE FROM Pricing WHERE ItemID = (SELECT TOP 1 ItemID FROM Items WHERE ItemCode = @ItemCode)";
                            // using (SqlCommand cmdDeletePricing = new SqlCommand(deletePricingQuery, conn))
                            // {
                            //     cmdDeletePricing.Parameters.AddWithValue("@ItemCode", itemId);
                            //     cmdDeletePricing.ExecuteNonQuery();
                            // }
                        }

                        MessageBox.Show($"'{itemName}' has been deleted successfully, including its pricing record!",
     "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadItems(); // refresh grid
                                     // 🔔 Notify subscribers that an item was archived
                        ItemArchived?.Invoke();

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void ShowPopup(UserControl control, Form popupForm, Size size)
        {
            FormDimmer dimmer = new FormDimmer
            {
                Size = mainForm.Size,
                Location = mainForm.Location,
                Owner = mainForm
            };
            dimmer.Show();

            popupForm.FormBorderStyle = FormBorderStyle.None;
            popupForm.StartPosition = FormStartPosition.CenterParent;
            popupForm.Size = size;
            popupForm.BackColor = Color.White;
            popupForm.ShowInTaskbar = false;

            control.Dock = DockStyle.Fill;
            popupForm.Controls.Add(control);

            popupForm.ShowDialog();
            dimmer.Close();
        }

        private void RestockItem(string itemId, string itemName, string category, int stockAvailable)
        {
            try
            {
                Form popupForm = new Form();
                var restockUC = new ucRestock(mainForm, popupForm, "inventory");
                restockUC.LoadRestockDetails(itemId, itemName, category, stockAvailable);

                ShowPopup(restockUC, popupForm, new Size(467, 545));
                LoadItems();
                CheckLowStockItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening restock form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isPopupOpen = false;
            }
        }



        private void CbExit_Click(object sender, EventArgs e)
        {
            formLogin loginForm = new formLogin();
            loginForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
                parentForm.Close();
        }

        private void CbMax_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.WindowState = parentForm.WindowState == FormWindowState.Normal
                    ? FormWindowState.Maximized
                    : FormWindowState.Normal;
            }
        }

        private void CbMin_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
                parentForm.WindowState = FormWindowState.Minimized;
        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {

            FormDimmer dimmer = new FormDimmer();
            dimmer.Size = mainForm.Size;
            dimmer.Location = mainForm.Location;
            dimmer.Owner = mainForm;
            dimmer.Show();

            Form popupForm = new Form();
            popupForm.FormBorderStyle = FormBorderStyle.None;
            popupForm.StartPosition = FormStartPosition.CenterParent;
            popupForm.Size = new Size(427, 621);
            popupForm.BackColor = Color.White;
            popupForm.ShowInTaskbar = false;

            ucInventoryForm orderForm = new ucInventoryForm(mainForm, popupForm);
            orderForm.Dock = DockStyle.Fill;
            popupForm.Controls.Add(orderForm);

            popupForm.ShowDialog();
            dimmer.Close();
            LoadItems();
            CheckLowStockItems();
        }
        private void CheckLowStockItems()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT ItemName, StockAvailable
                FROM Items
                WHERE LowStockThreshold IS NOT NULL
                  AND StockAvailable <= LowStockThreshold
                  AND Category NOT IN ('Full Service', 'Self Service', 'Extra Charges')
                  AND ItemName NOT IN ('Fold', 'Service Charge')
                  AND (IsArchived = 0 OR IsArchived IS NULL)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder lowStockItems = new StringBuilder();
                        StringBuilder outOfStockItems = new StringBuilder();

                        while (reader.Read())
                        {
                            string itemName = reader["ItemName"].ToString();
                            int stock = Convert.ToInt32(reader["StockAvailable"]);

                            if (stock <= 0)
                                outOfStockItems.AppendLine($"- {itemName} (Out of stock)");
                            else
                                lowStockItems.AppendLine($"- {itemName} ({stock} left)");
                        }

                        if (outOfStockItems.Length > 0)
                        {
                            string message =
                                "The following item/s are out of stock:\n\n" +
                                outOfStockItems.ToString() +
                                "\nPlease restock.";

                            MessageBox.Show(
                                message,
                                "Out of Stock Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }
                        else if (lowStockItems.Length > 0)
                        {
                            string message =
                                "The following item/s are low on stock:\n\n" +
                                lowStockItems.ToString() +
                                "\nPlease restock them soon.";

                            MessageBox.Show(
                                message,
                                "Low Stock Warning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Something went wrong while checking stock levels:\n\n" + ex.Message,
                    "System Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private bool hasShownStockWarning = false;

        public void CheckLowStockItemsOnce()
        {
            if (hasShownStockWarning) return;

            CheckLowStockItems(); // your existing function
            hasShownStockWarning = true;
        }
    }
}
