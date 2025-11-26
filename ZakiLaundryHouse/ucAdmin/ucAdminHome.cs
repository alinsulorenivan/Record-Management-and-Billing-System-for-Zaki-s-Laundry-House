using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucAdminHome : UserControl
    {
        private int UserID;
        private string userRole;
        private bool IsEnabled;


        bool isPanelExpanded = false;
        int panelMaxWidth = 114;   // Full expanded width
        int panelMinWidth = 0;     // Collapsed width
        int slideSpeed = 10;


        bool isPanelExpandeder = false;
        int panelMaxHeight = 40;
        int panelMinHeight = 0;
        int slideSpeeder = 10;

        bool isPanelExpandedStaff = false;
        int panelMaxWidthStaff = 114;   // Full expanded width
        int panelMinWidthStaff = 0;     // Collapsed width
        int slideSpeedStaff = 10;

        bool isPanelExpandedDownStaff = false;
        int panelMaxHeightStaff = 40;
        int panelMinHeightStaff = 0;
        int slideSpeederStaff = 10;

        //labeltext
        private string labeltext_1 = "";
        private string labeltext_2 = "";
        private string labeltext_3 = "";
        private string labeltext_4 = "";
        private string labeltext_5 = "";

        // Store reference position when opening
        Point anchorPoint;

        //private readonly FlowLayoutPanel flow = new FlowLayoutPanel();
        private readonly FlowLayoutPanel flow = new DoubleBufferedFlowLayoutPanel();
        private readonly List<Panel> tiles = new List<Panel>();

        // --- Tile sizing (adjust as needed) ---
        private const int TileW = 220;
        private const int TileH = 140;
        private const int Gap = 15;


        private Image backgroundLogo;

        public ucAdminHome(int userID, string role)
        {
            UserID = userID;
            userRole = role; // ✅ assign role first
            InitializeComponent();
            LoadTodayStats();

            this.Resize += MainForm_Resize;
            this.VisibleChanged += ucAdmin_VisibleChanged;
            this.VisibleChanged += ucStaff_VisibleChanged;
            this.DoubleBuffered = true;

            backgroundLogo = Properties.Resources.Zaki;

            // --- Flow setup ---
            flow.Dock = DockStyle.Fill;
            flow.WrapContents = true;
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.BackColor = Color.Transparent;
            flow.BorderStyle = BorderStyle.None;

            // ✅ Staff and Admin have different paddings
            if (userRole == "admin")
                flow.Padding = new Padding(30, 40, 30, 40);
            else if (userRole == "staff")
                flow.Padding = new Padding(0); // Remove outer padding for clean centering

            // ✅ Center tiles when resized
            flow.Resize += (s, e) =>
            {
                if (!flow.IsHandleCreated || flow.Controls.Count == 0) return;
                flow.SuspendLayout();
                CenterFlowContent(flow);
                flow.ResumeLayout();
            };


            // --- Container setup ---
            Panel container = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None
            };

            Panel spacer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 20,
                BackColor = Color.Transparent
            };

            Panel logoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = Color.Transparent
            };

            PictureBox logoBox = new PictureBox
            {
                Image = backgroundLogo,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(900, 200),
                Location = new Point((logoPanel.Width - 900) / 2, 5),
                Anchor = AnchorStyles.Top
            };
            logoPanel.Controls.Add(logoBox);
            logoPanel.Resize += (s, e) => { logoBox.Left = (logoPanel.Width - logoBox.Width) / 2; };

            container.Controls.Add(flow);
            container.Controls.Add(logoPanel);
            container.Controls.Add(spacer);
            this.Controls.Add(container);

            // ✅ Create tiles depending on user role
            tiles.Clear(); // always clear previous ones

            if (userRole == "admin")
            {
                tiles.Add(CreateTile(labeltext_1, Properties.Resources.Sales));            // Total Expected Sales
                tiles.Add(CreateTile(labeltext_2, Properties.Resources.Completed));       // Completed Orders
                tiles.Add(CreateTile(labeltext_3, Properties.Resources.Pending));         // Pending Orders
                tiles.Add(CreateTile(labeltext_4, Properties.Resources.TOTALCUSTOMERS));  // Total Customers
                tiles.Add(CreateTile(labeltext_5, Properties.Resources.TotalOrderss));    // Total Orders Today
            }
            else if (userRole == "staff")
            {
                // ✅ Staff does not see Expected Sales
                tiles.Add(CreateTile(labeltext_2, Properties.Resources.Completed));       // Completed Orders
                tiles.Add(CreateTile(labeltext_3, Properties.Resources.Pending));         // Pending Orders
                tiles.Add(CreateTile(labeltext_4, Properties.Resources.TOTALCUSTOMERS));  // Total Customers
                tiles.Add(CreateTile(labeltext_5, Properties.Resources.TotalOrderss));    // Total Orders Today
            }

            // ✅ Add tiles to the layout
            foreach (var t in tiles)
                flow.Controls.Add(t);

            // ✅ Load data after tiles exist
            LoadTodayStats();

            // Popup setup
            panelPopup.Visible = false;
            panelPopup.Width = 0;
            timerSlide.Interval = 10;

            panelPopupStaff.Visible = false;
            panelPopupStaff.Width = 0;
            timerSlideStaff.Interval = 10;

            panelPopupDown.Visible = false;
            panelPopupDown.Height = 0;
            timerSlideDown.Interval = 10;

            panelPopupDownStaff.Visible = false;
            panelPopupDownStaff.Height = 0;
            timerSlideDownStaff.Interval = 10;

            this.Resize += Form1_Resize;
            this.Resize += Form2_Resize;
        }

        public class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                this.DoubleBuffered = true;
                this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.ResizeRedraw |
                              ControlStyles.UserPaint, true);
                this.UpdateStyles();
            }
        }

        public class DoubleBufferedFlowLayoutPanel : FlowLayoutPanel
        {
            public DoubleBufferedFlowLayoutPanel()
            {
                this.DoubleBuffered = true;
                this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.ResizeRedraw |
                              ControlStyles.UserPaint, true);
                this.UpdateStyles();
            }
        }


        // test
        private void CenterFlowContent(FlowLayoutPanel flow)
        {
            if (flow.Controls.Count == 0) return;

            flow.WrapContents = true;
            flow.FlowDirection = FlowDirection.LeftToRight;

            int iconCount = flow.Controls.Count;

            // Assume all icons are same size
            Control icon = flow.Controls[0];
            int iconWidth = icon.Width + icon.Margin.Horizontal;
            int iconHeight = icon.Height + icon.Margin.Vertical;

            // ---- Layout rules based on number of icons ----
            int firstRowCount = 0, secondRowCount = 0;

            if (iconCount == 5)
            {
                // Admin: 3 on first row, 2 on second
                firstRowCount = 3;
                secondRowCount = 2;
            }
            else if (iconCount == 4)
            {
                // Staff: 2 on first row, 2 on second
                firstRowCount = 2;
                secondRowCount = 2;
            }
            else
            {
                // Fallback (if other counts appear)
                firstRowCount = (int)Math.Ceiling(iconCount / 2.0);
                secondRowCount = iconCount - firstRowCount;
            }

            // ---- Check if wide enough for single row (maximized) ----
            bool singleRow = flow.ClientSize.Width >= (iconCount * iconWidth + 40);

            int totalWidth, totalHeight;

            // ✅ Always reset breaks before reapplying layout
            foreach (Control c in flow.Controls)
                flow.SetFlowBreak(c, false);

            if (singleRow)
            {
                // ---- MAXIMIZED MODE ----
                totalWidth = iconCount * iconWidth;
                totalHeight = iconHeight;
            }
            else
            {
                // ---- MINIMIZED / NORMAL MODE ----
                totalWidth = Math.Max(firstRowCount, secondRowCount) * iconWidth;
                totalHeight = 2 * iconHeight;

                // ✅ Force break after first row count
                for (int i = 0; i < flow.Controls.Count; i++)
                {
                    if (i == firstRowCount - 1)
                    {
                        flow.SetFlowBreak(flow.Controls[i], true);
                        break;
                    }
                }
            }

            // ---- Center the icons ----
            int leftPadding = Math.Max((flow.ClientSize.Width - totalWidth) / 2, 0);

            // 🔼 Raise icons slightly toward the middle
            int verticalOffset = 180; // increase for higher placement
            int topPadding = Math.Max((flow.ClientSize.Height - totalHeight) / 2 - verticalOffset, 0);

            flow.Padding = new Padding(leftPadding, topPadding, 0, 0);
            flow.PerformLayout();
        }





        // Create a single tile (icon + text)

        private Panel CreateTile(string title, Image icon)
        {
            // Container for the tile
            Panel panel = new Panel
            {
                Width = 180,
                Height = 160,
                Margin = new Padding(2),
                BackColor = Color.Transparent
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 70F)); // top (icon)
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F)); // bottom (text)

            PictureBox pb = new PictureBox
            {
                Image = icon,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                Margin = new Padding(10)
            };

            Label lbl = new Label
            {
                Text = title, // <-- Put your full text here
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.TopCenter, // keeps all labels level
                Padding = new Padding(0, 5, 0, 0) // small top padding for balance  
            };


            // Add controls
            layout.Controls.Add(pb, 0, 0);
            layout.Controls.Add(lbl, 0, 1);

            panel.Controls.Add(layout);
            return panel;
        }

        // test

        private void Form1_Resize(object sender, EventArgs e)
        {
            CenterFlowContent(flow);

        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            CenterFlowContent(flow);

        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panelPopup_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {

        }

        private void timerSlide_Tick(object sender, EventArgs e)
        {
            if (!isPanelExpanded)
            {
                // Opening → slide LEFT (expand beside the button)
                if (panelPopup.Width < panelMaxWidth)
                {
                    panelPopup.Width += slideSpeed;

                    // Keep it aligned beside the profile button
                    panelPopup.Left = btnProfileAdmin.Left - panelPopup.Width;
                    panelPopup.Top = btnProfileAdmin.Top; // << FIX: stay beside vertically
                }
                else
                {
                    timerSlide.Stop();
                    isPanelExpanded = true;
                    panelPopup.Visible = true;
                }
            }
            else
            {
                // Closing → slide RIGHT (shrink back beside button)
                if (panelPopup.Width > panelMinWidth)
                {
                    panelPopup.Width -= slideSpeed;

                    // Keep it aligned beside the profile button
                    panelPopup.Left = btnProfileAdmin.Left - panelPopup.Width;
                    panelPopup.Top = btnProfileAdmin.Top; // << FIX
                }
                else
                {
                    timerSlide.Stop();
                    isPanelExpanded = false;
                    panelPopup.Visible = false;
                }
            }

        }

        private void timerSlideStaff_Tick(object sender, EventArgs e)
        {
            if (!isPanelExpandedStaff)
            {
                // Opening → slide LEFT (expand beside the button)
                if (panelPopupStaff.Width < panelMaxWidthStaff)
                {
                    panelPopupStaff.Width += slideSpeedStaff;
                    if (panelPopupStaff.Width > panelMaxWidthStaff) panelPopupStaff.Width = panelMaxWidthStaff; // clamp

                    // Always align to current button position
                    panelPopupStaff.Left = btnProfileStaff.Left - panelPopupStaff.Width;
                    panelPopupStaff.Top = btnProfileStaff.Top;
                }
                else
                {
                    timerSlideStaff.Stop();
                    isPanelExpandedStaff = true;
                    panelPopupStaff.Visible = true;
                }
            }
            else
            {
                // Closing → slide RIGHT (shrink back beside button)
                if (panelPopupStaff.Width > panelMinWidthStaff)
                {
                    panelPopupStaff.Width -= slideSpeedStaff;
                    if (panelPopupStaff.Width < panelMinWidthStaff) panelPopupStaff.Width = panelMinWidthStaff; // clamp

                    // Always align to current button position
                    panelPopupStaff.Left = btnProfileStaff.Left - panelPopupStaff.Width;
                    panelPopupStaff.Top = btnProfileStaff.Top;
                }
                else
                {
                    timerSlideStaff.Stop();
                    isPanelExpandedStaff = false;
                    panelPopupStaff.Visible = false;
                }
            }

        }

        private void ucAdminHome_Load(object sender, EventArgs e)
        {
            if (userRole == "admin")
            {
                btnProfileStaff.Visible = false;
                panelPopupStaff.Visible = false;
                panelPopupDownStaff.Visible = false;
            }

            if (userRole == "staff")
            {
                btnProfileAdmin.Visible = false;
                panelPopup.Visible = false;
                panelPopupDown.Visible = false;
            }
        }


        public void LoadTodayStats()
        {
            string connStr = DbConnection.ConnectionString;

            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();

                // Total Sales Today
                using (SQLiteCommand cmd = new SQLiteCommand(
                    @"SELECT IFNULL(SUM(o.TotalAmount), 0) 
              FROM Orders o 
              WHERE DATE(o.OrderDate) = DATE('now')", conn))
                {
                    labeltext_1 = "Total Expected Sales Today: ₱" +
                                  Convert.ToDecimal(cmd.ExecuteScalar()).ToString("N2");
                }

                // Total Customers Who Ordered Today
                using (SQLiteCommand cmd = new SQLiteCommand(
                    @"SELECT COUNT(DISTINCT CustomerID) 
              FROM Orders 
              WHERE DATE(OrderDate) = DATE('now')", conn))
                {
                    labeltext_4 = "Total Customers Today: " + cmd.ExecuteScalar().ToString();
                }

                // Completed Orders Today
                using (SQLiteCommand cmd = new SQLiteCommand(
                    @"SELECT COUNT(*) 
              FROM Orders 
              WHERE Status = 'Completed' 
              AND DATE(OrderDate) = DATE('now')", conn))
                {
                    labeltext_2 = "Completed Orders: " + cmd.ExecuteScalar().ToString();
                }

                // Pending Orders
                using (SQLiteCommand cmd = new SQLiteCommand(
                    @"SELECT COUNT(*) 
              FROM Orders 
              WHERE Status = 'Pending'", conn))
                {
                    labeltext_3 = "Pending Orders: " + cmd.ExecuteScalar().ToString();
                }

                // Total Orders Today
                using (SQLiteCommand cmd = new SQLiteCommand(
                    @"SELECT COUNT(*) 
              FROM Orders 
              WHERE DATE(OrderDate) = DATE('now')", conn))
                {
                    labeltext_5 = "Total Orders Today: " + cmd.ExecuteScalar().ToString();
                }
            }

            // 🟢 Now update the tile labels
            if (tiles.Count >= 5)
            {
                UpdateTileLabel(tiles[0], labeltext_1);
                UpdateTileLabel(tiles[1], labeltext_2);
                UpdateTileLabel(tiles[2], labeltext_3);
                UpdateTileLabel(tiles[3], labeltext_4);
                UpdateTileLabel(tiles[4], labeltext_5);
            }
        }

        private void UpdateTileLabel(Panel tile, string newText)
        {
            foreach (Control c in tile.Controls)
            {
                if (c is TableLayoutPanel layout)
                {
                    foreach (Control inner in layout.Controls)
                    {
                        if (inner is Label lbl)
                        {
                            lbl.Text = newText;
                            lbl.Refresh(); // optional, ensures UI updates immediately
                            return;
                        }
                    }
                }
            }
        }


        private void lblAdminName_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private DateTime lastClickTime = DateTime.MinValue;
        private bool isProfileActive = false;


        private void btnProfileAdmin_Click(object sender, EventArgs e)
        {
            // prevent double-click glitch
            if ((DateTime.Now - lastClickTime).TotalMilliseconds < 300)
                return;

            lastClickTime = DateTime.Now;

            // toggle panel and borderline
            isProfileActive = !isProfileActive;
            panelPopup.Visible = isProfileActive;

            if (isProfileActive)
            {
                btnProfileAdmin.FlatAppearance.BorderSize = 2;
                btnProfileAdmin.FlatAppearance.BorderColor = Color.FromArgb(21, 77, 113);
                btnProfileAdmin.FlatStyle = FlatStyle.Flat;

            }
            else
            {
                btnProfileAdmin.FlatAppearance.BorderSize = 0;
                btnProfileAdmin.FlatAppearance.BorderColor = Color.FromArgb(21, 77, 113);
                btnProfileAdmin.FlatStyle = FlatStyle.Flat;

            }

            // Prevent double-click conflict → ignore click if already sliding
            if (timerSlide.Enabled || timerSlideDown.Enabled)
                return;

            // Show the panels
            panelPopup.Visible = true;
            panelPopupDown.Visible = true;

            // Start the timers
            timerSlide.Start();
            timerSlideDown.Start();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Log Out Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ActivityLogger.LogAction(UserID, "Logout");
                formLogin loginForm = new formLogin();
                loginForm.Show();
                this.ParentForm.Close();
            }
        }

        private void timerSlideDown_Tick(object sender, EventArgs e)
        {
            if (!isPanelExpandeder)
            {

                if (panelPopupDown.Height < panelMaxHeight)
                {
                    panelPopupDown.Height += slideSpeeder;
                }
                else
                {
                    timerSlideDown.Stop();
                    isPanelExpandeder = true;
                }
            }
            else
            {

                if (panelPopupDown.Height > panelMinHeight)
                {
                    panelPopupDown.Height -= slideSpeeder;
                }
                else
                {
                    timerSlideDown.Stop();
                    isPanelExpandeder = false;
                    panelPopupDown.Visible = false;
                }
            }
        }


        private void btnProfileStaff_Click_2(object sender, EventArgs e)
        {
            // prevent double-click glitch
            if ((DateTime.Now - lastClickTime).TotalMilliseconds < 300)
                return;

            lastClickTime = DateTime.Now;

            // toggle panel and borderline
            isProfileActive = !isProfileActive;
            panelPopupStaff.Visible = isProfileActive;

            if (isProfileActive)
            {
                btnProfileStaff.FlatAppearance.BorderSize = 2;
                btnProfileStaff.FlatAppearance.BorderColor = Color.FromArgb(21, 77, 113);
                btnProfileStaff.FlatStyle = FlatStyle.Flat;
            }
            else
            {
                btnProfileStaff.FlatAppearance.BorderSize = 0;
                btnProfileStaff.FlatAppearance.BorderColor = Color.FromArgb(21, 77, 113);
                btnProfileStaff.FlatStyle = FlatStyle.Flat;
            }

            // Prevent double-click conflict → ignore click if already sliding
            if (timerSlideStaff.Enabled || timerSlideDownStaff.Enabled)
                return;

            // Show the panels
            panelPopupStaff.Visible = true;
            panelPopupDownStaff.Visible = true;

            // Start the timers
            timerSlideStaff.Start();
            timerSlideDownStaff.Start();
        }

        private void btnLogoutStaff_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Log Out Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ActivityLogger.LogAction(UserID, "Logout");
                formLogin loginForm = new formLogin();
                loginForm.Show();
                this.ParentForm.Close();
            }
        }

        private void timerSlideDownStaff_Tick(object sender, EventArgs e)
        {
            if (!isPanelExpandedDownStaff)
            {

                if (panelPopupDownStaff.Height < panelMaxHeightStaff)
                {
                    panelPopupDownStaff.Height += slideSpeederStaff;
                }
                else
                {
                    timerSlideDownStaff.Stop();
                    isPanelExpandedDownStaff = true;
                }
            }
            else
            {

                if (panelPopupDownStaff.Height > panelMinHeightStaff)
                {
                    panelPopupDownStaff.Height -= slideSpeederStaff;
                }
                else
                {
                    timerSlideDownStaff.Stop();
                    isPanelExpandedDownStaff = false;
                    panelPopupDownStaff.Visible = false;
                }
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Keep popup panel aligned beside the profile button
            if (panelPopup.Visible)
            {
                panelPopup.Left = btnProfileAdmin.Left - panelPopup.Width;
                panelPopup.Top = btnProfileAdmin.Top;
            }
        }

        private void ucAdmin_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                // Stop any running animations
                timerSlide.Stop();
                timerSlideDown.Stop();

                // Hide both panels
                panelPopup.Visible = false;
                panelPopupDown.Visible = false;

                // Reset sizes for smooth reopen later
                panelPopup.Width = panelMinWidth;
                panelPopupDown.Height = panelMinHeight;

                // Reset toggle flags
                isPanelExpanded = false;
                isPanelExpandeder = false;
                // Reset active/border state
                isProfileActive = false;
                btnProfileAdmin.FlatAppearance.BorderSize = 0;
            }
        }

        private void ucStaff_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                // Stop any running animations
                timerSlideStaff.Stop();
                timerSlideDownStaff.Stop();

                // Hide both panels
                panelPopupStaff.Visible = false;
                panelPopupDownStaff.Visible = false;

                // Reset sizes for smooth reopen later
                panelPopupStaff.Width = panelMinWidth;
                panelPopupDownStaff.Height = panelMinHeight;

                // Reset toggle flags
                isPanelExpandedStaff = false;
                isPanelExpandedDownStaff = false;
                // Reset active/border state
                isProfileActive = false;
                btnProfileStaff.FlatAppearance.BorderSize = 0;
            }
        }
    }
}

