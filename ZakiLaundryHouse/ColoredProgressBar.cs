using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ZakiLaundryHouse
{
   
    public class ColoredProgressBar : ProgressBar
    {
        public Color BarColor { get; set; } = Color.DodgerBlue; // default blue color

        public ColoredProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = e.ClipRectangle;
            e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), rect); // background

            rect.Width = (int)(rect.Width * ((double)Value / Maximum));
            e.Graphics.FillRectangle(new SolidBrush(BarColor), 0, 0, rect.Width, rect.Height); // progress color

            e.Graphics.DrawRectangle(Pens.Gray, 0, 0, this.Width - 1, this.Height - 1); // optional border
        }
    }

   
}
