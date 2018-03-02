using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Start9.Api.Objects
{
    public class CutCornerInfo : DependencyObject
    {
        public CornerRadius CornerDistance
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

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(CutCornerInfo), new PropertyMetadata((double)100));

        public double Height
        {
            get => (double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double), typeof(CutCornerInfo), new PropertyMetadata((double)100));
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
