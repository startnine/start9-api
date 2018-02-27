using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Start9.Api.Tools;

namespace Start9.Api.Tools
{
    public static class ShortcutTools
    {
        public static string GetTargetPath(string filePath)
        {
            string targetPath = ResolveMsiShortcut(filePath);

            if (targetPath == null)
            {
                targetPath = ResolveShortcut(filePath);
            }

            if (targetPath == null)
            {
                targetPath = GetInternetShortcut(filePath);
            }

            if (targetPath == null | targetPath == "" | targetPath.Replace(" ", "") == "")
            {
                return filePath;
            }
            else
            {
                return targetPath;
            }
        }

        public static string GetInternetShortcut(string filePath)
        {
            try
            {
                string url = "";

                using (TextReader reader = new StreamReader(filePath))
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

        static string ResolveShortcut(string filePath)
        {
            // IWshRuntimeLibrary is in the COM library "Windows Script Host Object Model"
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

            try
            {
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(filePath);
                return shortcut.TargetPath;
            }
            catch
            {
                // A COMException is thrown if the file is not a valid shortcut (.lnk) file 
                return null;
            }
        }

        static string ResolveMsiShortcut(string file)
        {
            StringBuilder product = new StringBuilder(WinApi.MaxGuidLength + 1);
            StringBuilder feature = new StringBuilder(WinApi.MaxFeatureLength + 1);
            StringBuilder component = new StringBuilder(WinApi.MaxGuidLength + 1);

            WinApi.MsiGetShortcutTarget(file, product, feature, component);

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
