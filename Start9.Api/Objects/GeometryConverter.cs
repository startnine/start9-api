using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Start9.Api.Objects
{
    //THERE WAS AN ATTEMPT
    //THIS ENTIRE FILE IS SUPERSEDED BY CutCornerBorder
    public class CutCornerInfo : DependencyObject
    {
        /*public Path Target
        {
            get => (Path)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(Path), typeof(CutCornerInfo), new PropertyMetadata(null));

        */public CornerRadius CornerDistance
        {
            get => (CornerRadius)GetValue(CornerDistanceProperty);
            set => SetValue(CornerDistanceProperty, value);
        }

        public static readonly DependencyProperty CornerDistanceProperty =
            DependencyProperty.Register("CornerDistance", typeof(CornerRadius), typeof(CutCornerInfo), new PropertyMetadata(new CornerRadius(5)));

        public double Width
        {
            get => (double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public static DependencyProperty WidthProperty =
            DependencyProperty.RegisterAttached("Width", typeof(double), typeof(CutCornerInfo), new FrameworkPropertyMetadata((double)100, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Height
        {
            get => (double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public static DependencyProperty HeightProperty =
            DependencyProperty.RegisterAttached("Height", typeof(double), typeof(CutCornerInfo), new FrameworkPropertyMetadata((double)100, FrameworkPropertyMetadataOptions.AffectsRender));
    }


    public class CutCornerGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var geom = Geometry.Parse("M 0 " + (parameter as CutCornerInfo).CornerDistance.TopLeft.ToString() + " L " + (parameter as CutCornerInfo).CornerDistance.TopLeft.ToString() + " 0 " +
                "L " + ((parameter as CutCornerInfo).Width - (parameter as CutCornerInfo).CornerDistance.TopRight).ToString() + " 0 L " + (parameter as CutCornerInfo).Width.ToString() + " " + (parameter as CutCornerInfo).CornerDistance.TopRight.ToString() + " " +
                "L " + (parameter as CutCornerInfo).Width.ToString() + " " + ((parameter as CutCornerInfo).Height - (parameter as CutCornerInfo).CornerDistance.BottomRight).ToString() + " L " + ((parameter as CutCornerInfo).Width - (parameter as CutCornerInfo).CornerDistance.BottomRight).ToString() + " " + (parameter as CutCornerInfo).Height.ToString() + " " +
                "L " + (parameter as CutCornerInfo).CornerDistance.BottomLeft.ToString() + " " + (parameter as CutCornerInfo).Height.ToString() + " L 0 " + ((parameter as CutCornerInfo).Height - (parameter as CutCornerInfo).CornerDistance.BottomLeft).ToString()
                + " Z");
            return geom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}