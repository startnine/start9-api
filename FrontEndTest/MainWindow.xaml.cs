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
using Start9.Api.Tools;

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
            //@"http://{language}.appex-rf.msn.com/cgtile/v1/{language}/News/Today.xml"
            //Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe
            AppxTools.NotificationInfo notify = AppxTools.GetLiveTileNotification(AppxTools.GetUpdateAddressFromApp("Microsoft.BingSports_3.0.4.212_x64__8wekyb3d8bbwe"));

            foreach (ImageBrush i in notify.Images)
            {
                Canvas c = new Canvas()
                {
                    Width = 100,
                    Height = 100,
                    Background = i
                };
                TileTestStackPanel.Children.Add(c);
            }

            foreach (string s in notify.Text)
            {
                TextBlock t = new TextBlock()
                {
                    Text = s
                };
                TileTestStackPanel.Children.Add(t);
            }
        }
    }
}
