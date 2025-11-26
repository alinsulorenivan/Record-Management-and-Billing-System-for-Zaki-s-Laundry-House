//using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{

    public partial class ucPricing : UserControl
    {
        private Dictionary<string, int> savedColumnWidths = new Dictionary<string, int>();
        private dashboardAdmin mainForm;
        private Form popup;

        private bool isPanelExpanded = false;
        private double currentPanelHeight = 0.0;
        private readonly int panelMaxHeight = 155;
        private readonly int panelMinHeight = 0;
        private readonly double easingFactor = 0.15; // 0.05..0.30; lower = slower/smoother
        private string _role;

        private DataTable allPricingRows = new DataTable();
        private int currentPageIndex = 0;
        private int pageSize = 27;


        public ucPricing(dashboardAdmin form, Form popupForm, string role = "")
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
           | ControlStyles.OptimizedDoubleBuffer
           | ControlStyles.UserPaint, true);
            UpdateStyles();
            InitializeComponent();

            this.Resize += (s, e) =>
            {
                if (dtgPricing.Columns.Count == 0) return;

                // Resize all columns except the "Action" column
                foreach (DataGridViewColumn col in dtgPricing.Columns)
                {
                    if (col.Name != "Action")
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Keep Action column at a fixed width
                if (dtgPricing.Columns.Contains("Action"))
                {
                    dtgPricing.Columns["Action"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dtgPricing.Columns["Action"].Width = 140; // fixed width for buttons
                }

                // 🔹 Shrink "#" and give its space to "Item Name"
                var numCol = dtgPricing.Columns.Cast<DataGridViewColumn>()
                    .FirstOrDefault(c => c.HeaderText.Trim() == "#" ||
                                         c.Name.Equals("Number", StringComparison.OrdinalIgnoreCase));

                var nameCol = dtgPricing.Columns.Cast<DataGridViewColumn>()
                    .FirstOrDefault(c => c.Name.Equals("Name", StringComparison.OrdinalIgnoreCase) ||
                                         c.HeaderText.Trim().Equals("Item Name", StringComparison.OrdinalIgnoreCase));

                if (numCol != null && nameCol != null)
                {
                    int shrinkAmount = (int)(numCol.Width * 0.5); // shrink "#" by 50%

                    numCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    nameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                    numCol.Width = Math.Max(20, numCol.Width - shrinkAmount); // minimum readable size
                    nameCol.Width += shrinkAmount; // give extra space to Item Name
                }

                dtgPricing.Refresh();
            };



            _role = role;
            mainForm = form;
            popup = popupForm;
            LoadPricing();

            this.VisibleChanged += ucPricing_VisibleChanged;

            // wire original handlers
            dtgPricing.CellPainting += dtgPricing_CellPainting;
            dtgPricing.CellClick += dtgPricing_CellClick;
            dtgPricing.CellMouseDown += dtgPricing_CellMouseDown;

            DataGridViewStyler.ApplyNonSelectableStyle(dtgPricing);
            dtgPricing.BorderStyle = BorderStyle.FixedSingle;
            this.MouseUp += UcPricing_MouseUp;

            this.MouseUp += UcPricing_MouseUp;
        }

        public void ResetForm()
        {
            // Clear category and item
            cbxCategory.SelectedIndex = -1;
            cbxItem.DataSource = null;
            cbxItem.Text = string.Empty;

            // Clear min/max weight textboxes
            tbxMin.Clear();
            tbxMaxWeight.Clear();

            // Clear pricing type combobox
            cbxPricingType.SelectedIndex = -1;

            // Clear price textbox
            tbxPrice.Clear();

            // Collapse dropdown panel if expanded
            pnlDropdownPricing.Visible = false;
            pnlDropdownPricing.Height = panelMinHeight;
            isPanelExpanded = false;
            timerSlideDownPricing.Stop();
        }


        private void UcPricing_MouseUp(object sender, MouseEventArgs e)
        {
            // If panel is open and the click is OUTSIDE the dropdown
            if (!isPanelExpanded)
                return;

            Control clicked = GetChildAtPoint(e.Location);
            if (clicked != null && (clicked is TextBox || clicked is ComboBox))
                return; // avoid collapsing when interacting with inputs

            if (!pnlDropdownPricing.Bounds.Contains(this.PointToClient(Cursor.Position)))
                CollapseDropdown();
        }
        private void CollapseDropdown()
        {
            if (timerSlideDownPricing.Enabled) return;
            isPanelExpanded = true; // so animation runs in reverse
            timerSlideDownPricing.Start();
        }


        private void label6_Click(object sender, EventArgs e)
        {
        }

        public void LoadPricing(string category = "All")
        {
            try
            {
                dtgPricing.SuspendLayout();

                string connectionString = DbConnection.ConnectionString;
                string query = @"
SELECT 
    p.PriceID AS [#],
    i.Category AS [Category],
    i.ItemName AS [Item Name],
    IFNULL(p.PricingType, '-') AS [Pricing Type],
    IFNULL(CAST(p.MinWeight AS TEXT), '-') AS [Min Weight],
    IFNULL(CAST(p.MaxWeight AS TEXT), '-') AS [Max Weight],
    p.Price AS [Price],
    p.IsArchived AS [IsArchived]
FROM Pricing p
INNER JOIN Items i ON p.ItemID = i.ItemID
WHERE (@Category = 'All' OR i.Category = @Category)
    AND (i.IsArchived = 0 OR i.IsArchived IS NULL)
ORDER BY i.Category, i.ItemName;";

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Category", category);

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Create a new DataTable including category header rows
                    DataTable dtWithHeaders = dt.Clone();
                    dtWithHeaders.Columns["#"].DataType = typeof(string);

                    string lastCategory = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        string cat = row["Category"].ToString();

                        // Add category header row if new
                        if (cat != lastCategory)
                        {
                            DataRow headerRow = dtWithHeaders.NewRow();
                            headerRow["Category"] = cat;
                            headerRow["Item Name"] = "";
                            headerRow["Pricing Type"] = "";
                            headerRow["Min Weight"] = DBNull.Value;
                            headerRow["Max Weight"] = DBNull.Value;
                            headerRow["Price"] = DBNull.Value;
                            dtWithHeaders.Rows.Add(headerRow);

                            lastCategory = cat;
                        }

                        // Add actual item row
                        DataRow itemRow = dtWithHeaders.NewRow();
                        itemRow.ItemArray = row.ItemArray.Clone() as object[];
                        itemRow["Category"] = ""; // remove duplicate category
                        dtWithHeaders.Rows.Add(itemRow);
                    }

                    dtgPricing.DataSource = dtWithHeaders;

                    // Hide # column
                    if (dtgPricing.Columns.Contains("#"))
                        dtgPricing.Columns["#"].Visible = false;

                    if (dtgPricing.Columns.Contains("IsArchived"))
                        dtgPricing.Columns["IsArchived"].Visible = false;

                    if (dtgPricing.Columns.Contains("Price"))
                    {
                        dtgPricing.Columns["Price"].DefaultCellStyle.Format = "N2";
                    }

                    // Add Action column if not exists
                    if (!dtgPricing.Columns.Contains("Action"))
                    {
                        DataGridViewButtonColumn actionCol = new DataGridViewButtonColumn
                        {
                            Name = "Action",
                            HeaderText = "Action",
                            Text = "Edit/Archive",
                            Width = 140,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            UseColumnTextForButtonValue = false
                        };
                        dtgPricing.Columns.Add(actionCol);
                    }

                    // Apply your Styler
                    DataGridViewStyler.ApplyNonSelectableStyle(dtgPricing);
                    foreach (DataGridViewColumn col in dtgPricing.Columns)
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                    // Subscribe RowPrePaint for grayed category headers
                    dtgPricing.RowPrePaint -= DtgPricing_GrayedHeader_RowPrePaint;
                    dtgPricing.RowPrePaint += DtgPricing_GrayedHeader_RowPrePaint;

                    // Subscribe CellPainting for buttons
                    dtgPricing.CellPainting -= dtgPricing_CellPainting;
                    dtgPricing.CellPainting += dtgPricing_CellPainting;
                }

                dtgPricing.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading pricing data: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // RowPrePaint for grayed category header
        private void DtgPricing_GrayedHeader_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dtgPricing.Rows[e.RowIndex];
            if (row == null || row.IsNewRow) return;

            // Check if it's a category header row: Price is DBNull
            if (row.Cells["Price"].Value == DBNull.Value)
            {
                // ... (Header row painting logic remains the same)
                using (SolidBrush brush = new SolidBrush(Color.LightBlue))
                {
                    e.Graphics.FillRectangle(brush, e.RowBounds);
                }
                // ... (Draw category text)
                var cell = row.Cells["Category"];
                if (cell != null && cell.Value != null)
                {
                    string text = cell.Value.ToString();
                    Rectangle rect = row.DataGridView.GetCellDisplayRectangle(cell.ColumnIndex, e.RowIndex, true);
                    TextRenderer.DrawText(e.Graphics, text, new Font(dtgPricing.Font, FontStyle.Bold), rect, Color.Black, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                }

                e.Handled = true;
            }
            else
            {
                // --- New: Style for Archived Item Rows ---
                bool isArchived = false;
                object archivedValue = row.Cells["IsArchived"].Value;
                if (archivedValue != null && archivedValue != DBNull.Value)
                {
                    isArchived = Convert.ToBoolean(archivedValue);
                }

                if (isArchived)
                {
                    // Set the background color to light gray for archived items
                    row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
                else
                {
                    // Ensure non-archived items use the default back color
                    row.DefaultCellStyle.BackColor = dtgPricing.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = dtgPricing.DefaultCellStyle.ForeColor;
                }
            }
        }
        // Improved method to remove duplicate categories
        private void RemoveDuplicateCategories()
        {
            string lastCategory = "";
            foreach (DataGridViewRow row in dtgPricing.Rows)
            {
                if (row.IsNewRow) continue; // skip the new row placeholder

                string category = row.Cells["Category"].Value?.ToString();
                if (category == lastCategory)
                {
                    row.Cells["Category"].Value = ""; // blank duplicate
                }
                else
                {
                    lastCategory = category;
                }
            }
        }

        private void LoadCategories()
        {
            string connStr = DbConnection.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                string query = @"
                SELECT Category
                FROM Items
                GROUP BY Category
                ORDER BY
                    CASE 
                        WHEN Category = 'Full Service' THEN 1
                        WHEN Category = 'Self Service' THEN 2
                        ELSE 3
                    END, Category;";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cbxCategory.DataSource = dt;
                cbxCategory.DisplayMember = "Category";
                cbxCategory.ValueMember = "Category";
            }
        }

        // Replaced icon drawing with two button-like rectangles (Edit + Delete)
        private void dtgPricing_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dtgPricing.Columns["Action"].Index)
                return; // skip header row or other columns

            var row = dtgPricing.Rows[e.RowIndex];

            // Check if it's a category header row (Price is DBNull)
            if (row.Cells["Price"].Value == DBNull.Value)
            {
                // ... (Header row painting logic remains the same)
                using (SolidBrush brush = new SolidBrush(Color.LightGray))
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                e.Handled = true;
                return;
            }

            // --- New: Determine if the item is archived ---
            bool isArchived = false;
            object archivedValue = row.Cells["IsArchived"].Value;
            if (archivedValue != null && archivedValue != DBNull.Value)
            {
                isArchived = Convert.ToBoolean(archivedValue);
            }

            // Item rows: paint Edit/Disable or Edit/Enable buttons
            e.PaintBackground(e.CellBounds, true);

            int buttonWidth = 60;
            int buttonHeight = 24;
            int spacing = 6;
            int leftPadding = 8;
            int centerY = e.CellBounds.Top + (e.CellBounds.Height - buttonHeight) / 2;

            Rectangle editRect = new Rectangle(e.CellBounds.Left + leftPadding, centerY, buttonWidth, buttonHeight);
            Rectangle actionRect = new Rectangle(editRect.Right + spacing, centerY, buttonWidth + 5, buttonHeight); // Slightly wider for "Enable"

            // --- Paint Edit button (always present) ---
            Color editColor = Color.FromArgb(230, 230, 230);
            using (SolidBrush b = new SolidBrush(editColor))
                e.Graphics.FillRectangle(b, editRect);
            e.Graphics.DrawRectangle(Pens.Gray, editRect);
            TextRenderer.DrawText(e.Graphics, "Edit", e.CellStyle.Font, editRect, Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            // --- Paint Archive/Enable button ---
            Color actionColor;
            string actionText;

            if (isArchived)
            {
                // Green button for Enable
                actionColor = Color.FromArgb(204, 255, 204);
                actionText = "Enable";
            }
            else
            {
                // Light Red button for Archive (old "Delete")
                actionColor = Color.FromArgb(255, 204, 204);
                actionText = "Disable"; // Changed from "Delete"
            }

            using (SolidBrush b = new SolidBrush(actionColor))
                e.Graphics.FillRectangle(b, actionRect);
            e.Graphics.DrawRectangle(Pens.Gray, actionRect);
            TextRenderer.DrawText(e.Graphics, actionText, e.CellStyle.Font, actionRect, Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            e.Handled = true;
        }

        private void dtgPricing_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dtgPricing.Rows.Count) return;

            // Check for Category Header row (no actions allowed)
            if (dtgPricing.Rows[e.RowIndex].Cells["Price"].Value == DBNull.Value) return;

            if (e.ColumnIndex == dtgPricing.Columns["Action"].Index)
            {
                Rectangle cellRect = dtgPricing.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int buttonWidth = 60;
                int buttonHeight = 24;
                int spacing = 6;
                int leftPadding = 8;
                int centerY = cellRect.Top + (cellRect.Height - buttonHeight) / 2;

                Rectangle editRect = new Rectangle(cellRect.Left + leftPadding, centerY, buttonWidth, buttonHeight);
                Rectangle actionRect = new Rectangle(editRect.Right + spacing, centerY, buttonWidth + 5, buttonHeight);

                Point clickPoint = dtgPricing.PointToClient(Cursor.Position);
                string priceId = dtgPricing.Rows[e.RowIndex].Cells["#"].Value.ToString();
                int itemId = 0; // Will be set during the edit/action process
                // --- EDIT BUTTON ---
                if (editRect.Contains(clickPoint))
                {
                    string itemName = "", category = "", weight = "", price = "", pricingType = "";

                    using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                    {
                        string q = @"
                        SELECT i.ItemName, i.Category, p.MinWeight, p.MaxWeight, p.PricingType, p.Price
                        FROM Pricing p
                        INNER JOIN Items i ON p.ItemID = i.ItemID
                        WHERE p.PriceID = @PriceID";

                        using (SQLiteCommand cmd = new SQLiteCommand(q, conn))
                        {
                            cmd.Parameters.AddWithValue("@PriceID", priceId);
                            conn.Open();

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    itemName = reader["ItemName"].ToString();
                                    category = reader["Category"].ToString();

                                    string minWeight = reader["MinWeight"] != DBNull.Value ? reader["MinWeight"].ToString() : "";
                                    string maxWeight = reader["MaxWeight"] != DBNull.Value ? reader["MaxWeight"].ToString() : "";
                                    weight = (!string.IsNullOrEmpty(minWeight) || !string.IsNullOrEmpty(maxWeight))
                                                ? $"{minWeight} - {maxWeight} kg"
                                                : "";

                                    pricingType = reader["PricingType"] != DBNull.Value ? reader["PricingType"].ToString() : "";
                                    price = reader["Price"] != DBNull.Value ? reader["Price"].ToString() : "";
                                }
                            }
                        }
                    }

                    Form popupForm = new Form();
                    var editUC = new ucEditPricing(mainForm, popupForm, "ucPricing",
                                                   priceId, itemName, category, weight, pricingType, price);

                    // ✅ Subscribe to the event BEFORE showing popup
                    editUC.PricingUpdated += (s, ev) =>
                    {
                        LoadPricing();            // Reload updated data
                        dtgPricing.ClearSelection();
                        dtgPricing.Refresh();
                    };

                    // ✅ Removed the "this.Resize" handler that was resizing your dtg
                    ShowPopup(editUC, popupForm, new Size(774, 574));
                }

                // --- ARCHIVE / ENABLE BUTTON ---
                else if (actionRect.Contains(clickPoint))
                {
                    bool isArchived = Convert.ToBoolean(dtgPricing.Rows[e.RowIndex].Cells["IsArchived"].Value);

                    string actionText = isArchived ? "enable" : "archive (disable)";
                    string itemName = dtgPricing.Rows[e.RowIndex].Cells["Item Name"].Value.ToString();

                    DialogResult confirm = MessageBox.Show(
                        $"Are you sure you want to {actionText} the pricing for '{itemName}'?",
                        $"Confirm {actionText.ToUpper()}",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirm == DialogResult.Yes)
                    {
                        // Retrieve ItemID using a query, or modify LoadPricing to select it.
                        // For simplicity, we'll retrieve ItemID inside the UpdateItemArchiveStatus method.

                        UpdateItemArchiveStatus(priceId, !isArchived); // Pass the new archive status
                    }
                }
            }
        }
        private void UpdateItemArchiveStatus(string priceId, bool archiveStatus)
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();

                try
                {
                    string updateQuery = "UPDATE Pricing SET IsArchived = @IsArchived WHERE PriceID = @PriceID";
                    using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@IsArchived", archiveStatus);
                        cmd.Parameters.AddWithValue("@PriceID", priceId);
                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();

                    // Refresh DataGridView
                    LoadPricing();

                    string statusText = archiveStatus ? "archived (disabled)" : "enabled";
                    MessageBox.Show($"Pricing item successfully {statusText}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Error updating archive status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void SetupDataGridView()
        {
            dtgPricing.AllowUserToAddRows = false;
            dtgPricing.ReadOnly = true;
            dtgPricing.RowHeadersVisible = false;

            // ✅ Base autosize setup
            dtgPricing.AllowUserToResizeColumns = false;
            dtgPricing.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//!!!!!!!!!!!!

            dtgPricing.DefaultCellStyle.SelectionBackColor = dtgPricing.DefaultCellStyle.BackColor;
            dtgPricing.DefaultCellStyle.SelectionForeColor = dtgPricing.DefaultCellStyle.ForeColor;
            dtgPricing.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // 🔹 Resize all columns first
            //!!!!!!!dtgPricing.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // ✅ Compact “#” column
            if (dtgPricing.Columns.Contains("#"))
            {
                var col = dtgPricing.Columns["#"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // ✅ Auto-fit “Category”
            if (dtgPricing.Columns.Contains("Category"))
            {
                var col = dtgPricing.Columns["Category"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // ✅ Auto-fit “Min Weight”
            if (dtgPricing.Columns.Contains("Min Weight"))
            {
                var col = dtgPricing.Columns["Min Weight"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // ✅ Auto-fit “Max Weight”
            if (dtgPricing.Columns.Contains("Max Weight"))
            {
                var col = dtgPricing.Columns["Max Weight"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // ✅ Auto-fit “Pricing Type”
            if (dtgPricing.Columns.Contains("Pricing Type"))
            {
                var col = dtgPricing.Columns["Pricing Type"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // ✅ Auto-fit “Price”
            if (dtgPricing.Columns.Contains("Price"))
            {
                var col = dtgPricing.Columns["Price"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // ✅ “Name” column — wrap text
            if (dtgPricing.Columns.Contains("Name"))
            {
                var col = dtgPricing.Columns["Name"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            // ✅ “Action” column — auto-fit fully
            if (dtgPricing.Columns.Contains("Action"))
            {
                var col = dtgPricing.Columns["Action"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                col.MinimumWidth = 140; // ensures both Edit + Delete fit
            }

            // ✅ Separate “Edit” / “Delete” buttons (if they exist individually)
            if (dtgPricing.Columns.Contains("Edit"))
            {
                var col = dtgPricing.Columns["Edit"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dtgPricing.Columns.Contains("Delete"))
            {
                var col = dtgPricing.Columns["Delete"];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // ✅ Allow multi-line rows (for wrapped “Name”)
            dtgPricing.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            if (dtgPricing.Columns.Contains("Action"))
            {
                var col = dtgPricing.Columns["Action"];
                col.Width += 20; // small buffer for padding
            }

            // ✅ Refresh grid
            //dtgPricing.Refresh();
        }
        private void dtgPricing_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dtgPricing.Columns["Action"].Index)
            {
                dtgPricing.ClearSelection();
            }
        }

        private int GetOrCreateItemId(SQLiteConnection conn, SQLiteTransaction tran, string category, string itemName)
        {
            string checkItemQuery = "SELECT ItemID FROM Items WHERE ItemName = @ItemName AND Category = @Category";
            using (SQLiteCommand cmd = new SQLiteCommand(checkItemQuery, conn, tran))
            {
                cmd.Parameters.AddWithValue("@ItemName", itemName);
                cmd.Parameters.AddWithValue("@Category", category);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            // Insert new item if not exists
            string insertItem = @"
INSERT INTO Items (ItemName, Category) 
VALUES (@ItemName, @Category);
SELECT last_insert_rowid();";

            using (SQLiteCommand insertCmd = new SQLiteCommand(insertItem, conn, tran))
            {
                insertCmd.Parameters.AddWithValue("@ItemName", itemName);
                insertCmd.Parameters.AddWithValue("@Category", category);

                return Convert.ToInt32(insertCmd.ExecuteScalar());
            }
        }

        private void CbExit_Click(object sender, EventArgs e)
        {
            formLogin loginForm = new formLogin();
            loginForm.Show();
            Form parentForm = this.FindForm(); // find the parent form
            if (parentForm != null)
            {
                parentForm.Close(); // close the whole form instead of just the user control
            }
        }

        private void CbMax_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                if (parentForm.WindowState == FormWindowState.Normal)
                {
                    parentForm.WindowState = FormWindowState.Maximized;
                }
                else if (parentForm.WindowState == FormWindowState.Maximized)
                {
                    parentForm.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void CbMin_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.WindowState = FormWindowState.Minimized;
            }
        }

        private void tbxQtyWeight_TextChanged(object sender, EventArgs e)
        {

        }

        private void timerSlideDownPricing_Tick(object sender, EventArgs e)
        {
            pnlDropdownPricing.SuspendLayout();
            int step = 20; // pixels per tick, bigger = faster

            if (!isPanelExpanded)
            {
                pnlDropdownPricing.Height += step;
                if (pnlDropdownPricing.Height >= panelMaxHeight)
                {
                    pnlDropdownPricing.Height = panelMaxHeight;
                    timerSlideDownPricing.Stop();
                    isPanelExpanded = true;
                }
            }
            else
            {
                pnlDropdownPricing.Height -= step;
                if (pnlDropdownPricing.Height <= panelMinHeight)
                {
                    pnlDropdownPricing.Height = panelMinHeight;
                    timerSlideDownPricing.Stop();
                    isPanelExpanded = false;
                    pnlDropdownPricing.Visible = false; // hide when collapsed
                }
            }
            pnlDropdownPricing.ResumeLayout();

        }


        private void rdbFullServ_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFullServ.Checked)
            {
                LoadPricing("Full Service");
            }
        }

        private void rdbSelfServ_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSelfServ.Checked)
            {
                LoadPricing("Self Service");
            }
        }

        private void rdbDetergent_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAddOn.Checked)
            {
                LoadPricing("Add On");
            }
        }

        private void rdbOthers_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbOthers.Checked)
            {
                LoadPricing("Others");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAll.Checked)
            {
                LoadPricing("All"); // show all again
            }
        }


        private void btnFilterPricing_Click(object sender, EventArgs e)
        {

        }

        private void pnlDropdownPricing_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ucPricing_Load(object sender, EventArgs e)
        {
            // Start hidden
            pnlDropdownPricing.Visible = false;
            pnlDropdownPricing.Height = panelMinHeight;
            currentPanelHeight = panelMinHeight;

            timerSlideDownPricing.Interval = 15; // smooth

            LoadCategories();

            // Reset combo boxes and textboxes when the pricing form opens
            cbxCategory.SelectedIndex = -1;
            cbxItem.DataSource = null;
            cbxItem.Text = string.Empty;
            cbxPricingType.SelectedIndex = -1;
            tbxMin.Clear();
            tbxMaxWeight.Clear();
            tbxPrice.Clear();

            // Apply disabled/gray-out state
            UpdateWeightTextboxState();

            // When category changes, load items
            cbxCategory.SelectedIndexChanged += (s, ev) =>
            {
                if (cbxCategory.SelectedValue != null)
                {
                    string selectedCategory = cbxCategory.SelectedValue.ToString();
                    LoadItemsByCategory(selectedCategory);
                    UpdateWeightTextboxState();
                }
                else
                {
                    // Clear items when no category selected
                    cbxItem.DataSource = null;
                    cbxItem.Text = string.Empty;
                }
            };

            // Radio button wiring
            rdbFullServ.CheckedChanged += rdbFullServ_CheckedChanged;
            rdbSelfServ.CheckedChanged += rdbSelfServ_CheckedChanged;
            rdbAddOn.CheckedChanged += rdbDetergent_CheckedChanged;
            rdbOthers.CheckedChanged += rdbOthers_CheckedChanged;

            // Role handling
            if (_role.ToLower() == "staff")
            {
                panelPricingForm.Visible = false;
                dtgPricing.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                dtgPricing.Left = 0;
                dtgPricing.Width = this.ClientSize.Width - 10;
                dtgPricing.Height = this.ClientSize.Height - dtgPricing.Top - 10;
                if (dtgPricing.Columns.Contains("Action"))
                    dtgPricing.Columns["Action"].Visible = false;
            }
            SetupDataGridView();
            cbxCategory.SelectedIndexChanged += (s, ev) => UpdateWeightTextboxState();
            cbxPricingType.SelectedIndexChanged += (s, ev) => UpdateWeightTextboxState();

        }

        private void iconButton1_Click(object sender, EventArgs e)//btnFilterPricing
        {
            // prevent double-start while animating
            if (timerSlideDownPricing.Enabled)
                return;

            // If we're about to expand, make sure panel is shown first
            if (!isPanelExpanded)
            {
                pnlDropdownPricing.Visible = true;
                // keep currentPanelHeight in sync (in case you changed Height elsewhere)
                currentPanelHeight = pnlDropdownPricing.Height;
            }

            timerSlideDownPricing.Start();
        }

        private void LoadItemsByCategory(string category)
        {
            string connStr = DbConnection.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                string query = @"
                    SELECT ItemID, ItemName 
                    FROM Items 
                    WHERE Category = @Category 
                      AND (IsArchived = 0 OR IsArchived IS NULL)";

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@Category", category);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cbxItem.DataSource = dt;
                cbxItem.DisplayMember = "ItemName";
                cbxItem.ValueMember = "ItemName";

                // allow typing new items even if not in list
                cbxItem.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        private void UpdateWeightTextboxState()
        {
            string selectedCategory = cbxCategory.Text.Trim();
            string selectedPricingType = cbxPricingType.Text.Trim();

            //
            // ✅ Make cbxItem untypeable (DropDownList) for "Add On" and "Others"
            //
            bool isAddonOrOthers = selectedCategory.Equals("Add On", StringComparison.OrdinalIgnoreCase) ||
                                   selectedCategory.Equals("Others", StringComparison.OrdinalIgnoreCase);

            cbxItem.DropDownStyle = isAddonOrOthers ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;

            //
            // ✅ Enable PricingType for all categories
            //
            cbxPricingType.Enabled = true;
            cbxPricingType.BackColor = Color.White;
            cbxPricingType.ForeColor = Color.Black;

            //
            // ✅ Weight textbox logic (enabled if "Per Minimum" for ANY category)
            //
            bool allowWeight = selectedPricingType == "Per Minimum";
            tbxMin.Enabled = allowWeight;
            tbxMaxWeight.Enabled = allowWeight;

            tbxMin.BackColor = allowWeight ? Color.White : Color.LightGray;
            tbxMaxWeight.BackColor = allowWeight ? Color.White : Color.LightGray;

            if (!allowWeight)
            {
                tbxMin.Clear();
                tbxMaxWeight.Clear();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void cbxPricingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWeightTextboxState();
        }

        private void cbxPricingType_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }


        private void ucPricing_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                pnlDropdownPricing.Visible = false; // Hide filter panel automatically
                pnlDropdownPricing.Height = 0;
                isPanelExpanded = false; // ✅ Reset expansion state
                timerSlideDownPricing.Stop(); // optional, in case the animation was mid-run
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string itemName = cbxItem.Text.Trim();
            string category = cbxCategory.SelectedValue?.ToString();
            string minWeightText = tbxMin.Text.Trim();
            string maxWeightText = tbxMaxWeight.Text.Trim();
            string pricingType = cbxPricingType.Text.Trim();
            string priceText = tbxPrice.Text.Trim();

            // Validate required fields
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Please complete all required fields.", "Missing Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate price
            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Price must be a valid number.", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (price <= 0)
            {
                MessageBox.Show("Please enter a valid amount greater than 0.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate min/max weight
            decimal? minWeight = null;
            decimal? maxWeight = null;

            if (!string.IsNullOrEmpty(minWeightText))
            {
                if (!decimal.TryParse(minWeightText, out decimal min))
                {
                    MessageBox.Show("Minimum weight must be a valid number.", "Invalid Minimum Weight", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                minWeight = min;
            }

            if (!string.IsNullOrEmpty(maxWeightText))
            {
                if (!decimal.TryParse(maxWeightText, out decimal max))
                {
                    MessageBox.Show("Maximum weight must be a valid number.", "Invalid Maximum Weight", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                maxWeight = max;
            }

            if (minWeight.HasValue && maxWeight.HasValue && minWeight.Value > maxWeight.Value)
            {
                MessageBox.Show("Minimum weight cannot be greater than Maximum weight.", "Weight Range Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();

                try
                {
                    int itemId = GetOrCreateItemId(conn, tran, category, itemName);

                    // ===== Duplicate pricing check =====
                    string checkQuery = @"
                    SELECT COUNT(*) 
                    FROM Pricing 
                    WHERE ItemID = @ItemID 
                    AND IFNULL(PricingType, '') = IFNULL(@PricingType, '')";

                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn, tran))
                    {
                        checkCmd.Parameters.AddWithValue("@ItemID", itemId);
                        checkCmd.Parameters.AddWithValue("@PricingType", string.IsNullOrEmpty(pricingType) ? (object)DBNull.Value : pricingType);

                        long count = (long)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Pricing for this item already exists.", "Duplicate Pricing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tran.Rollback();
                            return;
                        }
                    }
                    // ===== End of duplicate check =====

                    // Insert new pricing
                    string insertPricing = @"
                    INSERT INTO Pricing (ItemID, PricingType, MinWeight, MaxWeight, Price)
                    VALUES (@ItemID, @PricingType, @MinWeight, @MaxWeight, @Price)";

                    using (SQLiteCommand cmd = new SQLiteCommand(insertPricing, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@ItemID", itemId);
                        cmd.Parameters.AddWithValue("@PricingType", string.IsNullOrEmpty(pricingType) ? (object)DBNull.Value : pricingType);
                        cmd.Parameters.AddWithValue("@MinWeight", minWeight.HasValue ? (object)minWeight.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaxWeight", maxWeight.HasValue ? (object)maxWeight.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Price", price);

                        cmd.ExecuteNonQuery();
                    }

                    tran.Commit();

                    LoadPricing();

                    MessageBox.Show("Pricing saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields
                    cbxItem.Text = "";
                    tbxPrice.Clear();
                    tbxMin.Clear();
                    tbxMaxWeight.Clear();
                    cbxPricingType.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Error saving pricing: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private bool _isUpdatingPriceText = false;

        private void tbxPrice_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingPriceText) return;
            _isUpdatingPriceText = true;

            TextBox tbx = sender as TextBox;
            string text = tbx.Text;

            // === Remove existing ".00" if present ===
            if (text.EndsWith(".00"))
                text = text.Substring(0, text.Length - 3);

            // === Keep only digits and minus sign ===
            text = new string(text.Where(c => char.IsDigit(c) || c == '-').ToArray());

            // === Ensure only one '-' and only at the start ===
            bool isNegative = false;
            if (text.Contains('-'))
            {
                text = text.Replace("-", "");
                isNegative = true;
            }

            // === Remove leading zeros (but keep one if all zeros) ===
            text = text.TrimStart('0');
            if (string.IsNullOrEmpty(text))
                text = "0";

            // === Reapply negative sign ===
            if (isNegative)
                text = "-" + text;

            // === Format final value ===
            tbx.Text = text + ".00";
            tbx.SelectionStart = tbx.Text.Length - 3;
            tbx.SelectionLength = 0;

            _isUpdatingPriceText = false;
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Clear textboxes
            tbxMin.Clear();
            tbxMaxWeight.Clear();
            tbxPrice.Clear();

            // Reset comboboxes
            cbxCategory.SelectedIndex = -1;
            cbxItem.SelectedIndex = -1;
            cbxPricingType.SelectedIndex = -1;
        }

        private void dtgPricing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbxItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tbx = sender as TextBox;

            // Disallow '.' (since we auto-append .00)
            if (e.KeyChar == '.')
            {
                e.Handled = true;
                return;
            }

            // Allow control keys (Backspace, Delete)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow digits
            if (char.IsDigit(e.KeyChar))
                return;

            // Handle '-' toggle
            if (e.KeyChar == '-')
            {
                e.Handled = true;

                // Toggle negative sign at start
                if (tbx.Text.StartsWith("-"))
                    tbx.Text = tbx.Text.Substring(1);
                else
                    tbx.Text = "-" + tbx.Text;

                // Keep caret before .00
                tbx.SelectionStart = tbx.Text.Length - 3;
                tbx.SelectionLength = 0;
                return;
            }

            // Disallow everything else
            e.Handled = true;
        }

        private void tbxMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow control keys (Backspace, Delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits and one decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && tb.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            // Prevent more than 2 digits before decimal if no decimal exists
            if (!tb.Text.Contains("."))
            {
                // Get current selection start
                if (tb.Text.Length >= 2 && e.KeyChar != '.' && tb.SelectionStart > tb.Text.Length - 1)
                {
                    e.Handled = true;
                    return;
                }
                else if (tb.Text.Length >= 2 && e.KeyChar != '.' && !tb.SelectedText.Any())
                {
                    e.Handled = true;
                    return;
                }
            }

            // Prevent more than 2 decimal places
            if (tb.Text.Contains("."))
            {
                int index = tb.Text.IndexOf(".");
                int decimals = tb.Text.Length - index - 1;

                // If cursor is after decimal and already 2 decimals typed, block more
                if (tb.SelectionStart > index && decimals >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void tbxMin_TextChanged(object sender, EventArgs e)
        {
            //var textBox = (TextBox)sender;
            //if (decimal.TryParse(textBox.Text, out decimal value))
            //{
            //    // If the value has more than two decimal places, round it
            //    textBox.Text = value.ToString("0.00");
            //    textBox.SelectionStart = textBox.Text.Length; // Maintain the cursor at the end
            //}
        }

        private void tbxMin_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(tbxMin.Text, out decimal value))
            {
                // Format to 2 decimal places on leave (always show .00)
                tbxMin.Text = value.ToString("0.00");
            }
            else
            {
                tbxMin.Text = ""; // Clear invalid input
            }
        }

        private void tbxMaxWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow control keys (Backspace, Delete, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Allow only digits and one decimal point
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && tb.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            // Prevent more than 2 digits before decimal if no decimal exists
            if (!tb.Text.Contains("."))
            {
                if (tb.Text.Length >= 2 && e.KeyChar != '.' && tb.SelectionLength == 0)
                {
                    e.Handled = true;
                    return;
                }
            }

            // Prevent more than 2 decimal places
            if (tb.Text.Contains("."))
            {
                int index = tb.Text.IndexOf(".");
                int decimals = tb.Text.Length - index - 1;

                if (tb.SelectionStart > index && decimals >= 2)
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void tbxMaxWeight_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(tbxMaxWeight.Text, out decimal value))
            {
                tbxMaxWeight.Text = value.ToString("0.00");
            }
            else
            {
                tbxMaxWeight.Text = "";
            }
        }

        private void cbxItem_TextChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            if (!string.IsNullOrEmpty(cb.Text))
            {
                int selectionStart = cb.SelectionStart;
                int selectionLength = cb.SelectionLength;

                // Capitalize the first letter and keep the rest unchanged
                string text = cb.Text;
                string capitalized = char.ToUpper(text[0]) + text.Substring(1);

                if (cb.Text != capitalized)
                {
                    cb.Text = capitalized;
                    cb.SelectionStart = selectionStart;   // Preserve cursor position
                    cb.SelectionLength = selectionLength; // Preserve selected text
                }
            }
        }

        private void cbxItem_Leave(object sender, EventArgs e)
        {
            string input = cbxItem.Text.Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
                // Format capitalization (Title Case)
                System.Globalization.TextInfo textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                cbxItem.Text = textInfo.ToTitleCase(input.ToLower());
            }
        }

        private void cbxItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbxItem.SelectionStart == 0 && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }
    }
}
