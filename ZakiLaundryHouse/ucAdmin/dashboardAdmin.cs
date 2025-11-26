using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;
using ZakiLaundryHouse.ucAdmin;
using static ZakiLaundryHouse.ucAdmin.ucAdminHome;
using System.Runtime.InteropServices;

namespace ZakiLaundryHouse
{
    public partial class dashboardAdmin : Form
    {

 
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public ucAdminOrders AdminOrdersUserControl { get; private set; }
        private UserControl currentControl = null;
        private ucAdminHome homeControl;
        private ucSales salesControl;
        private ucInventory inventoryControl;
        private ucPricing pricingControl;
        private ucAdminOrders ordersControl;
        private ucBilling billingControl;
        private ucCustomers customersControl;
        private ucUsers usersControl;
        private ucOrderForm orderFormControl;
        private string _role;
        private string userRole;
        bool isPanelExpanded = false;
        int panelMaxHeight = 150;
        int panelMinHeight = 0;
        int slideSpeed = 10;
        private Timer idleTimer;
        private DateTime lastActivityTime;
        private readonly TimeSpan idleLimit = TimeSpan.FromMinutes(30);                  
        private bool isLoggedOut = false;



        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;

        //        // Apply WS_EX_COMPOSITED style to reduce flickering
        //        cp.ExStyle |= 0x02000000;  // WS_EX_COMPOSITED

        //        // Recreate the handle to apply the new style
        //        return cp;
        //    }
        //}


        public ucAdminHome AdminHomeUserControl { get; set; }

        private int currentUserId;
        private string currentUserRole;

        public dashboardAdmin(int userId, string role)
        {
            InitializeComponent();

            currentUserId = userId;   // store user ID for log trail
            currentUserRole = role;   // store role for later use
            _role = role;

            panelTop.Width = 769;
            panelTop.Visible = true;
            panelTop.Dock = DockStyle.Top;
            panelTop.Left = (this.ClientSize.Width - panelTop.Width) / 2;
            this.Controls.Add(panelTop);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.UserPaint |
                  ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            this.Resize += new EventHandler(dashboardAdmin_Resize);
        }
        private void dashboardAdmin_Resize(object sender, EventArgs e)
        {
            // Dynamically adjust the height of panel1 to fill the remaining space
            panel1.Width = this.ClientSize.Width - panelSidemenu.Width; // Adjust width based on side menu width
            panel1.Height = this.ClientSize.Height - panelTop.Height;

            panelTop.Width = 769;
            panelTop.Left = (this.ClientSize.Width - panelTop.Width) / 2;

        }



        //private void EnableDoubleBuffering(Panel panel)
        //{
        //    typeof(Panel).InvokeMember("DoubleBuffered",
        //        System.Reflection.BindingFlags.SetProperty |
        //        System.Reflection.BindingFlags.Instance |
        //        System.Reflection.BindingFlags.NonPublic,
        //        null, panel, new object[] { true });
        //}
        bool userExpand = false;
        int minHeight = 0;
        int maxHeight = 120;

        private void customizeLayout()
        {
            panelReportsSubmenu.Height = minHeight;
            panelReportsSubmenu.Visible = true;
        }

        private void hideSubMenu()
        {
            if (panelReportsSubmenu.Visible == true)
                panelReportsSubmenu.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        public void addUserControl(UserControl userControl)
        {
            panel1.SuspendLayout();
            // Reuse existing instance if same type is already active
            foreach (Control c in panel1.Controls)
                c.Visible = false;

            userControl.Visible = true;
            userControl.BringToFront();
            panel1.ResumeLayout();
        }


        private void AddToPanel(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            uc.Visible = false;
            panel1.Controls.Add(uc);
        }

        private UserControl activeControl = null;
        private void ShowUserControl(UserControl uc)
        {

            if (activeControl == uc)
            {
                //Console.WriteLine($"[DEBUG] Duplicate call ignored for {uc.Name}");
                Console.WriteLine(Environment.StackTrace);
                return;
            }
            if (activeControl == uc)
                return;

            //  Clear search boxes from the previous page (if it has one)
            if (activeControl is ucCustomers customersUC)
                customersUC.ClearSearchBox();

            activeControl = uc;


            if (activeControl is ucPricing pricingUC)
                pricingUC.ResetForm();

            // Reset Sales if switching away (optional, similar)
            if (activeControl is ucSales salesUC)
                salesUC.ResetForm();

            //Console.WriteLine($"[DEBUG] ShowUserControl called for {uc.Name} at {DateTime.Now:HH:mm:ss.fff}");

            panel1.SuspendLayout();
            foreach (Control c in panel1.Controls)
                c.Visible = false;

            uc.Visible = true;
            uc.BringToFront();

            if (uc is ucBilling billingUC)
                billingUC.LoadPendingPayments();

            panel1.ResumeLayout();

            
        }



        private void dashboardAdmin_Load(object sender, EventArgs e)
        {
            customizeLayout();

            if (_role.ToLower() == "staff")
            {
                maxHeight = 80; // shorter size for staff
            }
            else
            {
                maxHeight = 120; // full size for admin
            }

            //BufferedPanel bufferedPanel = new BufferedPanel();
            //bufferedPanel.Dock = panel1.Dock;
            //bufferedPanel.Location = panel1.Location;
            //bufferedPanel.Size = panel1.Size;
            //bufferedPanel.BackColor = panel1.BackColor; // If needed

            //// Move all child controls from old panel1 to the new one
            //while (panel1.Controls.Count > 0)
            //{
            //    var ctrl = panel1.Controls[0];
            //    panel1.Controls.RemoveAt(0);
            //    bufferedPanel.Controls.Add(ctrl);
            //}

            //// Replace in the form
            //this.Controls.Remove(panel1);
            //this.Controls.Add(bufferedPanel);


            //// ✅ Redirect your reference to use the new one
            //panel1 = bufferedPanel;
            this.SuspendLayout();
            // Create all UCs only once
            homeControl = new ucAdminHome(currentUserId, _role);
            salesControl = new ucSales();
            inventoryControl = new ucInventory(this, null, _role);
            pricingControl = new ucPricing(this, null, _role);
            ordersControl = new ucAdminOrders(this);
            billingControl = new ucBilling(this, null);
            customersControl = new ucCustomers(this, null);
            usersControl = new ucUsers(this, null);
            orderFormControl = new ucOrderForm(this, null);


            // expose the created instances for other controls to call
            AdminHomeUserControl = homeControl;          // ensure the home reference is set
            AdminOrdersUserControl = ordersControl;      // <-- expose orders control


            // 🔔 When an item is archived in inventory, refresh pricing & orders automatically
            inventoryControl.ItemArchived += () =>
            {
                pricingControl.LoadPricing();       // refresh pricing list
            };



            ordersControl.OnOrderSaved += () =>
            {
                billingControl.LoadPendingPayments();  // refresh pending orders immediately
                homeControl?.LoadTodayStats();
                inventoryControl.LoadItems();
                customersControl.LoadCustomers();
                // optional: refresh dashboard stats
            };

            billingControl.OnPaymentCompleted += () =>
            {
                homeControl.LoadTodayStats(); // or whatever method refreshes your home dashboard
            };
            // Add them all to panel1 (hidden by default)
            AddToPanel(homeControl);
            AddToPanel(salesControl);
            AddToPanel(inventoryControl);
            AddToPanel(pricingControl);
            AddToPanel(ordersControl);
            AddToPanel(billingControl);
            AddToPanel(customersControl);
            AddToPanel(usersControl);

            // Show home by default
            ShowUserControl(homeControl);

            // Role-based UI
            if (_role.ToLower() == "staff")
            {
                btnUsers.Visible = false;
                btnSales.Visible = false;
            }
            else if (_role.ToLower() == "admin")
            {
                btnUsers.Visible = true;
                btnSales.Visible = true;
            }
            //EnableDoubleBufferingForAllGrids(this);
            this.ResumeLayout();
            this.Visible = true;
            //ReplacePanelsRecursive(panelMain);

            // ✅ Also replace panelMain itself if needed
            //if (!(panelMain is BufferedPanel))
            //{
            //    BufferedPanel newMain = new BufferedPanel();
            //    newMain.Name = panelMain.Name;
            //    newMain.Location = panelMain.Location;
            //    newMain.Size = panelMain.Size;
            //    newMain.Dock = panelMain.Dock;

            //    while (panelMain.Controls.Count > 0)
            //    {
            //        Control ctrl = panelMain.Controls[0];
            //        panelMain.Controls.RemoveAt(0);
            //        newMain.Controls.Add(ctrl);
            //    }

            //    this.Controls.Remove(panelMain);
            //    this.Controls.Add(newMain);

            //    panelMain = newMain;
            CbMin.Visible = true;
            CbMax.Visible = true;
            CbExit.Visible = true;

            CbMin.BringToFront();
            CbMax.BringToFront();
            CbExit.BringToFront();

            

    }




        private void btnReports_Click(object sender, EventArgs e)
        {

            userTransition.Start();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            ShowUserControl(salesControl);
           
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            ShowUserControl(inventoryControl);
           
            //inventoryControl.CheckLowStockItems();
            inventoryControl.CheckLowStockItemsOnce();

        }

        private void btnPricing_Click(object sender, EventArgs e)
        {
            ShowUserControl(pricingControl);
            
        }

        public void ShowHome()
        {
            btnHome_Click(null, EventArgs.Empty); // simulate Home click
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ShowUserControl(homeControl);
            homeControl.LoadTodayStats();  // ensure latest stats each time you go home
            

        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            ShowUserControl(ordersControl);
           
        }

        private void btnBilling_Click(object sender, EventArgs e)
        {
            ShowUserControl(billingControl);
         
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            ShowUserControl(customersControl);
            
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            ShowUserControl(usersControl);
            
        }

        private void userTransition_Tick(object sender, EventArgs e)
        {
            if (!userExpand)
            {
                panelReportsSubmenu.Height += 25;
                if (panelReportsSubmenu.Height >= maxHeight)
                {
                    panelReportsSubmenu.Height = maxHeight;
                    userTransition.Stop();
                    userExpand = true;
                }
            }
            else
            {
                panelReportsSubmenu.Height -= 25;
                if (panelReportsSubmenu.Height <= minHeight)
                {
                    panelReportsSubmenu.Height = minHeight;
                    userTransition.Stop();
                    userExpand = false;
                }
            }
        }
        //private void EnableDoubleBufferingForAllGrids(Control parent)
        //{
        //    foreach (Control c in parent.Controls)
        //    {
        //        if (c is DataGridView dgv)
        //        {
        //            typeof(DataGridView).InvokeMember("DoubleBuffered",
        //                System.Reflection.BindingFlags.NonPublic |
        //                System.Reflection.BindingFlags.Instance |
        //                System.Reflection.BindingFlags.SetProperty,
        //                null, dgv, new object[] { true });
        //        }

        //  Recursively go into child containers (panels, usercontrols, etc.)
        //        if (c.HasChildren)
        //            EnableDoubleBufferingForAllGrids(c);
        //    }
        //}




        private void panelLogo_Paint(object sender, PaintEventArgs e)
        {

        }
        //private void ReplacePanelsRecursive(Control parent)
        //{
        //    List<Control> toReplace = new List<Control>();

        //    // Find all Panels inside this parent
        //    foreach (Control c in parent.Controls)
        //    {
        //        if (c is Panel && !(c is BufferedPanel)) // Only replace normal panels
        //            toReplace.Add(c);

        //        // Recurse into children
        //        if (c.HasChildren)
        //            ReplacePanelsRecursive(c);
        //    }

        //    // Replace panels with BufferedPanel
        //    foreach (Panel oldPanel in toReplace)
        //    {
        //        BufferedPanel newPanel = new BufferedPanel();

        //        // Copy basic properties
        //        newPanel.Name = oldPanel.Name;
        //        newPanel.Location = oldPanel.Location;
        //        newPanel.Size = oldPanel.Size;
        //        newPanel.Dock = oldPanel.Dock;
        //        newPanel.BackColor = oldPanel.BackColor;
        //        newPanel.Anchor = oldPanel.Anchor;
        //        newPanel.Padding = oldPanel.Padding;
        //        newPanel.Margin = oldPanel.Margin;

        //        // Move child controls
        //        while (oldPanel.Controls.Count > 0)
        //        {
        //            Control ctrl = oldPanel.Controls[0];
        //            oldPanel.Controls.RemoveAt(0);
        //            newPanel.Controls.Add(ctrl);
        //        }

        //        // Replace in parent
        //        parent.Controls.Remove(oldPanel);
        //        parent.Controls.Add(newPanel);
        //    }
        //}

        private void CbMin_Click(object sender, EventArgs e)
        {
            Form parentForm = this;
            if (parentForm != null)
            {
                parentForm.WindowState = FormWindowState.Minimized;
            }
        }

        private void CbMax_Click(object sender, EventArgs e)
        {
            Form parentForm = this;
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

        private void CbExit_Click(object sender, EventArgs e)
        {
            ActivityLogger.LogAction(currentUserId, "Logout");
            Application.Exit();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xF012, 0);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
       

       


       

       

    }
}



