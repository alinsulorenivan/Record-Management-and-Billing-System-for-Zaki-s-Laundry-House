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
    public partial class FormDimmer : Form
    {
        public FormDimmer()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.Black;
            this.Opacity = 0.5; // You can adjust this (0.3 to 0.7 works well)
            this.ShowInTaskbar = false;
            this.TopMost = false;
        }

        private void FormDimmer_Load(object sender, EventArgs e)
        {

        }
    }
}
