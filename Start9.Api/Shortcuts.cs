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

    public static class Shortcuts
    {
        public class Shortcut
        {
            public static void CreateShortcut(string lnkFileName, string openTargetPath)
            {
                string dir = openTargetPath.Replace(Path.GetFileName(openTargetPath), "");

                if (dir.Contains(@"\"))
                {
                    dir = dir.Substring(0, dir.LastIndexOf(@"\"));
                }

                CreateShortcut(lnkFileName, "Location: " + Path.GetDirectoryName(openTargetPath) + " (" + dir + ")", openTargetPath);
            }

            public static void CreateShortcut(string lnkFileName, string lnkDescription, string openTargetPath)
            {
                object shDesktop = (object)"Desktop";
                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\" + lnkFileName;
                CreateShortcut(lnkFileName, lnkDescription, openTargetPath, shortcutAddress);
            }

            public static void CreateShortcut(string lnkFileName, string lnkDescription, string openTargetPath, string lnkOutputLocation)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(lnkOutputLocation);
                shortcut.Description = lnkDescription;
                shortcut.TargetPath = openTargetPath;
                shortcut.Save();
            }

            string _rawPath;

            public Shortcut(string s)
            {
                _rawPath = s;
            }

            public string TargetPath
            {
                get
                {
                    string targetPath = MsiShortcut;

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

            string InternetShortcut
            {
                get
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
            }

            string ResolveShortcut
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

            string MsiShortcut
            {
                get
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
            }
        }
    }
}