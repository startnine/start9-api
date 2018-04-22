using Start9.Api.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rectangle = System.Drawing.Rectangle;
using Bitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System.IO;
using System.Windows.Controls;
using System.Diagnostics;

namespace Start9.Api.Objects
{
    [MarkupExtensionReturnType(typeof(System.Windows.Media.Brush))]
    public class RevealBrush : MarkupExtension, INotifyPropertyChanged
    {
        /*Brush _hoverBrush = new SolidColorBrush(Colors.Red);

        public Brush HoverBrush
        {
            get => _hoverBrush;
            set => _hoverBrush = value;
        }

        double _hoverWidth = (double)100;

        public double HoverWidth
        {
            get => _hoverWidth;
            set => _hoverWidth = value;
        }

        double _hoverHeight = (double)100;

        public double HoverHeight
        {
            get => _hoverHeight;
            set => _hoverHeight = value;
        }*/

        FrameworkElement _hover = new Canvas()
        {
            Background = new SolidColorBrush(Colors.Red),
            Width = 24,
            Height = 24
        };

        public FrameworkElement Hover
        {
            get => _hover;
            set => _hover = value;
        }

        FrameworkElement _destination = null;

        public FrameworkElement Destination
        {
            get => _destination;
            set => _destination = value;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(null, null);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                if ((Destination != null) && (Hover != null) && (Destination.Parent != null))
                {
                    //var render = new RenderTargetBitmap((int)Destination.ActualWidth, (int)Destination.ActualHeight, 1, 1, PixelFormats.Default);
                    //render.Render(Hover);

                    /*using (Graphics g = Graphics.FromImage(new Bitmap((int)Destination.ActualWidth, (int)Destination.ActualHeight, PixelFormat.Format32bppArgb)))
                    {
                        //_destination.
                        var c = MainTools.GetDpiScaledCursorPosition();
                        var p = MainTools.GetDpiScaledGlobalControlPosition(Destination);
                        var t = new Point(c.X - p.X, c.Y - p.Y);
                        Rectangle targetLocation = new Rectangle((int)t.X, (int)t.Y, (int)_hoverWidth, (int)_hoverHeight);
                        g.(_hoverBrush, targetLocation);
                    }*/
                    var c = MainTools.GetDpiScaledCursorPosition();
                    var p = MainTools.GetDpiScaledGlobalControlPosition(Destination);
                    var t = new Point(c.X - p.X, c.Y - p.Y);
                    Rect targetLocation = new Rect(t.X, t.Y, Hover.ActualWidth, Hover.ActualHeight);

                    DrawingVisual drawingVisual = new DrawingVisual();

                    var context = drawingVisual.RenderOpen();

                    context.DrawRectangle(
                    new VisualBrush()
                    {
                        Visual = Hover
                    }, null, targetLocation);

                    Bitmap bitmap = new Bitmap((int)Destination.Width, (int)Destination.Height);
                    //encoder.Frames.Add(BitmapFrame.Create(render));

                    var stream = new MemoryStream();
                    bitmap.Save(stream, ImageFormat.Png);


                    return new ImageBrush(bitmap.ToBitmapSource());
                }
                else
                {
                    return new SolidColorBrush(Colors.Pink);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new SolidColorBrush(Colors.Pink);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}