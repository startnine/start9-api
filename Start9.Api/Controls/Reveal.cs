using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Start9.Api.Tools;
using Start9.Api;

namespace Start9.Api.Controls
{
    public class Reveal : Canvas
    {
        Timer RevealGlowTimer = new Timer(1);

        public Brush Hover
        {
            get => (Brush)GetValue(HoverProperty);
            set => SetValue(HoverProperty, (value));
        }

        internal static readonly DependencyProperty HoverProperty =
            DependencyProperty.Register("Hover", typeof(Brush), typeof(Reveal), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public double HoverWidth
        {
            get => (double)GetValue(HoverWidthProperty);
            set => SetValue(HoverWidthProperty, (value));
        }

        internal static readonly DependencyProperty HoverWidthProperty =
            DependencyProperty.Register("HoverWidth", typeof(double), typeof(Reveal), new PropertyMetadata((double)1));

        public double HoverHeight
        {
            get => (double)GetValue(HoverHeightProperty);
            set => SetValue(HoverHeightProperty, (value));
        }

        internal static readonly DependencyProperty HoverHeightProperty =
            DependencyProperty.Register("HoverHeight", typeof(double), typeof(Reveal), new PropertyMetadata((double)1));

        public Reveal()
        {
            IsVisibleChanged += Reveal_IsVisibleChanged;
            Loaded += Reveal_Loaded;
        }

        private void Reveal_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
                RevealGlowTimer.Start();
            else

                RevealGlowTimer.Stop();
        }

        private void Reveal_Loaded(object sender, RoutedEventArgs e)
        {
            SetReveal();
        }

        public void SetReveal()
        {
            //RevealGlowTimer.Stop();
            if (!(Background is VisualBrush))
            {
                Background = new VisualBrush();
            }
            /*Children.Clear();
            Children.Add(Hover);

            Grid RevealGlowGrid = new Grid()
            {
                Background = new ImageBrush(RevealGlowBitmapImage),
                Width = RevealGlowBitmapImage.PixelWidth,
                Height = RevealGlowBitmapImage.PixelHeight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 0),
                Visibility = Visibility.Visible,
                Focusable = false
            };*/

            var bg = (Background as VisualBrush);

            if (bg.Visual == null)
            {
                bg.Visual = new Grid()
                {
                    Background = new SolidColorBrush(Color.FromArgb(0x01, 0x0, 0x0, 0x0))
                };
                var gr = (bg.Visual as Grid);
                Binding widthBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("ActualWidth"),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(gr, Grid.WidthProperty, widthBinding);

                Binding heightBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("ActualHeight"),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(gr, Grid.HeightProperty, heightBinding);

                gr.Children.Add(new Canvas()
                {
                    Background = Hover,
                    Width = HoverWidth,
                    Height = HoverHeight,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                });
            }

            var vis = (bg.Visual as Grid).Children[0] as Canvas;
            /*if (Hover is ImageBrush)
            {
                var br = Hover as ImageBrush;
                vis.Width = br.ImageSource.Width;
                vis.Height = br.ImageSource.Height;
            }*/

            bg.ViewboxUnits = BrushMappingMode.Absolute;

            RevealGlowTimer.Elapsed += delegate
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    var c = SystemScaling.CursorPosition;
                    var p = PointToScreen(new Point(0, 0));
                    var t = new Point((c.X - p.X) - (vis.Width / 2), (c.Y - p.Y) - (vis.Height / 2));
                    /*bg.Viewbox = new Rect(
                        ((c.X - p.X) - (ActualWidth / 2)) * -1,
                        ((c.Y - p.Y) - (ActualHeight / 2)) * -1,
                        ActualWidth,
                        ActualHeight);*/
                    bg.Viewbox = new Rect(t.X * -1, t.Y * -1, ActualWidth, ActualHeight);
                    //vis.Margin = new Thickness(t.X, t.Y, t.X * -1, t.Y * -1);
                }));
            };

            if (IsVisible)
                RevealGlowTimer.Start();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }
    }
}