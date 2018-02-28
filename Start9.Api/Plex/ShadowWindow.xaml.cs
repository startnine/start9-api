using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using Start9.Api.Tools;

namespace Start9.Api.Plex
{
	/// <summary>
	///     Interaction logic for ShadowWindow.xaml
	/// </summary>
	public partial class ShadowWindow : Window
	{
		public PlexWindow PlexWindow;

		public ShadowWindow(PlexWindow window)
		{
			Opacity = 0;
			InitializeComponent();
			PlexWindow = window;

			var renderTransformBinding = new Binding
			{
				Source = PlexWindow,
				Path = new PropertyPath("RenderTransform"),
				Mode = BindingMode.OneWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			BindingOperations.SetBinding(this, RenderTransformProperty, renderTransformBinding);

			var opacityBinding = new Binding
			{
				Source = PlexWindow,
				Path = new PropertyPath("Opacity"),
				Mode = BindingMode.OneWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			BindingOperations.SetBinding(ShadowGrid, OpacityProperty, opacityBinding);

			var shadowOpacityBinding = new Binding
			{
				Source = PlexWindow,
				Path = new PropertyPath("ShadowOpacity"),
				Mode = BindingMode.OneWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			BindingOperations.SetBinding(this, OpacityProperty, shadowOpacityBinding);

			var topmostBinding = new Binding
			{
				Source = PlexWindow,
				Path = new PropertyPath("Topmost"),
				Mode = BindingMode.OneWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			BindingOperations.SetBinding(this, TopmostProperty, topmostBinding);

			var visibilityBinding = new Binding
			{
				Source = PlexWindow,
				Path = new PropertyPath("Visibility"),
				Mode = BindingMode.OneWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			BindingOperations.SetBinding(this, VisibilityProperty, visibilityBinding);

			Loaded += (sender, e) => { PlexWindow.ShiftShadowBehindWindow(); };

			IsVisibleChanged += (sender, e) => { PlexWindow.ShiftShadowBehindWindow(); };
		}


		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			//Set the window style to noactivate.
			var helper = new WindowInteropHelper(this);
			WinApi.SetWindowLong(helper.Handle, WinApi.GwlExstyle,
				(int) WinApi.GetWindowLong(helper.Handle, WinApi.GwlExstyle) | 0x00000080 | 0x00000020);
			PlexWindow.ShiftShadowBehindWindow();
		}
	}

	/*public class RawOpacityToMultipliedOpacityConverter : IValueConverter
	{
	    public object Convert(object value, Type targetType,
	        object parameter, CultureInfo culture)
	    {
	        return (double)value * ((ShadowWindow)parameter).OpacityMultiplier;
	    }

	    public object ConvertBack(object value, Type targetType,
	        object parameter, CultureInfo culture)
	    {
	        return (double)value / ((ShadowWindow)parameter).OpacityMultiplier;
	    }
	}*/

	public class RawOpacityToMultipliedOpacityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return (double) values[0] * (double) values[1];
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}