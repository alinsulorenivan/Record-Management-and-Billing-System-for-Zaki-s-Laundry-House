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
using FontAwesome.Sharp;


namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucUsers : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        // Tracks which rows have password visible (key = row index)
        private Dictionary<int, bool> passwordVisibility = new Dictionary<int, bool>();


        public ucUsers(dashboardAdmin form, Form popupForm)
        {
            InitializeComponent();
            mainForm = form;
            popup = popupForm;
            LoadUsers();
            dtgUsers.CellClick += dtgUsers_CellClick;
            DataGridViewStyler.ApplySelectableStyle(dtgUsers);
            DataGridViewStyler.ApplyBlueSelectableStyle(dtgUsers);


        }
        private void LoadUsers()
        {
            string connectionString = DbConnection.ConnectionString;
            string query = @"SELECT 
        UserCode AS [UserID],
        Name,
        Role,
        Username,
        Password
        FROM Users";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dtgUsers.DataSource = dt;
                SetupDataGridView();
            }

            dtgUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgUsers.MultiSelect = false;
            dtgUsers.ReadOnly = true;
            dtgUsers.RowHeadersVisible = false;
            dtgUsers.AllowUserToAddRows = false;
            dtgUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgUsers.EnableHeadersVisualStyles = false;
            dtgUsers.ClearSelection();

            // Reset visibility tracking
            passwordVisibility.Clear();

            // Hook up drawing and clicking events
            dtgUsers.CellPainting -= dtgUsers_CellPainting_ShowEyeIcon;
            dtgUsers.CellPainting += dtgUsers_CellPainting_ShowEyeIcon;

            dtgUsers.CellClick -= dtgUsers_CellClick_TogglePassword;
            dtgUsers.CellClick += dtgUsers_CellClick_TogglePassword;
        }





        private void ShowAccountPopup(string mode, Dictionary<string, string> userData)
        {
            FormDimmer dimmer = new FormDimmer
            {
                Size = mainForm.Size,
                Location = mainForm.Location,
                Owner = mainForm
            };
            dimmer.Show();

            Form popupForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(774, 574),
                BackColor = Color.White,
                ShowInTaskbar = false
            };

            ucAddAccount addAccount = new ucAddAccount(mainForm, popupForm)
            {
                Dock = DockStyle.Fill,
                Mode = mode
            };

            if (mode == "edit" && userData != null)
            {
                addAccount.SetUserData(userData);
            }

            addAccount.OnUserSaved += () =>
            {
                LoadUsers();
                popupForm.Close();
                dimmer.Close();
            };
            popupForm.Controls.Add(addAccount);
            popupForm.ShowDialog(mainForm);
            dimmer.Close();
        }




        private void dtgUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dtgUsers.Rows[e.RowIndex].Selected = true;
                dtgUsers.Focus();
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dtgUsers.SelectedRows.Count == 0 || dtgUsers.SelectedRows[0].IsNewRow)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this user? " +
                "All activity logs related to this user will also be deleted.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string userCode = dtgUsers.SelectedRows[0].Cells["UserID"].Value.ToString();

                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    conn.Open();

                    using (SQLiteTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // STEP 1: Get numeric UserID
                            int numericUserId = 0;
                            using (SQLiteCommand cmdGetId = new SQLiteCommand(
                                "SELECT UserID FROM Users WHERE UserCode = @UserCode", conn, transaction))
                            {
                                cmdGetId.Parameters.AddWithValue("@UserCode", userCode);
                                object resultId = cmdGetId.ExecuteScalar();
                                if (resultId != null)
                                    numericUserId = Convert.ToInt32(resultId);
                            }

                            // STEP 2: Delete related logs using numeric ID
                            using (SQLiteCommand cmdLogs = new SQLiteCommand(
                                "DELETE FROM LogTrail WHERE UserID = @UserID", conn, transaction))
                            {
                                cmdLogs.Parameters.AddWithValue("@UserID", numericUserId);
                                cmdLogs.ExecuteNonQuery();
                            }

                            // STEP 3: Delete user using UserCode
                            using (SQLiteCommand cmdUser = new SQLiteCommand(
                                "DELETE FROM Users WHERE UserCode = @UserCode", conn, transaction))
                            {
                                cmdUser.Parameters.AddWithValue("@UserCode", userCode);
                                cmdUser.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("User and related logs deleted successfully.");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error deleting user: " + ex.Message);
                        }
                    }
                }

                LoadUsers();
                dtgUsers.ClearSelection();
            }
        }





        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtgUsers.SelectedRows.Count == 0 || dtgUsers.SelectedRows[0].IsNewRow)
            {
                MessageBox.Show("Please select a user to edit.");
                return;
            }
            DataGridViewRow selectedRow = dtgUsers.SelectedRows[0];
            var userData = new Dictionary<string, string>
            {
                 { "UserCode", selectedRow.Cells["UserID"].Value.ToString() },
                 { "Name", selectedRow.Cells["Name"].Value.ToString() },
                 { "Role", selectedRow.Cells["Role"].Value.ToString() },
                 { "Username", selectedRow.Cells["Username"].Value.ToString() },
                 { "Password", selectedRow.Cells["Password"].Value.ToString() }
            };
            ShowAccountPopup(mode: "edit", userData: userData);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ShowAccountPopup(mode: "create", userData: null);
           
        }

        private void btnLogTrail_Click(object sender, EventArgs e)
        {
            FormDimmer dimmer = new FormDimmer
            {
                Size = mainForm.Size,
                Location = mainForm.Location,
                Owner = mainForm
            };
            dimmer.Show();

            Form popupForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(774, 574),
                BackColor = Color.White,
                ShowInTaskbar = false
            };

            // ✅ FIX: make sure you pass the correct arguments
            ucLogTrail logtrail = new ucLogTrail(mainForm, popupForm)
            {
                Dock = DockStyle.Fill
            };

            popupForm.Controls.Add(logtrail);
            popupForm.ShowDialog(mainForm);

            dimmer.Close();
            this.BringToFront();
            this.Visible = true;

        }
        private void dtgUsers_CellPainting_ShowEyeIcon(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dtgUsers.Columns[e.ColumnIndex].Name == "Password")
            {
                e.PaintBackground(e.CellBounds, true);

                string password = dtgUsers.Rows[e.RowIndex].Cells["Password"].Value?.ToString() ?? "";
                bool isVisible = passwordVisibility.ContainsKey(e.RowIndex) && passwordVisibility[e.RowIndex];

                // Mask or show password text
                string displayText = isVisible ? password : new string('*', password.Length);
                TextRenderer.DrawText(
                    e.Graphics,
                    displayText,
                    e.CellStyle.Font,
                    new Point(e.CellBounds.X + 5, e.CellBounds.Y + 6),
                    e.CellStyle.ForeColor
                );

                // === Eye icon setup ===
                int iconSize = 20;
                Rectangle iconRect = new Rectangle(
                    e.CellBounds.Right - iconSize - 10,
                    e.CellBounds.Y + (e.CellBounds.Height - iconSize) / 2,
                    iconSize,
                    iconSize
                );

                // ✅ FIX: reversed logic corrected
                // When password is visible → show EYE (👁)
                // When password is hidden (**) → show EYE SLASH (🚫👁)
                IconChar iconChar = isVisible ? IconChar.Eye : IconChar.EyeSlash;

                using (Bitmap bmp = new Bitmap(iconSize, iconSize))
                using (IconPictureBox icon = new IconPictureBox())
                {
                    icon.IconChar = iconChar;
                    icon.IconColor = Color.Black;
                    icon.IconFont = IconFont.Auto;
                    icon.BackColor = Color.Transparent;
                    icon.Size = new Size(iconSize, iconSize);

                    icon.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.MakeTransparent();
                    e.Graphics.DrawImage(bmp, iconRect.Location);
                }

                e.Handled = true;
            }
        }






        private void dtgUsers_CellClick_TogglePassword(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dtgUsers.Columns[e.ColumnIndex].Name == "Password")
            {
                Rectangle cellBounds = dtgUsers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int iconSize = 16;
                Rectangle iconRect = new Rectangle(
                    cellBounds.Right - iconSize - 10,
                    cellBounds.Y + (cellBounds.Height - iconSize) / 2,
                    iconSize, iconSize);

                Point mousePos = dtgUsers.PointToClient(Cursor.Position);

                // Check if the user clicked the eye icon
                if (iconRect.Contains(mousePos))
                {
                    bool currentVisible = passwordVisibility.ContainsKey(e.RowIndex) && passwordVisibility[e.RowIndex];
                    passwordVisibility[e.RowIndex] = !currentVisible;

                    // Redraw only the affected cell
                    dtgUsers.InvalidateCell(e.ColumnIndex, e.RowIndex);
                }
            }
        }
        private void SetupDataGridView()
        {
            dtgUsers.AllowUserToAddRows = false;
            dtgUsers.ReadOnly = true;
            dtgUsers.RowHeadersVisible = false;
            dtgUsers.AllowUserToResizeRows = false;
            dtgUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgUsers.MultiSelect = false;
            dtgUsers.EnableHeadersVisualStyles = false;

            // ✅ Remove highlight color when selecting rows
            dtgUsers.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            dtgUsers.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;


            // (Optional) Remove highlight color from column headers
            dtgUsers.ColumnHeadersDefaultCellStyle.SelectionBackColor = dtgUsers.ColumnHeadersDefaultCellStyle.BackColor;
            dtgUsers.ColumnHeadersDefaultCellStyle.SelectionForeColor = dtgUsers.ColumnHeadersDefaultCellStyle.ForeColor;
        }
        private bool ValidatePassword(string password)
        {
            // Length validation
            if (password.Length < 6 || password.Length > 10)
            {
                MessageBox.Show("Password must be between 6 and 10 characters long.",
                                "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Must contain at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                MessageBox.Show("Password must contain at least one uppercase letter.",
                                "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true; // ✅ Passed all checks
        }

    }
}