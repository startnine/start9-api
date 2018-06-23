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

        public Double Width
        {
            get => (Double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public static DependencyProperty WidthProperty =
            DependencyProperty.RegisterAttached("Width", typeof(Double), typeof(CutCornerInfo), new FrameworkPropertyMetadata((Double)100, FrameworkPropertyMetadataOptions.AffectsRender));

        public Double Height
        {
            get => (Double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public static DependencyProperty HeightProperty =
            DependencyProperty.RegisterAttached("Height", typeof(Double), typeof(CutCornerInfo), new FrameworkPropertyMetadata((Double)100, FrameworkPropertyMetadataOptions.AffectsRender));
    }


    public class CutCornerGeometryConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var geom = Geometry.Parse("M 0 " + (parameter as CutCornerInfo).CornerDistance.TopLeft.ToString() + " L " + (parameter as CutCornerInfo).CornerDistance.TopLeft.ToString() + " 0 " +
                "L " + ((parameter as CutCornerInfo).Width - (parameter as CutCornerInfo).CornerDistance.TopRight).ToString() + " 0 L " + (parameter as CutCornerInfo).Width.ToString() + " " + (parameter as CutCornerInfo).CornerDistance.TopRight.ToString() + " " +
                "L " + (parameter as CutCornerInfo).Width.ToString() + " " + ((parameter as CutCornerInfo).Height - (parameter as CutCornerInfo).CornerDistance.BottomRight).ToString() + " L " + ((parameter as CutCornerInfo).Width - (parameter as CutCornerInfo).CornerDistance.BottomRight).ToString() + " " + (parameter as CutCornerInfo).Height.ToString() + " " +
                "L " + (parameter as CutCornerInfo).CornerDistance.BottomLeft.ToString() + " " + (parameter as CutCornerInfo).Height.ToString() + " L 0 " + ((parameter as CutCornerInfo).Height - (parameter as CutCornerInfo).CornerDistance.BottomLeft).ToString()
                + " Z");
            return geom;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}