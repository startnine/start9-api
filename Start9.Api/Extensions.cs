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
    /// <summary>
    /// Various assortment of extensions.
    /// </summary>
	public static class Extensions
    {

        #region Point extensions
        /// <summary>
        /// Converts a <see cref="System.Drawing.Point"/> to a <see cref="Point"/>.
        /// </summary>
        /// <param name="Point">The <see cref="System.Drawing.Point"/> to convert.</param>
        /// <returns>A <see cref="Point"/> that points to the same position as <paramref name="Point"/>.</returns>
        public static Point ToWindowsMediaPoint(this System.Drawing.Point Point) => new Point(Point.X, Point.Y);

        /// <summary>
        /// Converts a <see cref="Point"/> to a <see cref="System.Drawing.Point"/>.
        /// </summary>
        /// <param name="Point">The <see cref="Point"/> to convert.</param>
        /// <returns>A <see cref="System.Drawing.Point"/> that points to the same position as <paramref name="Point"/>.</returns>
        public static System.Drawing.Point ToDrawingPoint(this Point Point) => new System.Drawing.Point((Int32)Point.X, (Int32)Point.Y);
        #endregion

        #region Bitmap extensions

        /// <summary>
        /// Converts a <see cref="Bitmap"/> to a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="bitmap">The <see cref="Bitmap"/> to convert.</param>
        /// <returns>A <see cref="BitmapSource"/> that has the same content as <paramref name="bitmap"/>.</returns>
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
                WinApi.DeleteObject(hBitmap);
			}
		}

        /// <summary>
        /// Converts an <see cref="Icon"/> to a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="icon">The <see cref="Icon"/> to convert.</param>
        /// <returns>A <see cref="BitmapSource"/> that has the same content as <paramref name="icon"/>.</returns>
        public static BitmapSource ToBitmapSource(this Icon icon)
		{
			if (icon == null)
				throw new ArgumentNullException(nameof(icon));

			var hBitmap = icon.ToBitmap().GetHbitmap();

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
                WinApi.DeleteObject(hBitmap);
			}
        }

        /// <summary>
        /// Converts a <see cref="BitmapImage"/> to a <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="bitmapImage">The <see cref="BitmapImage"/> to convert.</param>
        /// <returns>A <see cref="Bitmap"/> that has the same content as <paramref name="bitmapImage"/>.</returns>
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

        /// <summary>
        /// Gets the dominant color of an image.
        /// </summary>
        /// <param name="sourceImage">The image to get the dominant color of.</param>
        /// <returns>A <see cref="Color"/> representing the dominant color in the image.</returns>
        public static Color DominantColor(this BitmapImage sourceImage) // TODO: update this one to use pointers
        {
            var outputColor = Colors.Gray;

            var destColor = System.Drawing.Color.Gray;
            using (var sourceBitmap = sourceImage.ToDrawingBitmap())
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

        #endregion

        #region UIElement extensions

        public static Point PointToScreenInWpfUnits(this UIElement uiElement, Point point)
        {
            var uiPoint = uiElement.PointToScreen(point);

            return new Point(RealPixelsToWpfUnits(uiPoint.X), RealPixelsToWpfUnits(uiPoint.Y));
        }

        public static Point GetOffsetFromCursor(this UIElement uiElement)
        {
            var cursor = SystemScaling.CursorPosition;
            var uiPoint = uiElement.PointToScreen(new Point(0, 0));

            return new Point(cursor.X - RealPixelsToWpfUnits(uiPoint.X), cursor.Y - RealPixelsToWpfUnits(uiPoint.Y));
        }

        #endregion
    }
    }