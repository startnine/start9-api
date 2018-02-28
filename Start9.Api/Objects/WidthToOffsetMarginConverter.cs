using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Start9.Api.Properties;

namespace Start9.Api.Objects
{
	public class WidthToOffsetMarginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			var param = parameter.ToString();
			if (param == "Top")
				return new Thickness(0, (double) value, 0, (double) value * -1);
			if (param == "Right")
				return new Thickness((double) value * -1, 0, (double) value, 0);
			if (param == "Bottom")
				return new Thickness(0, (double) value * -1, 0, (double) value);
			return new Thickness((double) value, 0, (double) value * -1, 0);
		}

		public object ConvertBack(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			var param = parameter.ToString();
			if (param == "Top")
				return ((Thickness) value).Top;
			if (param == "Right")
				return ((Thickness) value).Right;
			if (param == "Bottom")
				return ((Thickness) value).Bottom;
			return ((Thickness) value).Left; //return new Thickness((double)value, 0, (double)value * -1, 0);
		}
	}

	public class WidthToHalfWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			if (parameter == null) return (double) value / 2;

			var paramString = (string) parameter;
			return (double) value / System.Convert.ToDouble(paramString);
		}

		public object ConvertBack(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			if (parameter == null) return (double) value * 2;

			var paramString = (string) parameter;
			return (double) value * System.Convert.ToDouble(paramString);
		}
	}

	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			if ((bool) value)
				return Visibility.Visible;
			return Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			if ((Visibility) value == Visibility.Visible)
				return true;
			return false;
		}
	}

	public class GetResourceForImageBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//.IceIconFrame
			var resource = Resources.IceIconFrame;

			/*foreach(object o in .ResourceManager)
			{

			}*/

			var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(resource.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions());

			var brushFromResource = new ImageBrush(bitmapSource);

			resource.Dispose();

			return brushFromResource.ImageSource;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class DependencyPoint : DependencyObject
	{
		public static readonly DependencyProperty XProperty =
			DependencyProperty.Register("X", typeof(double), typeof(DependencyPoint), new PropertyMetadata((double) 0));

		public static readonly DependencyProperty YProperty =
			DependencyProperty.Register("Y", typeof(double), typeof(DependencyPoint), new PropertyMetadata((double) 0));

		public double X
		{
			get => (double) GetValue(XProperty);
			set => SetValue(XProperty, value);
		}

		public double Y
		{
			get => (double) GetValue(YProperty);
			set => SetValue(YProperty, value);
		}
	}

	public class Dimensions : DependencyObject
	{
		public static readonly DependencyProperty BaseControlDimensionsProperty =
			DependencyProperty.Register("BaseControlDimensions", typeof(DependencyPoint), typeof(Dimensions),
				new PropertyMetadata(null));

		public static readonly DependencyProperty BaseControlThicknessProperty =
			DependencyProperty.Register("BaseControlThickness", typeof(Thickness), typeof(Dimensions),
				new PropertyMetadata(null));

		public static readonly DependencyProperty TargetControlDimensionsProperty =
			DependencyProperty.Register("TargetControlDimensions", typeof(DependencyPoint), typeof(Dimensions),
				new PropertyMetadata(null));

		public DependencyPoint BaseControlDimensions
		{
			get => (DependencyPoint) GetValue(BaseControlDimensionsProperty);
			set => SetValue(BaseControlDimensionsProperty, value);
		}


		public Thickness BaseControlThickness
		{
			get => (Thickness) GetValue(BaseControlThicknessProperty);
			set => SetValue(BaseControlThicknessProperty, value);
		}


		public DependencyPoint TargetControlDimensions
		{
			get => (DependencyPoint) GetValue(TargetControlDimensionsProperty);
			set => SetValue(TargetControlDimensionsProperty, value);
		}
	}


	public class MarginToRelativeMarginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var paramDim = (Dimensions) parameter;
			if ((paramDim.BaseControlDimensions != null) & (paramDim.TargetControlDimensions != null) &
			    (paramDim.BaseControlThickness != null))
			{
				var baseThickness = paramDim.BaseControlThickness;
				var baseControlDim = paramDim.BaseControlDimensions;
				var targetControlDim = paramDim.TargetControlDimensions;
				var resultThickness = new Thickness(baseThickness.Left / baseControlDim.X * targetControlDim.X,
					baseThickness.Top / baseControlDim.Y * targetControlDim.Y,
					baseThickness.Right / baseControlDim.X * targetControlDim.X,
					baseThickness.Bottom / baseControlDim.Y * targetControlDim.Y);
				Debug.WriteLine(resultThickness + "     " + baseControlDim.X + "," + baseControlDim.Y + "     " +
				                targetControlDim.X + "," + targetControlDim.Y);
				return resultThickness;
			}

			Debug.WriteLine("RETURNING BASETHICKNESS...");
			return paramDim.BaseControlThickness;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter == null) return (double) value * 2;

			var paramString = (string) parameter;
			return (double) value * System.Convert.ToDouble(paramString);
		}
	}
}