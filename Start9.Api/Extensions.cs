using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Icon = System.Drawing.Icon;
using Bitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;
using static Start9.Api.SystemScaling;
using System.IO;

namespace Start9.Api
{
	public static class Extensions
    {

        /*Begin Point-related extensions*/

        public static Point ToWindowsMediaPoint(this System.Drawing.Point Point) => new Point(Point.X, Point.Y);

        public static System.Drawing.Point ToDrawingPoint(this Point Point) => new System.Drawing.Point((int)Point.X, (int)Point.Y);

        /*End Point-related extensions*/

        /*Begin Bitmap/Imaging-related extensions*/

        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			var hBitmap = bitmap.GetHbitmap();

			try
			{
				return Imaging.CreateBitmapSourceFromHBitmap(
					hBitmap,
					IntPtr.Zero,
					Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());
			}
			finally
			{
				Start9.Api.WinApi.DeleteObject(hBitmap);
			}
		}

        public static BitmapSource ToBitmapSource(this Icon bitmap)
		{
			if (bitmap == null)
				throw new ArgumentNullException(nameof(bitmap));

			var hBitmap = bitmap.ToBitmap().GetHbitmap();

			try
			{
				return Imaging.CreateBitmapSourceFromHBitmap(
					hBitmap,
					IntPtr.Zero,
					Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());
			}
			finally
			{
                Start9.Api.WinApi.DeleteObject(hBitmap);
			}
        }

        public static Bitmap ToDrawingBitmap(this BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream()
            {
                Position = 0
            })
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage.CloneCurrentValue()));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public static Color DominantColor(this BitmapImage SourceImage)
        {
            var outputColor = Colors.Gray;

            System.Drawing.Color destColor = System.Drawing.Color.Gray;
            using (var sourceBitmap = SourceImage.ToDrawingBitmap())
            {
                using (var g = Graphics.FromImage(sourceBitmap))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(sourceBitmap, 0, 0, 1, 1);

                    destColor = sourceBitmap.GetPixel(0, 0);
                }
            }

            outputColor = Color.FromArgb(destColor.A, destColor.R, destColor.G, destColor.B);

            return outputColor;
        }

        /*End Bitmap/Imaging-related extensions*/

        /*Begin UIElement extensions*/

        public static Point PointToScreenInWpfUnits(this UIElement uiElement, Point point)
        {
            var uiPoint = uiElement.PointToScreen(point);

            return new Point(RealPixelsToWpfUnits(uiPoint.X), RealPixelsToWpfUnits(uiPoint.Y));
        }

        public static Point OffsetFromCursor(this UIElement uiElement)
        {
            var cursor = SystemScaling.CursorPosition;
            var uiPoint = uiElement.PointToScreen(new Point(0, 0));

            return new Point(cursor.X - RealPixelsToWpfUnits(uiPoint.X), cursor.Y - RealPixelsToWpfUnits(uiPoint.Y));
        }

        /*End UIElement extensions*/
    }
}