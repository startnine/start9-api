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
using System.IO;
using Start9.Api.Plex;
using Start9.Api.Tools;
using Start9.Api.DiskFiles;
using System.Diagnostics;

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
            Loaded += MainWindow_Loaded;
            //@"http://{language}.appex-rf.msn.com/cgtile/v1/{language}/News/Today.xml"
            //Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe
            //AppxTools.NotificationInfo notify;// = AppxTools.GetLiveTileNotification(AppxTools.GetUpdateAddressFromApp("Microsoft.BingSports_3.0.4.212_x64__8wekyb3d8bbwe"));
            AppxTools.TileInfo Info = new AppxTools.TileInfo("Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe");
            Info.NotificationReceived += (object sneder, AppxTools.NotificationInfoEventArgs args) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    TileTestStackPanel.Children.Clear();
                    foreach (ImageBrush i in args.NewNotification.Images)
                    {
                        Canvas c = new Canvas()
                        {
                            Width = 100,
                            Height = 100,
                            Background = i
                        };
                        TileTestStackPanel.Children.Add(c);
                    }

                    foreach (string s in args.NewNotification.Text)
                    {
                        TextBlock t = new TextBlock()
                        {
                            Text = s
                        };
                        TileTestStackPanel.Children.Add(t);
                    }
                }));
            };
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Resources["TestImageBrush"] = new ImageBrush(new BitmapImage(new Uri(Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\TestImage.png"), UriKind.RelativeOrAbsolute)));

            foreach (DiskItem d in DiskItem.AllApps)
            {
                TreeViewItem t = GetTreeViewItemFromDiskItem(d);
                if (t.Tag.GetType().Name.Contains("DiskFolder"))
                {
                    t.Expanded += (object sneder, RoutedEventArgs args) =>
                    {
                        foreach (DiskItem i in (t.Tag as DiskFolder).SubItems)
                        {
                            t.Items.Add(GetTreeViewItemFromDiskItem(i));
                        }
                    };

                    t.MouseDoubleClick += (object sneder, MouseButtonEventArgs args) =>
                    {
                        Process.Start((t.Tag as DiskFolder).Path);
                    };
                }
                else
                {
                    t.Expanded += (object sneder, RoutedEventArgs args) =>
                    {
                        Process.Start((t.Tag as DiskItem).Path);
                    };
                }
                AllAppsTree.Items.Add(d);
            }
        }

        private TreeViewItem GetTreeViewItemFromDiskItem(DiskItem d)
        {
            string p = Path.GetFileNameWithoutExtension(d.Path);
            return new TreeViewItem()
            {
                Tag = d,
                Header = p,
                Style = (Style)Resources[typeof(TreeViewItem)]
            };
        }
    }
}
