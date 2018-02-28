using System;
using System.Drawing;

namespace Start9.Api.Tools
{
	public static class DpiManager
	{
		public static float GetScalingFactor()
		{
			var g = Graphics.FromHwnd(IntPtr.Zero);
			return g.DpiY / 96;
		}

		public static int ConvertWpfUnitsToPixels(double wpfUnits)
		{
			return (int) (wpfUnits * GetScalingFactor());
		}

		public static int ConvertPixelsToWpfUnits(double pixels)
		{
			return (int) (pixels / GetScalingFactor());
		}
	}
}