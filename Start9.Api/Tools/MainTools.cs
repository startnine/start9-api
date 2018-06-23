using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Start9.Api.Plex;
using Point = System.Drawing.Point;
using BitmapImage = System.Windows.Media.Imaging.BitmapImage;
using BitmapCacheOption = System.Windows.Media.Imaging.BitmapCacheOption;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Text;

namespace Start9.Api.Tools
{
    /// <summary>
    ///  Main tools.
    /// </summary>
	public static class MainTools
	{
        public static PlexWindow SettingsWindow;

        /// <summary>
        /// Shows the settings window.
        /// </summary>
		public static void ShowSettings()
		{
			SettingsWindow.Show();
			if (!SettingsWindow.IsActive)
				SettingsWindow.Focus();
		}

        public enum DeviceCap
		{
			Vertres = 10,
			Desktopvertres = 117,
			Logpixelsy = 90
		}
	}
}