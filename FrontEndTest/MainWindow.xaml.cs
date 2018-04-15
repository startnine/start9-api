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
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace FrontEndTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : PlexWindow
    {
        public MainWindow()
        {
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FileIconOverrides.ItemsSource = IconPref.FileIconOverrides;
        }


        /*public MainWindow(bool f)
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
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

        private void MainWindow_oldLoaded(object sender, RoutedEventArgs e)
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
        }*/

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

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Start9.Api.Plex.MessageBox.Show("Yes, I'm working on these for some reason.\n\n...blame Fleccy :P", "This is a Plex MessageBox");
        }
    }/*

    public class ReplacementIconNameToReplacedCanvasConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string source = value.ToString();
            return new Canvas()
            {
                Width = 48,
                Height = 48,
                Background = new ImageBrush(new BitmapImage(new Uri(Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\IconOverrides\" + source), UriKind.RelativeOrAbsolute)))
            };
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }*/

    class ReplacementIconNameToOriginalCanvasConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string source = value.ToString();
            WinApi.ShFileInfo shInfo = new WinApi.ShFileInfo();
            WinApi.SHGetFileInfo(Environment.ExpandEnvironmentVariables(source), 0, ref shInfo, (uint)Marshal.SizeOf(shInfo), 0x000000000 | 0x100);
            System.Drawing.Icon entryIcon = System.Drawing.Icon.FromHandle(shInfo.hIcon);
            ImageSource entryIconImageSource = Imaging.CreateBitmapSourceFromHIcon(
            entryIcon.Handle,
            Int32Rect.Empty,
            BitmapSizeOptions.FromWidthAndHeight(System.Convert.ToInt32(DpiManager.ConvertPixelsToWpfUnits(48)), System.Convert.ToInt32(DpiManager.ConvertPixelsToWpfUnits(48)))
            );
            return new Canvas()
            {
                Width = 48,
                Height = 48,
                Background = new ImageBrush(entryIconImageSource)  //MiscTools.GetIconFromFilePath(Environment.ExpandEnvironmentVariables(source)))
            };
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
