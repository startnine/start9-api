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
        public static double WpfUnitsToRealPixels(double wpfUnits) => (wpfUnits * SystemScaling.ScalingFactor);

        public static double RealPixelsToWpfUnits(double realPixels) => (realPixels / SystemScaling.ScalingFactor);

        public static float WpfUnitsToRealPixels(float wpfUnits) => (float)(wpfUnits * SystemScaling.ScalingFactor);

        public static float RealPixelsToWpfUnits(float realPixels) => (float)(realPixels / SystemScaling.ScalingFactor);

        public static int WpfUnitsToRealPixels(int wpfUnits) => (int)(wpfUnits * SystemScaling.ScalingFactor);

        public static int RealPixelsToWpfUnits(int realPixels) => (int)(realPixels / SystemScaling.ScalingFactor);

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
                return new Point(RealPixelsToWpfUnits(p.X), RealPixelsToWpfUnits(p.Y));
            }
        }
    }
}
