using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using static Start9.Api.Tools.WinApi;
using Rect = System.Windows.Rect;

namespace Start9.Api.Tools
{
	public static class DwmTools
	{
		private const int GWL_STYLE = -16;

		private const ulong WS_VISIBLE = 0x10000000L,
			WS_BORDER = 0x00800000L,
			TARGETWINDOW = WS_BORDER | WS_VISIBLE;

		public static IntPtr GetThumbnail(IntPtr hWnd, UIElement targetElement)
		{
			Debug.WriteLine($"{hWnd} {targetElement}");
			DwmRegisterThumbnail(new WindowInteropHelper(Window.GetWindow(targetElement)).Handle, hWnd, out var thumbHandle);
			var targetElementPoint = MainTools.GetDpiScaledGlobalControlPosition(targetElement);
			Point targetElementOppositePoint;
			if (targetElement.GetType().IsAssignableFrom(typeof(Control)))
			{
				var targetControl = targetElement as Control;
				targetElementOppositePoint = new Point(targetElementPoint.X + targetControl.ActualWidth,
					targetElementPoint.Y + targetControl.ActualHeight);
			}
			else
			{
				targetElementOppositePoint = new Point(targetElementPoint.X, targetElementPoint.Y);
			}

			var targetRect = new Rect(targetElementPoint.X, targetElementPoint.Y, (int) targetElementOppositePoint.X,
				(int) targetElementOppositePoint.Y);

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