using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Start9.Api.Tools;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Size = System.Drawing.Size;

namespace Start9.Api.Objects
{
	public class ImageBrushNineGridInfo : DependencyObject
	{
		public static readonly DependencyProperty SizingImageBrushProperty =
			DependencyProperty.Register("SizingImageBrush", typeof(ImageBrush), typeof(ImageBrushNineGridInfo),
				new PropertyMetadata(new ImageBrush()));

		public static readonly DependencyProperty SizingMarginProperty =
			DependencyProperty.Register("SizingMargin", typeof(Thickness), typeof(ImageBrushNineGridInfo),
				new PropertyMetadata(new Thickness(0, 0, 0, 0)));

		public static readonly DependencyProperty TargetWidthProperty =
			DependencyProperty.Register("TargetWidth", typeof(double), typeof(ImageBrushNineGridInfo),
				new PropertyMetadata((double) 10));

		public static readonly DependencyProperty TargetHeightProperty =
			DependencyProperty.Register("TargetHeight", typeof(double), typeof(ImageBrushNineGridInfo),
				new PropertyMetadata((double) 10));

		public ImageBrush SizingImageBrush
		{
			get => (ImageBrush) GetValue(SizingImageBrushProperty);
			set => SetValue(SizingImageBrushProperty, value);
		}

		public Thickness SizingMargin
		{
			get => (Thickness) GetValue(SizingMarginProperty);
			set => SetValue(SizingMarginProperty, value);
		}

		public double TargetWidth
		{
			get => (double) GetValue(TargetWidthProperty);
			set => SetValue(TargetWidthProperty, value);
		}

		public double TargetHeight
		{
			get => (double) GetValue(TargetHeightProperty);
			set => SetValue(TargetHeightProperty, value);
		}
	}

	public class ImageBrushNineGridConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			var fromControl = value as Control;
			// Do the conversion from bool to visibility
			//InsetResize()
			if (parameter is ImageBrushNineGridInfo)
			{
				var paramNineGridInfo = parameter as ImageBrushNineGridInfo;
				if (paramNineGridInfo.SizingImageBrush.ImageSource != null)
				{
					var bitmap = new BitmapImage();
					var b = InsetResize(MiscTools.GetSysDrawingBitmapFromImageSource(paramNineGridInfo.SizingImageBrush.ImageSource),
						new Size((int) paramNineGridInfo.TargetWidth, (int) paramNineGridInfo.TargetHeight),
						paramNineGridInfo.SizingMargin);
					var memory = new MemoryStream();
					memory.Position = 0;
					b.Save(memory, ImageFormat.Png); //(memory, System.Drawing.Imaging.ImageFormat.Png);
					bitmap.BeginInit();
					bitmap.StreamSource = memory;
					//bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.EndInit();
					return new ImageBrush(bitmap);
				}

				return new ImageBrush();
			}

			return new ImageBrush();
		}

		public object ConvertBack(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			var fromControl = value as Control;
			return fromControl.Background;
		}

		//huge thanccs to dejco
		private Bitmap InsetResize(Bitmap sourceBitmap, Size destSize, Thickness insetThickness)
		{
			var outputBitmap = new Bitmap(destSize.Width, destSize.Height);
			try
			{
				var inset = new Int32Rect((int) insetThickness.Left, (int) insetThickness.Top, (int) insetThickness.Right,
					(int) insetThickness.Bottom);

				using (var g = Graphics.FromImage(outputBitmap))
				{
					g.CompositingQuality = CompositingQuality.AssumeLinear;
					g.InterpolationMode = InterpolationMode.NearestNeighbor;
					g.PixelOffsetMode = PixelOffsetMode.Half;
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.CompositingMode = CompositingMode.SourceCopy;

					g.DrawRectangle(new Pen(Color.FromArgb(0, 0, 0, 0)), 0, 0, destSize.Width, destSize.Height);

					g.DrawImage(sourceBitmap,
						new Rectangle(inset.X, inset.Y, destSize.Width - (inset.X + inset.Width),
							destSize.Height - (inset.Y + inset.Height)), //destination
						new Rectangle(inset.X, inset.Y, sourceBitmap.Width - (inset.X + inset.Width),
							sourceBitmap.Height - (inset.Y + inset.Height)), //source
						GraphicsUnit.Pixel);

					if (inset.Y > 0) //Top
						g.DrawImage(sourceBitmap,
							new Rectangle(inset.X, 0, destSize.Width - (inset.X + inset.Width), inset.Y), //destination
							new Rectangle(inset.X, 0, sourceBitmap.Width - (inset.X + inset.Width), inset.Y), //source
							GraphicsUnit.Pixel);

					if (inset.Height > 0)
						g.DrawImage(sourceBitmap,
							new Rectangle(inset.X, destSize.Height - inset.Height, destSize.Width - (inset.X + inset.Width),
								inset.Height), //destination
							new Rectangle(inset.X, sourceBitmap.Height - inset.Height, sourceBitmap.Width - (inset.X + inset.Width),
								inset.Height), //source
							GraphicsUnit.Pixel);

					if (inset.X > 0)
					{
						//left
						g.DrawImage(sourceBitmap,
							new Rectangle(0, inset.Y, inset.X, destSize.Height - (inset.Y + inset.Height)), //destination
							new Rectangle(0, inset.Y, inset.X, sourceBitmap.Height - (inset.Y + inset.Height)), //source
							GraphicsUnit.Pixel);

						//top left
						if (inset.Y > 0)
							g.DrawImage(sourceBitmap,
								new Rectangle(0, 0, inset.X, inset.Y), //destination
								new Rectangle(0, 0, inset.X, inset.Y), //source
								GraphicsUnit.Pixel);

						//bottom left
						if (inset.Height > 0)
							g.DrawImage(sourceBitmap,
								new Rectangle(0, destSize.Height - inset.Height, inset.X, inset.Height), //destination
								new Rectangle(0, sourceBitmap.Height - inset.Height, inset.X, inset.Height), //source
								GraphicsUnit.Pixel);
					}

					if (inset.Width > 0) //Right
					{
						g.DrawImage(sourceBitmap,
							new Rectangle(destSize.Width - inset.Width, inset.Y, inset.Width,
								destSize.Height - (inset.Y + inset.Height)), //destination
							new Rectangle(sourceBitmap.Width - inset.Width, inset.Y, inset.Width,
								sourceBitmap.Height - (inset.Y + inset.Height)), //source
							GraphicsUnit.Pixel);

						//top right
						if (inset.Y > 0)
							g.DrawImage(sourceBitmap,
								new Rectangle(destSize.Width - inset.Width, 0, inset.Width, inset.Y), //destination
								new Rectangle(sourceBitmap.Width - inset.Width, 0, inset.Width, inset.Y), //source
								GraphicsUnit.Pixel);

						//bottom right
						if (inset.Height > 0)
							g.DrawImage(sourceBitmap,
								new Rectangle(destSize.Width - inset.Width, destSize.Height - inset.Height, inset.Width,
									inset.Height), //destination
								new Rectangle(sourceBitmap.Width - inset.Width, sourceBitmap.Height - inset.Height, inset.Width,
									inset.Height), //source
								GraphicsUnit.Pixel);
					}
				}
			}
			catch
			{
			}

			return outputBitmap;
		}
	}
}