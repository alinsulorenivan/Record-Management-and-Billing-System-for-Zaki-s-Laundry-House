using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class SearchResultControl : UserControl
    {
        public event EventHandler<string> CustomerClicked;
        private Data customerData;

        public static bool clicked = false;

        public SearchResultControl()
        {
            InitializeComponent();
            this.Click += SearchResultControl_Click;
            lblName.Click += SearchResultControl_Click;
            lblContact.Click += SearchResultControl_Click;

            this.MouseEnter += SearchResultControl_MouseHover;
            this.MouseLeave += SearchResultControl_MouseLeave;

            lblName.MouseEnter += SearchResultControl_MouseHover;
            lblName.MouseLeave += SearchResultControl_MouseLeave;

            lblContact.MouseEnter += SearchResultControl_MouseHover;
            lblContact.MouseLeave += SearchResultControl_MouseLeave;

            picProfile.MouseEnter += SearchResultControl_MouseHover;
            picProfile.MouseLeave += SearchResultControl_MouseLeave;
        }

        private void SearchResultControl_Click(object sender, EventArgs e)
        {
            clicked = true;
            if (customerData != null)
            {
                CustomerClicked?.Invoke(this, customerData.name);
            }
        }

        public void details(Data d)
        {
            customerData = d;
            lblName.Text = d.name;
            lblContact.Text = d.contact;
            picProfile.Image = Properties.Resources.profilePic;
        }

        public Data GetCustomerData()
        {
            return customerData;
        }

        private void SearchResultControl_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void SearchResultControl_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Lavender;
        }

        private void SearchResultControl_Load(object sender, EventArgs e)
        {

        }
    }
}
