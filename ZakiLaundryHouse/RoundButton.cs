using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ZakiLaundryHouse
{
    class RoundButton : Button
    {

        protected override void OnPaint(PaintEventArgs pevent)
        {
            GraphicsPath grp = new GraphicsPath();
            grp.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(grp);
            base.OnPaint(pevent);
        }
    }
}
