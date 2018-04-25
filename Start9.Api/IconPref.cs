using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.IO;

namespace Start9.Api
{
    public static class IconPref
    {
        static XmlDocument prefDocument = new XmlDocument();
        static string prefPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\IconPref.xml");
        public static string iconsPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Start9\IconOverrides");

        static IconPref()
        {
            if (!File.Exists(prefPath))
            {
                //File.Create(prefPath);
                File.WriteAllLines(prefPath, new List<string>()
                {
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                    "<icon>",
                    "	<file>",
                    "	</file>",
                    "	<process>",
                    "	</process>",
                    "</icon>"
                });
            }

            if (!Directory.Exists(iconsPath))
            {
                Directory.CreateDirectory(iconsPath);
            }

            prefDocument.Load(prefPath);
        }

        public static void Save()
        {
            IconPref.Save();
        }

        public static List<IconOverride> FileIconOverrides
        {
            get
            {
                List<IconOverride> overrides = new List<IconOverride>();
                foreach (XmlNode node in prefDocument.SelectSingleNode(@"/icon/file").ChildNodes)
                {
                    overrides.Add(new IconOverride(node));
                }
                return overrides;
            }
        }
    }

    public class IconOverride// : INotifyPropertyChanged
    {
        XmlNode target;

        public string TargetName
        {
            get
            {
                string result = "";
                if (target.Attributes["targetName"] != null)
                {
                    result = target.Attributes["targetName"].Value;
                }
                return result;
            }
            set
            {
                target.Attributes["targetName"].Value = value.ToString();
                IconPref.Save();
            }
        }

        public bool IsFullPath
        {
            get
            {
                bool result = false;
                if (target.Attributes["fullPath"] != null)
                {
                    result = bool.Parse(target.Attributes["fullPath"].Value);
                }
                return result;
            }
            set
            {
                target.Attributes["fullPath"].Value = value.ToString();
                IconPref.Save();
            }
        }

        public string ReplacementName
        {
            get
            {
                string result = "";
                if (target.Attributes["replacement"] != null)
                {
                    result = target.Attributes["replacement"].Value;
                }
                return result;
            }
            set
            {
                target.Attributes["replacement"].Value = value.ToString();
                IconPref.Save();
            }
        }

        public ImageBrush ReplacementBrush
        {
            get => new ImageBrush(new BitmapImage(new Uri(Environment.ExpandEnvironmentVariables(IconPref.iconsPath + @"\" + ReplacementName), UriKind.RelativeOrAbsolute)));
            //return new ImageBrush(MiscTools.GetIconFromFilePath(Environment.ExpandEnvironmentVariables(ReplacementName)));
            //}
        }

        public IconOverride(XmlNode targetNode)
        {
            target = targetNode;
        }
    }
}
