using Start9.Api.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Start9.Api.DiskFiles
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
                    AllAppsAppDataItems.Add(new DiskFile(s));
                }
                foreach (string s in Directory.EnumerateDirectories(Environment.ExpandEnvironmentVariables(@"%appdata%\Microsoft\Windows\Start Menu\Programs")))
                {
                    AllAppsAppDataItems.Add(new DiskFolder(s));
                }

                List<DiskItem> AllAppsProgramDataItems = new List<DiskItem>();
                foreach (string s in Directory.EnumerateFiles(Environment.ExpandEnvironmentVariables(@"%programdata%\Microsoft\Windows\Start Menu\Programs")))
                {
                    AllAppsProgramDataItems.Add(new DiskFile(s));
                }
                foreach (string s in Directory.EnumerateDirectories(Environment.ExpandEnvironmentVariables(@"%programdata%\Microsoft\Windows\Start Menu\Programs")))
                {
                    AllAppsProgramDataItems.Add(new DiskFolder(s));
                }

                List<DiskItem> AllAppsItems = new List<DiskItem>();
                List<DiskItem> AllAppsReorgItems = new List<DiskItem>();

                new UIElement().Dispatcher.Invoke(new Action(() =>
                {
                    foreach (DiskItem t in AllAppsAppDataItems)
                    {
                        bool FolderIsDuplicate = false;

                        foreach (DiskItem v in AllAppsProgramDataItems)
                        {
                            List<DiskItem> SubItemsList = new List<DiskItem>();

                            if (Directory.Exists(t.Path))
                            {
                                if (((t is DiskFolder) & (v is DiskFolder)) && ((t.Path.Substring(t.Path.LastIndexOf(@"\"))) == (v.Path.Substring(v.Path.LastIndexOf(@"\")))))
                                {
                                    FolderIsDuplicate = true;
                                    foreach (DiskItem i in (t as DiskFolder).SubItems)
                                    {
                                        SubItemsList.Add(i);
                                    }

                                    foreach (DiskItem j in (v as DiskFolder).SubItems)
                                    {
                                        SubItemsList.Add(j);
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
                        if (File.Exists(x.Path))
                        {
                            AllAppsReorgItems.Add(x);
                        }
                    }

                    foreach (DiskItem x in AllAppsItems)
                    {
                        if (Directory.Exists(x.Path))
                        {
                            AllAppsReorgItems.Add(x);
                        }
                    }
                }));

                return AllAppsReorgItems;
            }
        }

        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, (value));
        }

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(DiskItem), new PropertyMetadata());


        public ImageSource Icon
        {
            get
            {
                if ((File.Exists(Path) | (Directory.Exists(Path)) && (true /*if no user-specified icon override is present*/)))
                {
                    return MiscTools.GetIconFromFilePath(Path);
                }
                else
                {
                    return null;
                }
            }
            set => SetValue(IconProperty, MiscTools.GetIconFromFilePath(Path));
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(DiskItem), new PropertyMetadata());

        public ImageSource RealIcon
        {
            get => MiscTools.GetIconFromFilePath(Path);
            set => SetValue(RealIconProperty, MiscTools.GetIconFromFilePath(Path));
        }

        public static readonly DependencyProperty RealIconProperty =
            DependencyProperty.Register("RealIcon", typeof(ImageSource), typeof(DiskItem), new PropertyMetadata());

        public DiskItem(string path)
        {
            Path = path;
        }
    }

    public partial class DiskFile : DiskItem
    {
        new public string Path
        {
            get
            {
                string f = (string)GetValue(PathProperty);
                if (System.IO.Path.GetExtension(f).Contains("lnk"))
                {
                    return ShortcutTools.GetTargetPath(f);
                }
                else
                {
                    return f;
                }
            }
            set
            {
                if (System.IO.Path.GetExtension(value).Contains("lnk"))
                {
                    SetValue(PathProperty, ShortcutTools.GetTargetPath(value));
                }
                else
                {
                    SetValue(PathProperty, value);
                }
            }
        }

        new public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(DiskFile), new PropertyMetadata());

        public DiskFile(string path) : base(path)
        {
            Path = path;
        }
    }

    public partial class DiskFolder : DiskItem
    {
        public List<DiskItem> SubItems
        {
            get
            {
                List<DiskItem> items = new List<DiskItem>();
                foreach (string s in Directory.EnumerateFiles(Path))
                {
                    items.Add(new DiskFile(s));
                }
                foreach (string s in Directory.EnumerateDirectories(Path))
                {
                    items.Add(new DiskFolder(s));
                }
                return items;
            }
            set { }
        }

        public static readonly DependencyProperty SubItemsProperty =
            DependencyProperty.Register("SubItems", typeof(List<DiskItem>), typeof(DiskFolder), new PropertyMetadata());

        public DiskFolder(string path) : base(path)
        {
            Path = path;
        }
    }
}