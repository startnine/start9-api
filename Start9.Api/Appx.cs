using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Timers;
using System.Windows;
using System.Collections.ObjectModel;

namespace Start9.Api
{
    public static class Appx
    {
        //https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Notifications.TileTemplateType
        public enum TileNotificationType
        {
            TileSquare150x150Block,
            TileSquare150x150IconWithBadge,
            TileSquare150x150Image,
            TileSquare150x150PeekImageAndText01,
            TileSquare150x150PeekImageAndText02,
            TileSquare150x150PeekImageAndText03,
            TileSquare150x150PeekImageAndText04,
            TileSquare150x150Text01,
            TileSquare150x150Text02,
            TileSquare150x150Text03,
            TileSquare150x150Text04,
            TileSquare310x310BlockAndText01,
            TileSquare310x310BlockAndText02,
            TileSquare310x310Image,
            TileSquare310x310ImageAndText01,
            TileSquare310x310ImageAndText02,
            TileSquare310x310ImageAndTextOverlay01,
            TileSquare310x310ImageAndTextOverlay02,
            TileSquare310x310ImageAndTextOverlay03,
            TileSquare310x310ImageCollection,
            TileSquare310x310ImageCollectionAndText01,
            TileSquare310x310ImageCollectionAndText02,
            TileSquare310x310SmallImageAndText01,
            TileSquare310x310SmallImagesAndTextList01,
            TileSquare310x310SmallImagesAndTextList02,
            TileSquare310x310SmallImagesAndTextList03,
            TileSquare310x310SmallImagesAndTextList04,
            TileSquare310x310SmallImagesAndTextList05,
            TileSquare310x310Text01,
            TileSquare310x310Text02,
            TileSquare310x310Text03,
            TileSquare310x310Text04,
            TileSquare310x310Text05,
            TileSquare310x310Text06,
            TileSquare310x310Text07,
            TileSquare310x310Text08,
            TileSquare310x310Text09,
            TileSquare310x310TextList01,
            TileSquare310x310TextList02,
            TileSquare310x310TextList03,
            TileSquareBlock,
            TileSquareImage,
            TileSquarePeekImageAndText01,
            TileSquarePeekImageAndText02,
            TileSquarePeekImageAndText03,
            TileSquarePeekImageAndText04,
            TileSquareText01,
            TileSquareText02,
            TileSquareText03,
            TileSquareText04,
            TileTall150x310Image,
            TileWide310x150BlockAndText01,
            TileWide310x150BlockAndText02,
            TileWide310x150IconWithBadgeAndText,
            TileWide310x150Image,
            TileWide310x150ImageAndText01,
            TileWide310x150ImageAndText02,
            TileWide310x150ImageCollection,
            TileWide310x150PeekImage01,
            TileWide310x150PeekImage02,
            TileWide310x150PeekImage03,
            TileWide310x150PeekImage04,
            TileWide310x150PeekImage05,
            TileWide310x150PeekImage06,
            TileWide310x150PeekImageAndText01,
            TileWide310x150PeekImageAndText02,
            TileWide310x150PeekImageCollection01,
            TileWide310x150PeekImageCollection02,
            TileWide310x150PeekImageCollection03,
            TileWide310x150PeekImageCollection04,
            TileWide310x150PeekImageCollection05,
            TileWide310x150PeekImageCollection06,
            TileWide310x150SmallImageAndText01,
            TileWide310x150SmallImageAndText02,
            TileWide310x150SmallImageAndText03,
            TileWide310x150SmallImageAndText04,
            TileWide310x150SmallImageAndText05,
            TileWide310x150Text01,
            TileWide310x150Text02,
            TileWide310x150Text03,
            TileWide310x150Text04,
            TileWide310x150Text05,
            TileWide310x150Text06,
            TileWide310x150Text07,
            TileWide310x150Text08,
            TileWide310x150Text09,
            TileWide310x150Text10,
            TileWide310x150Text11,
            TileWideBlockAndText01,
            TileWideBlockAndText02,
            TileWideImage,
            TileWideImageAndText01,
            TileWideImageAndText02,
            TileWideImageCollection,
            TileWidePeekImage01,
            TileWidePeekImage02,
            TileWidePeekImage03,
            TileWidePeekImage04,
            TileWidePeekImage05,
            TileWidePeekImage06,
            TileWidePeekImageAndText01,
            TileWidePeekImageAndText02,
            TileWidePeekImageCollection01,
            TileWidePeekImageCollection02,
            TileWidePeekImageCollection03,
            TileWidePeekImageCollection04,
            TileWidePeekImageCollection05,
            TileWidePeekImageCollection06,
            TileWideSmallImageAndText01,
            TileWideSmallImageAndText02,
            TileWideSmallImageAndText03,
            TileWideSmallImageAndText04,
            TileWideSmallImageAndText05,
            TileWideText01,
            TileWideText02,
            TileWideText03,
            TileWideText04,
            TileWideText05,
            TileWideText06,
            TileWideText07,
            TileWideText08,
            TileWideText09,
            TileWideText10,
            TileWideText11
        }

        public class NotificationInfoEventArgs : RoutedEventArgs
        {
            public NotificationInfo OldNotification;
            public NotificationInfo NewNotification;
        }

        public class AppInfo : DependencyObject
        {
            public string DisplayName
            {
                get => (string)GetValue(DisplayNameProperty);
                set => SetValue(DisplayNameProperty, value);
            }

            public static readonly DependencyProperty DisplayNameProperty =
                DependencyProperty.Register("DisplayName", typeof(string), typeof(AppInfo), new PropertyMetadata(""));

            public string InternalPath
            {
                get => Environment.ExpandEnvironmentVariables(@"%programfiles%\WindowsApps\" + InternalName + @"\AppxManifest.xml");
            }

            public string InternalName
            {
                get => (string)GetValue(InternalNameProperty);
                set => SetValue(InternalNameProperty, value);
            }

            public static readonly DependencyProperty InternalNameProperty =
                DependencyProperty.Register("InternalName", typeof(string), typeof(AppInfo), new PropertyMetadata(""));

            public ImageBrush Icon
            {
                get => (ImageBrush)GetValue(IconProperty);
                set => SetValue(IconProperty, value);
            }

            public static readonly DependencyProperty IconProperty =
                DependencyProperty.Register("Icon", typeof(ImageBrush), typeof(AppInfo), new PropertyMetadata(new ImageBrush()));

            public Color Color
            {
                get => (Color)GetValue(ColorProperty);
                set => SetValue(ColorProperty, value);
            }

            public static readonly DependencyProperty ColorProperty =
                DependencyProperty.Register("Color", typeof(Color), typeof(AppInfo), new FrameworkPropertyMetadata(Color.FromArgb(0xFF, 0, 0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

            public ObservableCollection<string> LiveText
            {
                get => (ObservableCollection<string>)GetValue(LiveTextProperty);
                set => SetValue(LiveTextProperty, value);
            }

            public static readonly DependencyProperty LiveTextProperty =
                DependencyProperty.Register("LiveText", typeof(ObservableCollection<string>), typeof(AppInfo), new FrameworkPropertyMetadata(new ObservableCollection<string>(), FrameworkPropertyMetadataOptions.AffectsRender));

            public ObservableCollection<ImageBrush> LiveImages
            {
                get => (ObservableCollection<ImageBrush>)GetValue(LiveImagesProperty);
                set => SetValue(LiveImagesProperty, value);
            }

            public static readonly DependencyProperty LiveImagesProperty =
                DependencyProperty.Register("LiveImages", typeof(ObservableCollection<ImageBrush>), typeof(AppInfo), new FrameworkPropertyMetadata(new ObservableCollection<ImageBrush>(), FrameworkPropertyMetadataOptions.AffectsRender));

            XmlDocument _appxManifest = new XmlDocument();
            //%programfiles%/WindowsApps/Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe/AppxManifest.xml

            NotificationInfo _currentNotification;

            public Timer _notificationTimer;

            public event EventHandler<NotificationInfoEventArgs> NotificationReceived;

            public AppInfo(string App)
            {
                InternalName = App;

                _appxManifest.Load(InternalPath);

                string dummyString = "If you see this text, remind me to look into getting Apps' display names.";
                DisplayName = dummyString;
                //XmlNodeList nodes = ;
                foreach (XmlNode node in _appxManifest.GetElementsByTagName("Package")[0].ChildNodes)
                {
                    if (node.Name == "Properties")
                    {
                        foreach (XmlNode property in node.ChildNodes)
                        {
                            if (property.Name == "DisplayName")
                            {
                                DisplayName = property.InnerText;
                            }
                        }
                    }
                }

                if (DisplayName == dummyString)
                {
                    foreach (XmlNode node in _appxManifest.GetElementsByTagName("Package")[0].ChildNodes)
                    {
                        if ((node.Name == "Identity") && (node.Attributes["Name"] != null))
                        {
                            DisplayName = node.Attributes["Name"].Value;
                        }
                    }
                }

                _notificationTimer = new Timer(5000);

                Icon = new ImageBrush(); //Get App icon here
                Color = Color.FromArgb(0xFF, 0xFF, 0, 0xFF); //Get App color here
                foreach (XmlNode node in _appxManifest.GetElementsByTagName("Package")[0].ChildNodes)
                {
                    Debug.WriteLine("node.Name " + node.Name);
                    if (node.Name == "Applications")
                    {
                        foreach (XmlNode secondNode in node.ChildNodes)
                        {
                            if (secondNode.Name == "Application")
                            {
                                foreach (XmlNode subNode in secondNode.ChildNodes)
                                {
                                    Debug.WriteLine("subNode.Name " + subNode.Name);
                                    if ((subNode.Name.ToLower().EndsWith("visualelements")) && (subNode.Attributes["BackgroundColor"] != null))
                                    {
                                        Debug.WriteLine("Releasing the  C O L O U R E S ...");
                                        string coloures = subNode.Attributes["BackgroundColor"].Value.Replace("#", "").ToUpper();
                                        byte red = 0;
                                        byte green = 0;
                                        byte blue = 0;
                                        Debug.WriteLine("coloures.Length " + coloures.Length.ToString());
                                        if (coloures.Length == 6)
                                        {
                                            red = byte.Parse(coloures.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                                            green = byte.Parse(coloures.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                                            blue = byte.Parse(coloures.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                                        }
                                        else if (coloures.Length == 8)
                                        {
                                            red = byte.Parse(coloures.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                                            green = byte.Parse(coloures.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                                            blue = byte.Parse(coloures.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                                        }
                                        Color = Color.FromArgb(0xFF, red, green, blue);
                                    }
                                }
                            }
                        }
                    }
                }
                    //"Microsoft.BingSports_3.0.4.212_x64__8wekyb3d8bbwe"

                _currentNotification = CurrentLiveTileNotification;
                UIElement element = new UIElement();
                _notificationTimer.Elapsed += (sneder, args) =>
                {
                    element.Dispatcher.Invoke(new Action(() =>
                    {
                        var NewNotification = CurrentLiveTileNotification;
                        if (_currentNotification != NewNotification)
                        {
                            NotificationReceived?.Invoke(this, new NotificationInfoEventArgs()
                            {
                                OldNotification = _currentNotification,
                                NewNotification = NewNotification
                            });
                            _currentNotification = NewNotification;
                            LiveText.Clear();
                            foreach (string s in NewNotification.Text)
                            {
                                Debug.WriteLine(s);
                                LiveText.Add(s);
                            }
                            LiveImages.Clear();
                            foreach (ImageBrush b in NewNotification.Images)
                            {
                                Debug.WriteLine("IMAGEBRUSH ADDED " + b.ImageSource.Width.ToString() + " " + b.ImageSource.Height.ToString());
                                LiveImages.Add(b);
                            }
                        }
                    }));
                };
                _notificationTimer.Start();
            }

            string TileNotificationAddress
            {
                get
                {
                    string address = "";
                    //XmlDocument appxManifest = new XmlDocument();
                    //%programfiles%/WindowsApps/Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe/AppxManifest.xml
                    //appxManifest.Load(InternalPath);
                    foreach (XmlNode node in _appxManifest.GetElementsByTagName("Package")[0].ChildNodes)
                    {
                        Debug.WriteLine("node.Name " + node.Name);
                        if (node.Name.ToLower() == "applications")
                        {
                            foreach (XmlNode secondNode in node.ChildNodes)
                            {
                                Debug.WriteLine("secondNode.Name " + secondNode.Name);
                                if (secondNode.Name.ToLower() == "application")
                                {
                                    foreach (XmlNode subNode in secondNode.ChildNodes)
                                    {
                                        Debug.WriteLine("subNode.Name " + subNode.Name);
                                        if (subNode.Name.ToLower().EndsWith("visualelements"))
                                        {
                                            foreach (XmlNode defaultTileNode in subNode.ChildNodes)
                                            {
                                                Debug.WriteLine("defaultTileNode.Name " + defaultTileNode.Name);
                                                if (defaultTileNode.Name.ToLower().EndsWith("defaulttile"))
                                                {
                                                    foreach (XmlNode tileUpdNode in defaultTileNode.ChildNodes)
                                                    {
                                                        Debug.WriteLine("tileUpdNode.Name " + tileUpdNode.Name);
                                                        //XmlNodeList tileUpdate = _appxManifest.GetElementsByTagName("wb:TileUpdate");
                                                        if ((tileUpdNode.Name.ToLower().EndsWith("tileupdate")) && (tileUpdNode.Attributes["UriTemplate"] != null))
                                                        {
                                                            address = tileUpdNode.Attributes["UriTemplate"].Value;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //XmlNode root = appxManifest.DocumentElement;
                    //wb: TileUpdate
                    Debug.WriteLine("ADDRESS: " + address);
                    return address;
                }
            }

            NotificationInfo CurrentLiveTileNotification
            {
                get
                {
                    NotificationInfo notifyInfo = new NotificationInfo();
                    XmlDocument tileDocument = new XmlDocument();

                    //@"http://{language}.appex-rf.msn.com/cgtile/v1/{language}/News/Today.xml"
                    Debug.WriteLine(InternalPath);
                    tileDocument.LoadXml(GetLiveTileFromWebAddress(TileNotificationAddress));

                    foreach (XmlNode visual in tileDocument.SelectNodes("/tile/visual"))
                    {
                        foreach (XmlNode binding in visual.ChildNodes)
                        {
                            Debug.WriteLine(binding.ChildNodes.Count.ToString());
                            foreach (XmlNode node in binding.ChildNodes)
                            {
                                if (node.Name.ToLower() == "image")
                                {
                                    if (Uri.IsWellFormedUriString(node.Attributes["src"].Value, UriKind.RelativeOrAbsolute))
                                    {
                                        notifyInfo.Images.Add(new ImageBrush(new BitmapImage(new Uri(node.Attributes["src"].Value, UriKind.RelativeOrAbsolute))));
                                        Debug.WriteLine("Image added from src");
                                    }
                                    else if (Uri.IsWellFormedUriString(node.Attributes["alt"].Value, UriKind.RelativeOrAbsolute))
                                    {
                                        notifyInfo.Images.Add(new ImageBrush(new BitmapImage(new Uri(node.Attributes["alt"].Value, UriKind.RelativeOrAbsolute))));
                                        Debug.WriteLine("Image added from alt");
                                    }
                                }
                                else if (node.Name.ToLower() == "text")
                                {
                                    notifyInfo.Text.Add(Encoding.Default.GetString(Encoding.Convert(Encoding.UTF8, Encoding.Default, Encoding.Default.GetBytes(node.InnerText))));
                                    Debug.WriteLine("Text added: " + Encoding.Default.GetString(Encoding.Convert(Encoding.UTF8, Encoding.Default, Encoding.Default.GetBytes(node.InnerText))));
                                }
                            }
                            try
                            {
                                notifyInfo.Type = (TileNotificationType)Enum.Parse(typeof(TileNotificationType), binding.Attributes["template"].Value, true);
                            }
                            catch
                            {
                                try
                                {
                                    notifyInfo.Type = (TileNotificationType)Enum.Parse(typeof(TileNotificationType), binding.Attributes["fallback"].Value, true);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex);
                                }
                            }
                        }
                    }

                    return notifyInfo;
                }
            }
        }

        public class NotificationInfo
        {
            public TileNotificationType Type;
            public List<string> Text = new List<string>();
            public List<ImageBrush> Images = new List<ImageBrush>();
        }

        public static string GetLiveTileFromWebAddress(string address)
        {
            string tempUri;
            using (var wc = new System.Net.WebClient())
            {
                string tempUriThingy = address.Replace("{language}", "en-US");
                tempUri = wc.DownloadString(tempUriThingy).Replace("{language}", "en-US");
            }
            return tempUri;
        }
    }
}
