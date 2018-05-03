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
using AppInfo = Start9.Api.Appx.AppInfo;
using static Start9.Api.WinApi;

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
                if ((ItemType == DiskItemType.Directory) | (ItemType == DiskItemType.File))
                {
                    return Path.GetFileName(ItemPath);
                }
                else
                {
                    return ItemAppInfo.DisplayName;// "If you see this text, remind me to look into getting Apps' display names.";
                }
            }
        }/*

        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(DiskItem), new PropertyMetadata());*/

        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(bool), typeof(DiskItem), new PropertyMetadata(false));

        public string ItemPath
        {
            get
            {
                if (ItemType == DiskItemType.Shortcut)
                {
                    string raw = (string)GetValue(ItemPathProperty);

                    string targetPath = GetMsiShortcut(raw);

                    if (targetPath == null)
                    {
                        targetPath = ResolveShortcut(raw);
                    }

                    if (targetPath == null)
                    {
                        targetPath = GetInternetShortcut(raw);
                    }

                    if (targetPath == null | targetPath == "" | targetPath.Replace(" ", "") == "")
                    {
                        return raw;
                    }
                    else
                    {
                        return targetPath;
                    }
                }
                else return (string)GetValue(ItemPathProperty);
            }
            set => SetValue(ItemPathProperty, (value));
        }

        public static readonly DependencyProperty ItemPathProperty =
            DependencyProperty.Register("ItemPath", typeof(string), typeof(DiskItem), new PropertyMetadata(""));

        string GetInternetShortcut(string _rawPath)
        {
            try
            {
                string url = "";

                using (TextReader reader = new StreamReader(_rawPath))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("URL="))
                        {
                            string[] splitLine = line.Split('=');
                            if (splitLine.Length > 0)
                            {
                                url = splitLine[1];
                                break;
                            }
                        }
                    }
                }
                return url;
            }
            catch
            {
                return null;
            }
        }

        string ResolveShortcut(string _rawPath)
        {
            // IWshRuntimeLibrary is in the COM library "Windows Script Host Object Model"
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

            try
            {
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(_rawPath);
                return shortcut.TargetPath;
            }
            catch
            {
                // A COMException is thrown if the file is not a valid shortcut (.lnk) file 
                return null;
            }
        }

        string GetMsiShortcut(string _rawPath)
        {
            StringBuilder product = new StringBuilder(WinApi.MaxGuidLength + 1);
            StringBuilder feature = new StringBuilder(WinApi.MaxFeatureLength + 1);
            StringBuilder component = new StringBuilder(WinApi.MaxGuidLength + 1);

            WinApi.MsiGetShortcutTarget(_rawPath, product, feature, component);

            int pathLength = WinApi.MaxPathLength;
            StringBuilder path = new StringBuilder(pathLength);

            WinApi.InstallState installState = WinApi.MsiGetComponentPath(product.ToString(), component.ToString(), path, ref pathLength);
            if (installState == WinApi.InstallState.Local)
            {
                return path.ToString();
            }
            else
            {
                return null;
            }
        }

        public AppInfo ItemAppInfo
        {
            get => (AppInfo)GetValue(ItemAppInfoProperty);
            set => SetValue(ItemAppInfoProperty, (value));
        }

        public static readonly DependencyProperty ItemAppInfoProperty =
            DependencyProperty.Register("ItemAppInfo", typeof(AppInfo), typeof(DiskItem), new PropertyMetadata(null));


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
            Shortcut,
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

        public string FriendlyItemType
        {
            get => (string)GetValue(FriendlyItemTypeProperty);
            set => SetValue(FriendlyItemTypeProperty, value);
        }

        public static readonly DependencyProperty FriendlyItemTypeProperty =
            DependencyProperty.Register("FriendlyItemType", typeof(string), typeof(DiskItem), new PropertyMetadata(""));

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

        ShFileInfo _fileInfo = new ShFileInfo();

        public DiskItem(string path)
        {
            ItemPath = path;
            if (File.Exists(ItemPath))
            {
                if (Path.GetExtension(path).EndsWith("lnk"))
                {
                    ItemType = DiskItemType.Shortcut;
                }
                ItemType = DiskItemType.File;
            }
            else if (!(Directory.Exists(ItemPath)))
            {
                if (Directory.Exists(Environment.ExpandEnvironmentVariables(@"%programfiles%\WindowsApps\" + path)))
                {
                    ItemType = DiskItemType.App;
                    ItemAppInfo = new AppInfo(path);
                }
            }

            if (ItemType == DiskItemType.File & File.Exists(path))
            {
                ///Fix this or something
                /*try
                {
                    SHGetFileInfo(path, (uint)(0x00000080), ref _fileInfo, (uint)Marshal.SizeOf(_fileInfo), (uint)(0x000000400 | 0x000000010));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }*/

                if (string.IsNullOrEmpty(_fileInfo.szTypeName))
                {
                    FriendlyItemType = Path.GetExtension(ItemPath).Replace(".", "").ToUpper() + " File";
                }
                else
                {
                    FriendlyItemType = _fileInfo.szTypeName;
                }
            }
            else if (ItemType == DiskItemType.Directory)// Directory.Exists(ItemPath))
            {
                FriendlyItemType = "File Folder";
            }
            else if (ItemType == DiskItemType.Shortcut)
            {
                FriendlyItemType = "Shortcut";
            }
            else if (ItemType == DiskItemType.App)
            {
                if (Environment.OSVersion.Version.Major >= 10)
                {
                    FriendlyItemType = "Universal App";
                }
                else
                {
                    FriendlyItemType = "Modern App";
                }
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

    static class DiskItemToIconShared
    {
        static Icon GetIconFromFilePath(string path, int size, uint flags)
        {
            ShFileInfo shInfo = new ShFileInfo();
            SHGetFileInfo(path, 0, ref shInfo, (uint)Marshal.SizeOf(shInfo), flags);
            System.Drawing.Icon entryIcon = System.Drawing.Icon.FromHandle(shInfo.hIcon);
            return entryIcon;
        }

        public static ImageBrush GetImageBrush(object value, Type targetType, object parameter, CultureInfo culture)
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
            }
            else if (info.ItemType != DiskItem.DiskItemType.App)
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
            else
            {
                return info.ItemAppInfo.Icon;
            }
        }
    }

    public class DiskItemToIconImageBrushOrThumbnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DiskItem info = value as DiskItem;
            ImageBrush returnValue = DiskItemToIconShared.GetImageBrush(value, targetType, parameter, culture);

            bool isThumbnailable = false;
            List<string> extensions = new List<string> { "png", "jpg", "bmp" };
            foreach (string e in extensions)
            {
                if (Path.GetExtension(info.ItemPath).EndsWith(e))
                {
                    isThumbnailable = true;
                    break;
                }
            }

            if (isThumbnailable)
            {
                try
                {
                    returnValue = new ImageBrush(new BitmapImage(new Uri(info.ItemPath, UriKind.RelativeOrAbsolute)));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DiskItemToIconImageBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DiskItem info = value as DiskItem;
            return DiskItemToIconShared.GetImageBrush(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}