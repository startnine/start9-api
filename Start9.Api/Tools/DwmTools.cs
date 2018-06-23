using System;
using System.Windows.Interop;
using System.Diagnostics;
using System.Windows;
using static Start9.Api.WinApi;
using Rect = System.Windows.Rect;

namespace Start9.Api.Tools
{

    /// <summary>
    /// An assortment of tools related to the desktop windowm manager.
    /// </summary>
    public static class DwmTools
	{
		const Int32 GWL_STYLE = -16;

		const UInt64 WS_VISIBLE = 0x10000000L,
					WS_BORDER           = 0x00800000L,
					TARGETWINDOW        = WS_BORDER | WS_VISIBLE;

        /// <summary>
        /// Gets the thumbnail of a window.
        /// </summary>
        /// <param name="hWnd">The window to get the thumbnail of</param>
        /// <param name="targetElement">The target element.</param>
        /// <returns>A handle to the window thumbnail.</returns>
        public static IntPtr GetThumbnail(IntPtr hWnd, System.Windows.UIElement targetElement)
        {
            Debug.WriteLine($"{hWnd} {targetElement}");
			DwmRegisterThumbnail(new WindowInteropHelper(Window.GetWindow(targetElement)).Handle, hWnd, out var thumbHandle);
            var targetElementPoint = targetElement.PointToScreenInWpfUnits(new Point(0, 0));
            Point targetElementOppositePoint;
            if (targetElement.GetType().IsAssignableFrom(typeof(System.Windows.Controls.Control)))
            {
                var targetControl = (targetElement as System.Windows.Controls.Control);
				targetElementOppositePoint = new Point(targetElementPoint.X + targetControl.ActualWidth,
													   targetElementPoint.Y + targetControl.ActualHeight);
			}
            else
            {
                targetElementOppositePoint = new Point(targetElementPoint.X, targetElementPoint.Y);
            }

            var targetRect = new Rect(targetElementPoint.X, targetElementPoint.Y, (Int32)(targetElementOppositePoint.X), (Int32)(targetElementOppositePoint.Y));

            DwmQueryThumbnailSourceSize(thumbHandle, out var size);

            var props = new DwmThumbnailProperties
			{
                fVisible = true,
                dwFlags = DwmTnpVisible | DwmTnpRectdestination | DwmTnpOpacity,
                opacity = 255,
                rcDestination = new WinApi.Rect(0, 0, 100, 100)
            };


			DwmUpdateThumbnailProperties(thumbHandle, ref props);
            return thumbHandle;
        }
    }
}
