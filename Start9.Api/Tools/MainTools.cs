using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IWshRuntimeLibrary;
using Start9.Api.Plex;
using Color = System.Drawing.Color;
using File = System.IO.File;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;

namespace Start9.Api.Tools
{
	public static class MainTools
	{
		public enum DeviceCap
		{
			Vertres = 10,
			Desktopvertres = 117,
			Logpixelsy = 90
		}

		public static PlexWindow SettingsWindow;

		public static void ShowSettings()
		{
			SettingsWindow.Show();
			if (!SettingsWindow.IsActive)
				SettingsWindow.Focus();
		}

		private static float GetScalingFactor()
		{
			var g = Graphics.FromHwnd(IntPtr.Zero);
			return g.DpiY / 96;
		}


		public static Point GetDpiScaledCursorPosition()
		{
			var p = Cursor.Position;
			p.X = DpiManager.ConvertPixelsToWpfUnits(p.X);
			p.Y = DpiManager.ConvertPixelsToWpfUnits(p.Y);
			return p;
		}

		public static Point GetDpiScaledGlobalControlPosition(UIElement uiElement)
		{
			var uiPoint = uiElement.PointToScreen(new System.Windows.Point(0, 0));

			var uiDrawPoint = new Point(DpiManager.ConvertPixelsToWpfUnits(uiPoint.X),
				DpiManager.ConvertPixelsToWpfUnits(uiPoint.Y));
			return uiDrawPoint;
		}
	}

	public static class ShortcutTools
	{
		public enum InstallState
		{
			NotUsed = -7,
			BadConfig = -6,
			Incomplete = -5,
			SourceAbsent = -4,
			MoreData = -3,
			InvalidArg = -2,
			Unknown = -1,
			Broken = 0,
			Advertised = 1,
			Removed = 1,
			Absent = 2,
			Local = 3,
			Source = 4,
			Default = 5
		}

		public const int MaxFeatureLength = 38;
		public const int MaxGuidLength = 38;
		public const int MaxPathLength = 1024;

		public static string GetTargetPath(string filePath)
		{
			var targetPath = ResolveMsiShortcut(filePath);

			if (targetPath == null) targetPath = ResolveShortcut(filePath);

			if (targetPath == null) targetPath = GetInternetShortcut(filePath);

			if ((targetPath == null) | (targetPath == "") | (targetPath.Replace(" ", "") == ""))
				return filePath;
			return targetPath;
		}

		public static string GetInternetShortcut(string filePath)
		{
			try
			{
				var url = "";

				using (TextReader reader = new StreamReader(filePath))
				{
					var line = "";
					while ((line = reader.ReadLine()) != null)
						if (line.StartsWith("URL="))
						{
							var splitLine = line.Split('=');
							if (splitLine.Length > 0)
							{
								url = splitLine[1];
								break;
							}
						}
				}

				return url;
			}
			catch
			{
				return null;
			}
		}

		private static string ResolveShortcut(string filePath)
		{
			// IWshRuntimeLibrary is in the COM library "Windows Script Host Object Model"
			var shell = new WshShell();

			try
			{
				var shortcut = (IWshShortcut) shell.CreateShortcut(filePath);
				return shortcut.TargetPath;
			}
			catch
			{
				// A COMException is thrown if the file is not a valid shortcut (.lnk) file 
				return null;
			}
		}

		[DllImport("msi.dll", CharSet = CharSet.Auto)]
		public static extern uint MsiGetShortcutTarget(string targetFile, StringBuilder productCode, StringBuilder featureID,
			StringBuilder componentCode);

		[DllImport("msi.dll", CharSet = CharSet.Auto)]
		public static extern InstallState MsiGetComponentPath(string productCode, string componentCode,
			StringBuilder componentPath, ref int componentPathBufferSize);

		private static string ResolveMsiShortcut(string file)
		{
			var product = new StringBuilder(MaxGuidLength + 1);
			var feature = new StringBuilder(MaxFeatureLength + 1);
			var component = new StringBuilder(MaxGuidLength + 1);

			MsiGetShortcutTarget(file, product, feature, component);

			var pathLength = MaxPathLength;
			var path = new StringBuilder(pathLength);

			var installState = MsiGetComponentPath(product.ToString(), component.ToString(), path, ref pathLength);
			if (installState == InstallState.Local)
				return path.ToString();
			return null;
		}
	}

	public static class MiscTools
	{
		public static Color GetColorFromImage(string sourceImagePath)
		{
			var outputColor = Color.Gray;
			var sourceBitmap = new Bitmap(1, 1);

			if (sourceImagePath == Environment.ExpandEnvironmentVariables(@"%windir%\Explorer.exe"))
			{
				outputColor = Color.FromArgb(255, 0, 130, 153);
			}
			else if (File.Exists(sourceImagePath) || Directory.Exists(sourceImagePath))
			{
				if (File.Exists(sourceImagePath))
				{
					sourceBitmap = Bitmap.FromHicon(Icon.ExtractAssociatedIcon(sourceImagePath).Handle);
				}
				else if (Directory.Exists(sourceImagePath))
				{
					var shinfo = new WinApi.ShFileInfo();
					WinApi.SHGetFileInfo(sourceImagePath, 0, ref shinfo, (uint) Marshal.SizeOf(shinfo), 256);
					sourceBitmap = Bitmap.FromHicon(shinfo.hIcon);
				}

				Color destColor;
				using (var destBitmap = new Bitmap(1, 1))
				using (var g = Graphics.FromImage(destBitmap))
				{
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.DrawImage(sourceBitmap, 0, 0, 1, 1);

					destColor = destBitmap.GetPixel(0, 0);
				}

				outputColor = Color.FromArgb(255, destColor.R, destColor.G, destColor.B);
			}

			return outputColor;
		}

		public static BitmapSource GetBitmapSourceFromSysDrawingBitmap(Bitmap SourceSysDrawingBitmap)
		{
			return Imaging.CreateBitmapSourceFromHBitmap(
				SourceSysDrawingBitmap.GetHbitmap(),
				IntPtr.Zero,
				Int32Rect.Empty,
				BitmapSizeOptions.FromWidthAndHeight(SourceSysDrawingBitmap.Width, SourceSysDrawingBitmap.Height));
		}

		public static BitmapImage GetBitmapImageFromBitmapSource(BitmapSource bitmapSource)
		{
			//from https://stackoverflow.com/questions/5338253/bitmapsource-to-bitmapimage
			var encoder = new PngBitmapEncoder();
			var memoryStream = new MemoryStream();
			var bImg = new BitmapImage();

			encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
			encoder.Save(memoryStream);

			memoryStream.Position = 0;
			bImg.BeginInit();
			bImg.StreamSource = memoryStream;
			bImg.EndInit();

			memoryStream.Close();

			return bImg;
		}

		public static BitmapImage GetBitmapImageFromSysDrawingBitmap(Bitmap SourceSysDrawingBitmap)
		{
			var memory = new MemoryStream
			{
				Position = 0
			};
			SourceSysDrawingBitmap.Save(memory, ImageFormat.Png);
			var bitmap = new BitmapImage
			{
				StreamSource = memory,
				CacheOption = BitmapCacheOption.Default
			};
			bitmap.BeginInit();
			bitmap.EndInit();
			return bitmap;
		}

		public static Bitmap ConvertBitmapImageToBitmap(BitmapImage bitmapImage)
		{
			using (var outStream = new MemoryStream
			{
				Position = 0
			})
			{
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage.CloneCurrentValue()));
				enc.Save(outStream);
				var bitmap = new Bitmap(outStream);

				return new Bitmap(bitmap);
			}

			/*try
			{
			    return new Bitmap(bitmapImage.StreamSource);
			}
			catch
			{
			    return (Bitmap)(Bitmap.FromFile(bitmapImage.UriSource.AbsolutePath));
			}*/
		}

		public static Bitmap GetSysDrawingBitmapFromImageSource(ImageSource src)
		{
			//https://stackoverflow.com/questions/32073767/convert-system-windows-media-imagesource-to-system-drawing-bitmap
			var source = src as BitmapImage;
			var width = (int) src.Width;
			var height = (int) src.Height;
			var stride = width * ((source.Format.BitsPerPixel + 7) / 8);
			var ptr = IntPtr.Zero;
			try
			{
				ptr = Marshal.AllocHGlobal(height * stride);
				source.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
				using (var bmp = new Bitmap(width, height, stride, PixelFormat.Format1bppIndexed, ptr))
				{
					return new Bitmap(bmp);
				}
			}
			finally
			{
				if (ptr != IntPtr.Zero) Marshal.FreeHGlobal(ptr);
			}
		}

		public static ImageSource GetIconFromFilePath(string path)
		{
			return GetIconFromFilePath(path, 16, 16, 0x000000001 | 0x100);
		}

		public static ImageSource GetIconFromFilePath(string path, int size)
		{
			return GetIconFromFilePath(path, size, size, 0x000000001 | 0x100);
		}

		public static ImageSource GetIconFromFilePath(string path, int width, int height)
		{
			return GetIconFromFilePath(path, width, height, 0x000000001 | 0x100);
		}

		public static ImageSource GetIconFromFilePath(string path, uint flags)
		{
			return GetIconFromFilePath(path, 16, 16, flags);
		}

		public static ImageSource GetIconFromFilePath(string path, int size, uint flags)
		{
			return GetIconFromFilePath(path, size, size, flags);
		}

		public static ImageSource GetIconFromFilePath(string path, int width, int height, uint flags)
		{
			var shInfo = new WinApi.ShFileInfo();
			WinApi.SHGetFileInfo(path, 0, ref shInfo, (uint) Marshal.SizeOf(shInfo), 0x000000001 | 0x100);
			var entryIcon = Icon.FromHandle(shInfo.hIcon);
			ImageSource entryIconImageSource = Imaging.CreateBitmapSourceFromHIcon(
				entryIcon.Handle,
				Int32Rect.Empty,
				BitmapSizeOptions.FromWidthAndHeight(Convert.ToInt32(DpiManager.ConvertPixelsToWpfUnits(width)),
					Convert.ToInt32(DpiManager.ConvertPixelsToWpfUnits(height)))
			);
			return entryIconImageSource;
		}
	}
}