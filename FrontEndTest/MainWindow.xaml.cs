using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Start9.Api.Plex;

namespace FrontEndTest
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : PlexWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			//Loaded += (sender, args) => Resources["TestImageBrush"] = new ImageBrush(
			//	new BitmapImage(new Uri(Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\TestImage.png"),
			//		UriKind.RelativeOrAbsolute)));
		}
	}
}