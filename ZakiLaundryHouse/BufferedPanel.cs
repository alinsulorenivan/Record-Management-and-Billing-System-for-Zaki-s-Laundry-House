using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZakiLaundryHouse
{
    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }
        private void EnableDoubleBuffering(Control c)
        {
            if (c == null) return;

            try
            {
                // Set the protected DoubleBuffered property
                typeof(Control).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, c, new object[] { true });

                // Call the protected SetStyle method to set additional flags
                var setStyle = typeof(Control).GetMethod("SetStyle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (setStyle != null)
                {
                    var styles = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint;
                    setStyle.Invoke(c, new object[] { styles, true });
                }

                // Call the protected UpdateStyles method to apply
                var updateStyles = typeof(Control).GetMethod("UpdateStyles", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                updateStyles?.Invoke(c, null);
            }
            catch (Exception ex)
            {
                // optional: log or ignore
                Console.WriteLine("EnableDoubleBuffering failed: " + ex.Message);
            }
        }

    }

}
