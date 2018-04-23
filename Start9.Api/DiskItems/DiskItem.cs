using Start9.Api.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Icon = System.Drawing.Icon;
using static Start9.Api.SystemScaling;

namespace Start9.Api.DiskItems
{
    public partial class DiskItem : DependencyObject
    {
        public static List<DiskItem> AllApps
        {
            get
            {
                List<DiskItem> items = new List<DiskItem>();

                List<DiskItem> AllAppsAppDataItems = new List<DiskItem>();
                foreach (string s in Directory.EnumerateFiles(Environment.ExpandEnvironmentVariables(@"%appdata%\Microsoft\Windows\Start Menu\Programs")))
                {
                    AllAppsAppDataItems.Add(new DiskItem(s));
                }
                foreach (string s in Directory.EnumerateDirectories(Environment.ExpandEnvironmentVariables(@"%appdata%\Microsoft\Windows\Start Menu\Programs")))
                {
                    AllAppsAppDataItems.Add(new DiskItem(s));
                }

                List<DiskItem> AllAppsProgramDataItems = new List<DiskItem>();
                foreach (string s in Directory.EnumerateFiles(Environment.ExpandEnvironmentVariables(@"%programdata%\Microsoft\Windows\Start Menu\Programs")))
                {
                    AllAppsProgramDataItems.Add(new DiskItem(s));
                }
                foreach (string s in Directory.EnumerateDirectories(Environment.ExpandEnvironmentVariables(@"%programdata%\Microsoft\Windows\Start Menu\Programs")))
                {
                    AllAppsProgramDataItems.Add(new DiskItem(s));
                }

                List<DiskItem> AllAppsItems = new List<DiskItem>();
                List<DiskItem> AllAppsReorgItems = new List<DiskItem>();
                foreach (DiskItem t in AllAppsAppDataItems)
                {
                    bool FolderIsDuplicate = false;

                    foreach (DiskItem v in AllAppsProgramDataItems)
                    {
                        List<DiskItem> SubItemsList = new List<DiskItem>();

                        if (Directory.Exists(t.ItemPath))
                        {
                            if (((t.ItemType == DiskItemType.Directory) & (v.ItemType == DiskItemType.Directory)) && ((t.ItemPath.Substring(t.ItemPath.LastIndexOf(@"\"))) == (v.ItemPath.Substring(v.ItemPath.LastIndexOf(@"\")))))
                            {
                                FolderIsDuplicate = true;
                                foreach (string i in Directory.EnumerateDirectories(t.ItemPath))
                                {
                                    SubItemsList.Add(new DiskItem(i));
                                }

                                foreach (string j in Directory.EnumerateFiles(v.ItemPath))
                                {
                                    SubItemsList.Add(new DiskItem(j));
                                }
                            }
                        }

                        if (!AllAppsItems.Contains(v))
                        {
                            AllAppsItems.Add(v);
                        }
                    }

                    if ((!AllAppsItems.Contains(t)) && (!FolderIsDuplicate))
                    {
                        AllAppsItems.Add(t);
                    }
                }

                foreach (DiskItem x in AllAppsItems)
                {
                    if (File.Exists(x.ItemPath))
                    {
                        AllAppsReorgItems.Add(x);
                    }
                }

                foreach (DiskItem x in AllAppsItems)
                {
                    if (Directory.Exists(x.ItemPath))
                    {
                        AllAppsReorgItems.Add(x);
                    }
                }

                return AllAppsReorgItems;
            }
        }

        public string ItemName
        {
            get
            {
                if (ItemType == DiskItemType.Directory)
                {
                    return Path.GetFileName(ItemPath);
                }
                else if (ItemType == DiskItemType.File)
                {
                    return Path.GetFileName(ItemPath);
                }
                else
                {
                    return "If you see this text, remind me to look into getting Apps' display names.";
                }
            }
        }/*

        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(DiskItem), new PropertyMetadata());*/

        public string ItemPath
        {
            get => (string)GetValue(ItemPathProperty);
            set => SetValue(ItemPathProperty, (value));
        }

        public static readonly DependencyProperty ItemPathProperty =
            DependencyProperty.Register("ItemPath", typeof(string), typeof(DiskItem), new PropertyMetadata(""));


        /*public Icon ItemIcon
        {
            get
            {
                return GetIconFromFilePath(ItemPath);
            }
        }*/

        public enum DiskItemType
        {
            File,
            Directory,
            App
        }

        public DiskItemType ItemType
        {
            get => (DiskItemType)GetValue(ItemTypeProperty);
            /*{
                if ((File.Exists(Path) | (Directory.Exists(Path)) && (true /*if no user-specified icon override is present*)))
                {
                    return MiscTools.GetIconFromFilePath(Path);
                }
                else
                {
                    return null;
                }
            }*/
            set => SetValue(ItemTypeProperty, value);
        }

        public static readonly DependencyProperty ItemTypeProperty =
            DependencyProperty.Register("ItemType", typeof(DiskItemType), typeof(DiskItem), new PropertyMetadata(DiskItemType.Directory));

        /*public ImageSource RealIcon
        {
            get => MiscTools.GetIconFromFilePath(Path);
            set => SetValue(RealIconProperty, MiscTools.GetIconFromFilePath(Path));
        }

        public static readonly DependencyProperty RealIconProperty =
            DependencyProperty.Register("RealIcon", typeof(ImageSource), typeof(DiskItem), new PropertyMetadata());*/

        public ObservableCollection<DiskItem> SubItems
        {
            get
            {
                ObservableCollection<DiskItem> items = new ObservableCollection<DiskItem>();
                if (ItemType == DiskItemType.Directory)
                {
                    foreach (string s in Directory.EnumerateDirectories(ItemPath))
                    {
                        items.Add(new DiskItem(s));
                    }
                    foreach (string s in Directory.EnumerateFiles(ItemPath))
                    {
                        items.Add(new DiskItem(s));
                    }
                }
                return items;
            }
        }

        /*public static readonly DependencyProperty SubItemsProperty =
            DependencyProperty.Register("SubItems", typeof(ObservableCollection<DiskItem>), typeof(DiskItem), new PropertyMetadata());*/

        public DiskItem(string path)
        {
            ItemPath = path;
            if (File.Exists(ItemPath))
            {
                ItemType = DiskItemType.File;
            }
        }

        /*Icon GetIconFromFilePath(string path)
        {
            return GetIconFromFilePath(path, 48, 0x000000000 | 0x100);
        }

        Icon GetIconFromFilePath(string path, int size)
        {
            return GetIconFromFilePath(path, size, 0x000000000 | 0x100);
        }

        Icon GetIconFromFilePath(string path, uint flags)
        {
            return GetIconFromFilePath(path, 48, flags);
        }*/
    }

    public class DiskItemToIconImageBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DiskItem info = value as DiskItem;
            int size = int.Parse(parameter.ToString());
            IconOverride over = null;
            foreach (IconOverride i in IconPref.FileIconOverrides)
            {
                if (
                    (Environment.ExpandEnvironmentVariables(i.TargetName) == Environment.ExpandEnvironmentVariables(info.ItemPath))
                    |
                    (
                    (!i.IsFullPath)
                    && Path.GetFileName(Environment.ExpandEnvironmentVariables(i.TargetName)) == Path.GetFileName(Environment.ExpandEnvironmentVariables(info.ItemPath)))
                    )
                {
                    over = i;
                }
            }

            if (over != null)
            {
                return over.ReplacementBrush;
                //BitmapSource source = BitmapSource.Create()
            }
            else
            {
                uint flags = (0x00000000 | 0x100);
                if (size <= 20)
                {
                    flags = (0x00000001 | 0x100);
                }
                Icon target = GetIconFromFilePath(info.ItemPath, size, flags);
                System.Windows.Media.ImageSource entryIconImageSource = Imaging.CreateBitmapSourceFromHIcon(
                target.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(System.Convert.ToInt32(RealPixelsToWpfUnits(size)), System.Convert.ToInt32(RealPixelsToWpfUnits(size)))
                );
                return new ImageBrush(entryIconImageSource);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        Icon GetIconFromFilePath(string path, int size, uint flags)
        {
            WinApi.ShFileInfo shInfo = new WinApi.ShFileInfo();
            WinApi.SHGetFileInfo(path, 0, ref shInfo, (uint)Marshal.SizeOf(shInfo), flags);
            System.Drawing.Icon entryIcon = System.Drawing.Icon.FromHandle(shInfo.hIcon);
            return entryIcon;
        }
    }
}