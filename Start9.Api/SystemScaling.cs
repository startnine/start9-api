using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Text;
using Graphics = System.Drawing.Graphics;
using Start9.Api;

namespace Start9.Api
{
    public static class SystemScaling
    {
        public static double ScalingFactor
        {
            get
            {
                var g = Graphics.FromHwnd(IntPtr.Zero);
                return g.DpiY / 96;
            }
        }

        public static Point CursorPosition
        {
            get
            {
                var p = System.Windows.Forms.Cursor.Position;
                return new Point(p.X.RealPixelsToWpfUnits(), p.Y.RealPixelsToWpfUnits());
            }
        }
    }
}
