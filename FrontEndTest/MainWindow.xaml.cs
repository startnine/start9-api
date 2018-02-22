using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Start9.Api.Plex;

namespace FrontEndTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : PlexWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += delegate
            {
                Resources["TestImageBrush"] = new ImageBrush(new BitmapImage(new Uri(Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\TestImage.png"), UriKind.RelativeOrAbsolute)));
            };
        }
    }
}
