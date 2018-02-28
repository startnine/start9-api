using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Start9.Api.Tools;
using Brush = System.Windows.Media.Brush;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Size = System.Drawing.Size;

namespace Start9.Api.Objects
{
	//Partially from http://blog.csdn.net/xeonol/article/details/1943067
	[MarkupExtensionReturnType(typeof(Brush))]
	public class NineGridImageBrush : MarkupExtension, INotifyPropertyChanged
	{
		private ImageBrush brushImageSource = new ImageBrush();

		private Thickness sizingMargins = new Thickness(0, 0, 0, 0);

		private double targetHeight = 1;

		public double TargetWidth { get; set; } = 1;

		public double TargetHeight
		{
			get => targetHeight;
			set
			{
				targetHeight = value;
				OnPropertyChanged("TargetHeight");
			}
		}

		public Thickness SizingMargins
		{
			get => sizingMargins;
			set
			{
				sizingMargins = value;
				OnPropertyChanged("SizingMargins");
			}
		}

		public ImageBrush BrushImageSource
		{
			get => brushImageSource;
			set
			{
				brushImageSource = value;
				OnPropertyChanged("BrushImageSource");
			}
		}


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (brushImageSource.ImageSource != null)
			{
				var bitmap = new BitmapImage();

				using (var memory = new MemoryStream())
				{
					InsetResize(MiscTools.GetSysDrawingBitmapFromImageSource(brushImageSource.ImageSource),
						new Size((int) TargetWidth, (int) TargetHeight), SizingMargins).Save(memory, ImageFormat.Png);
					memory.Position = 0;
					bitmap.BeginInit();
					bitmap.StreamSource = memory;
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.EndInit();
				}

				var brush = new ImageBrush(bitmap);

				return brush;
			}

			return new ImageBrush();
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