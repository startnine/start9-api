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

namespace Start9.Api.Tools
{
    public static class AppxTools
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

        public class NotificationInfoEventArgs : EventArgs
        {
            public NotificationInfo OldNotification;
            public NotificationInfo NewNotification;
        }

        public class TileInfo
        {
            public string Name;
            public ImageBrush Icon;
            public Color Color;

            public NotificationInfo CurrentNotification;

            Timer notificationTimer;

            public event EventHandler<NotificationInfoEventArgs> NotificationReceived;

            public TileInfo(string App)
            {
                notificationTimer = new Timer(5000);
                Name = "App Name (TEMP)"; //Get App name here
                Icon = new ImageBrush(); //Get App icon here
                Color = Color.FromArgb(0xFF, 0xFF, 0, 0xFF); //Get App color here

                //"Microsoft.BingSports_3.0.4.212_x64__8wekyb3d8bbwe"

                CurrentNotification = AppxTools.GetLiveTileNotification(AppxTools.GetUpdateAddressFromApp(App));
                UIElement element = new UIElement();
                notificationTimer.Elapsed += delegate
                {
                    element.Dispatcher.Invoke(new Action(() =>
                    {
                        var NewNotification = AppxTools.GetLiveTileNotification(AppxTools.GetUpdateAddressFromApp(App));
                        if (CurrentNotification != NewNotification)
                        {
                            NotificationReceived?.Invoke(this, new NotificationInfoEventArgs()
                            {
                                OldNotification = CurrentNotification,
                                NewNotification = NewNotification
                            });
                            CurrentNotification = NewNotification;
                        }
                    }));
                };
                notificationTimer.Start();
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
                tempUri = wc.DownloadString(tempUriThingy);
            }
            return tempUri;
        }

        public static string GetUpdateAddressFromApp(string app)
        {
            XmlDocument appxManifest = new XmlDocument();
            //%programfiles%/WindowsApps/Microsoft.BingNews_3.0.4.213_x64__8wekyb3d8bbwe/AppxManifest.xml
            string appxManifestPath = Environment.ExpandEnvironmentVariables(@"%programfiles%/WindowsApps/") + app + @"/AppxManifest.xml";
            appxManifest.Load(appxManifestPath);
            XmlNodeList tileUpdate = appxManifest.GetElementsByTagName("wb:TileUpdate");
            return tileUpdate[0].Attributes["UriTemplate"].Value;
            //XmlNode root = appxManifest.DocumentElement;
            //wb: TileUpdate
        }

        public static NotificationInfo GetLiveTileNotification(string app)
        {
            NotificationInfo notifyInfo = new NotificationInfo();
            XmlDocument tileDocument = new XmlDocument();

            //@"http://{language}.appex-rf.msn.com/cgtile/v1/{language}/News/Today.xml"
            tileDocument.LoadXml(GetLiveTileFromWebAddress(app));

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
