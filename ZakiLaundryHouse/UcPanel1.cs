using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace ZakiLaundryHouse
{
    public class UcPanel1 : Panel
    {
        private int borderRadius = 40;
        private int borderSize = 1;
        private Color borderColor = Color.LightSteelBlue;

        public UcPanel1()
        {
            this.Size = new Size(150, 60);
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
        }
        private GraphicsPath GetFigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectSurface = new RectangleF(0, 0, this.Width, this.Height);
            GraphicsPath pathSurface = null;

            // ===== Draw rounded panel =====
            if (borderRadius > 2)
            {
                pathSurface = GetFigurePath(rectSurface, borderRadius);
                using (Pen penSurface = new Pen(this.Parent?.BackColor ?? Color.White, 2))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    // fill background
                    g.FillPath(brush, pathSurface);

                    // assign region for rounded shape
                    this.Region = new Region(pathSurface);

                    // draw surface outline
                    g.DrawPath(penSurface, pathSurface);

                    // draw border
                    if (borderSize >= 1)
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        g.DrawPath(penBorder, pathSurface);
                    }
                }
            }
            else
            {
                // For square panel when borderRadius is less than or equal to 2
                pathSurface = new GraphicsPath();
                pathSurface.AddRectangle(rectSurface); // Draw a normal rectangle

                this.Region = new Region(pathSurface);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(80, 245, 245, 245)))
                {
                    g.FillPath(brush, pathSurface);  // Fill the rectangle (no rounded corners)
                }

                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        g.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1); // Draw square border
                    }
                }
            }
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this.Parent != null)
            {
                this.Parent.BackColorChanged += (s, ev) =>
                {
                    if (this.DesignMode)
                        this.Invalidate();
                };
            }
        }
    }
}