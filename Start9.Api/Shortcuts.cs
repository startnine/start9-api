using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Start9.Api.Tools;
using System.Windows;
using IWshRuntimeLibrary;

namespace Start9.Api
{
    /// <summary>
    /// Represents a shortcut on the filesystem.
    /// </summary>
    public class Shortcut
    {
        /*public static void CreateShortcut(string lnkFileName, string openTargetPath, string lnkOutputLocation)
        {
            string dir = openTargetPath.Replace(Path.GetFileName(openTargetPath), "");

            if (dir.Contains(@"\"))
            {
                dir = dir.Substring(0, dir.LastIndexOf(@"\"));
            }

            CreateShortcut(lnkFileName, "Location: " + Path.GetDirectoryName(openTargetPath) + " (" + dir + ")", openTargetPath, lnkOutputLocation);
        }

        public static void CreateShortcut(string lnkFileName, string openTargetPath)
        {
            string dir = openTargetPath.Replace(Path.GetFileName(openTargetPath), "");

            if (dir.Contains(@"\"))
            {
                dir = dir.Substring(0, dir.LastIndexOf(@"\"));
            }

            CreateShortcut(lnkFileName, "Location: " + Path.GetDirectoryName(openTargetPath) + " (" + dir + ")", openTargetPath);
        }*/

            /// <summary>
            /// Creates a shortcut.
            /// </summary>
            /// <param name="lnkFileName">The name of the shortcut.</param>
            /// <param name="lnkDescription">The shortcut's description.</param>
            /// <param name="openTargetPath">The target of the shortctut.</param>
        public static void CreateShortcut(String lnkFileName, String lnkDescription, String openTargetPath)
        {
            var shDesktop = (Object)"Desktop";
            WshShell shell = new WshShell(); // HOLY SHIT GUYS IT SAYS NEW THEN AN INTERFACE!!!!111
            var shortcutAddress = (String)shell.SpecialFolders.Item(ref shDesktop) + @"\" + lnkFileName;
            CreateShortcut(lnkFileName, lnkDescription, openTargetPath, shortcutAddress);
        }
        /// <summary>
        /// Creates a shortcut.
        /// </summary>
        /// <param name="lnkFileName">The name of the shortcut.</param>
        /// <param name="lnkDescription">The shortcut's description.</param>
        /// <param name="openTargetPath">The target of the shortctut.</param>
        /// <param name="lnkOutputLocation">The output location of the shortcut.</param>
        public static void CreateShortcut(String lnkFileName, String lnkDescription, String openTargetPath, String lnkOutputLocation)
        {
            var desc = lnkDescription;
            if (string.IsNullOrEmpty(lnkDescription))
            {
                var dir = openTargetPath.Replace(Path.GetFileName(openTargetPath), "");

                if (dir.Contains(@"\"))
                {
                    dir = dir.Substring(0, dir.LastIndexOf(@"\"));
                }

                desc = "Location: " + Path.GetDirectoryName(openTargetPath) + " (" + dir + ")";
            }
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut) shell.CreateShortcut(lnkOutputLocation);
            shortcut.Description = desc;
            shortcut.TargetPath = openTargetPath;
            shortcut.Save();
        }

        String _rawPath;

        public Shortcut(String s)
        {
            _rawPath = s;
        }

        /// <summary>
        /// Gets the target path of the shortcut.
        /// </summary>
        public String TargetPath
        {
            get
            {
                var targetPath = MsiShortcut;

                if (targetPath == null)
                {
                    targetPath = ResolveShortcut;
                }

                if (targetPath == null)
                {
                    targetPath = InternetShortcut;
                }

                if (targetPath == null | targetPath == "" | targetPath.Replace(" ", "") == "")
                {
                    return _rawPath;
                }
                else
                {
                    return targetPath;
                }
            }
        }

        String InternetShortcut
        {
            get
            {
                try
                {
                    var url = "";

                    using (TextReader reader = new StreamReader(_rawPath))
                    {
                        var line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.StartsWith("URL="))
                            {
                                String[] splitLine = line.Split('=');
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
        }

        String ResolveShortcut
        {
            get
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
        }

        String MsiShortcut
        {
            get
            {
                var product = new StringBuilder(WinApi.MaxGuidLength + 1);
                var feature = new StringBuilder(WinApi.MaxFeatureLength + 1);
                var component = new StringBuilder(WinApi.MaxGuidLength + 1);

                WinApi.MsiGetShortcutTarget(_rawPath, product, feature, component);

                var pathLength = WinApi.MaxPathLength;
                var path = new StringBuilder(pathLength);

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
        }
    }

}