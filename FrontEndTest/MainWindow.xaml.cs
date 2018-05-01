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
using Start9.Api;
using Start9.Api.AppBar;
using Start9.Api.Plex;
using Start9.Api.Tools;
using Start9.Api.DiskItems;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using static Start9.Api.SystemScaling;
using AppInfo = Start9.Api.Appx.AppInfo;
using System.Collections.ObjectModel;

namespace FrontEndTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : PlexWindow
    {
        [DllImport("dwmapi.dll")]
        static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

        [StructLayout(LayoutKind.Sequential)]
        struct DWM_BLURBEHIND
        {
            public DWM_BB dwFlags;
            public Boolean fEnable;
            public IntPtr hRgnBlur;
            public Boolean fTransitionOnMaximized;

            public DWM_BLURBEHIND(Boolean enabled)
            {
                fEnable = enabled ? true : false;
                hRgnBlur = IntPtr.Zero;
                fTransitionOnMaximized = false;
                dwFlags = DWM_BB.Enable;
            }

            public System.Drawing.Region Region
            {
                get { return System.Drawing.Region.FromHrgn(hRgnBlur); }
            }

            public Boolean TransitionOnMaximized
            {
                get { return fTransitionOnMaximized != false; }
                set
                {
                    fTransitionOnMaximized = value ? true : false;
                    dwFlags |= DWM_BB.TransitionMaximized;
                }
            }

            public void SetRegion(System.Drawing.Graphics graphics, System.Drawing.Region region)
            {
                hRgnBlur = region.GetHrgn(graphics);
                dwFlags |= DWM_BB.BlurRegion;
            }
        }

        [Flags]
        enum DWM_BB
        {
            Enable = 1,
            BlurRegion = 2,
            TransitionMaximized = 4
        }

        IntPtr helper;

        public ObservableCollection<DiskItem> DiskItems
        {
            get => (ObservableCollection<DiskItem>)GetValue(DiskItemsProperty);
            set => SetValue(DiskItemsProperty, value);
        }

        public static readonly DependencyProperty DiskItemsProperty =
            DependencyProperty.Register("DiskItems", typeof(ObservableCollection<DiskItem>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<DiskItem>()));

        public MainWindow()
        {
            InitializeComponent();
            new Window()
            {
                Width = 512,
                Height = 512,
                Left = 100,
                Top = 100,
                Topmost = true,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = SystemContext.WindowGlassColor
            }.Show();
            ResizeMode = PlexResizeMode.NoResize;
            AllowsTransparency = false;
            Loaded += MainWindow_Loaded;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            helper = new WindowInteropHelper(this).EnsureHandle();
            base.Background = new SolidColorBrush(Colors.Transparent);
            Background = new SolidColorBrush(Colors.Transparent);
            var hs = HwndSource.FromHwnd(helper);
            hs.CompositionTarget.BackgroundColor = System.Windows.Media.Colors.Transparent;
            base.OnSourceInitialized(e);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DWM_BLURBEHIND blur = new DWM_BLURBEHIND()
            {
                dwFlags = DWM_BB.Enable,
                fEnable = true,
                hRgnBlur = IntPtr.Zero
            };
            DwmEnableBlurBehindWindow(helper, ref blur);
            TestTreeView.ItemsSource = new DiskItem(Environment.ExpandEnvironmentVariables(@"%userprofile%\Pictures")).SubItems;
            //FileIconOverrides.ItemsSource = IconPref.FileIconOverrides;
            DiskItem item = new DiskItem("Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe");
            item.ItemAppInfo.NotificationReceived += (sneder, args) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    /*TestTileStackPanel.Children.Clear();
                    foreach (ImageBrush i in args.NewNotification.Images)
                    {
                        Canvas c = new Canvas()
                        {
                            Width = 100,
                            Height = 100,
                            Background = i
                        };
                        TestTileStackPanel.Children.Add(c);
                    }
                    */
                    foreach (string s in args.NewNotification.Text)
                    {
                        /*TextBlock t = new TextBlock()
                        {
                            Text = s
                        };
                        TestTileStackPanel.Children.Add(t);*/
                        Debug.WriteLine(s);
                    }
                }));
            };
            DiskItems.Add(item);
            AppBarWindow appBar = new AppBarWindow()
            {
                DockedWidthOrHeight = 100
            };

            Button sideButton = new Button()
            {
                Content = "CHANGE SIDE"
            };

            Button monitorButton = new Button()
            {
                Content = "CHANGE MONITOR"
            };
            monitorButton.Click += (sneder, args) =>
            {
                if (appBar.Monitor == MonitorInfo.AllMonitors[0] && (MonitorInfo.AllMonitors.Count > 1))
                {
                    appBar.Monitor = MonitorInfo.AllMonitors[1];
                }
                else if (MonitorInfo.AllMonitors.Count > 1)
                {
                    appBar.Monitor = MonitorInfo.AllMonitors[0];
                }
            };

            Button closeButton = new Button()
            {
                Content = "CLOSE"
            };
            closeButton.Click += (sneder, args) =>
            {
                appBar.Close();
            };

            appBar.Content = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Children = { sideButton, closeButton }
            };
            sideButton.Click += (sneder, args) =>
            {
                var stack = (appBar.Content as StackPanel);

                if (appBar.DockMode == AppBarDockMode.Bottom)
                {
                    appBar.DockMode = AppBarDockMode.Left;
                    stack.Orientation = Orientation.Vertical;
                }
                else if (appBar.DockMode == AppBarDockMode.Left)
                {
                    appBar.DockMode = AppBarDockMode.Top;
                    stack.Orientation = Orientation.Horizontal;
                }
                else if (appBar.DockMode == AppBarDockMode.Top)
                {
                    appBar.DockMode = AppBarDockMode.Right;
                    stack.Orientation = Orientation.Vertical;
                }
                else
                {
                    appBar.DockMode = AppBarDockMode.Bottom;
                    stack.Orientation = Orientation.Horizontal;
                }
            };
            if (MonitorInfo.AllMonitors.Count > 1)
            {
                (appBar.Content as StackPanel).Children.Add(monitorButton);
            }

            appBar.Show();
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
            string p = Path.GetFileNameWithoutExtension(d.ItemPath);
            return new TreeViewItem()
            {
                Tag = d,
                Header = p,
                Style = (Style)Resources[typeof(TreeViewItem)]
            };
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Start9.Api.Plex.MessageBox.Show(this, "Yes, I'm working on these for some reason.\n\n...blame Fleccy :P", "This is a Plex MessageBox");
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
            BitmapSizeOptions.FromWidthAndHeight(System.Convert.ToInt32(RealPixelsToWpfUnits(48)), System.Convert.ToInt32(RealPixelsToWpfUnits(48)))
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
