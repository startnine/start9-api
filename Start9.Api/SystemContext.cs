using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Environment;
using Image = System.Drawing.Image;
using System.Globalization;

namespace Start9.Api
{
    public static class SystemContext
    {
        [DllImport("shell32.dll", EntryPoint = "#261",
               CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void GetUserTilePath(
      string username,
      UInt32 whatever, // 0x80000000
      StringBuilder picpath, int maxLength);


        public static ImageBrush UserAvatar
        {
            get
            {
                var sb = new StringBuilder(1000);
                GetUserTilePath(null, 0x80000000, sb, sb.Capacity);
                string target = sb.ToString();
                ImageBrush brush = new ImageBrush();
                try
                {
                    if (File.Exists(target)) //GetFolderPath(SpecialFolder.LocalApplicationData) + @"\Temp\" + UserName + ".bmp")
                    {
                        brush = new ImageBrush(new BitmapImage(new Uri(target))); //GetFolderPath(SpecialFolder.LocalApplicationData) + @"\Temp\" + UserName + ".bmp")));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return brush;
            }
        }

        public static bool IsDesktopWindowManagerEnabled
        {
            get
            {
                bool isCompositionEnabled;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    WinApi.DwmIsCompositionEnabled(out isCompositionEnabled);
                }
                else
                {
                    isCompositionEnabled = false;
                }
                return isCompositionEnabled;
            }
        }

        public static string CurrentVisualStyleName
        {
            get
            {
                StringBuilder visualStyleName = new StringBuilder(0x200);
                WinApi.GetCurrentThemeName(visualStyleName, visualStyleName.Capacity, null, 0, null, 0);
                return visualStyleName.ToString();
            }
        }

        public static SolidColorBrush WindowGlassColor
        {
            get
            {
                //bool opaque = false;
                //uint coloures = (uint)1;
                SolidColorBrush brush = new SolidColorBrush();
                if (IsDesktopWindowManagerEnabled)
                {
                    ///https://stackoverflow.com/questions/13660976/get-the-active-color-of-windows-8-automatic-color-theme
                    //WinApi.DwmGetColorizationColor(out coloures, out opaque);
                    WinApi.DwmColorizationParams parameters = new WinApi.DwmColorizationParams();
                    WinApi.DwmGetColorizationParameters(ref parameters);
                    string coloures = parameters.ColorizationColor.ToString("X");
                    while (coloures.Length < 8)
                    {
                        coloures = "0" + coloures;
                    }
                    //Debug.WriteLine("coloures " + parameters.ColorizationColor.ToString("X"));
                    int alphaBase = Int32.Parse(coloures.Substring(0, 2), NumberStyles.HexNumber);
                    double alphaMultiplier = ((double)(parameters.ColorizationColorBalance + parameters.ColorizationBlurBalance)) / 128;
                    byte alpha = (byte)(alphaBase * alphaMultiplier);
                    Debug.WriteLine("balance over 255: " + (((double)(parameters.ColorizationColorBalance)) / 255) + "\nalpha: " + alpha);
                    brush = new SolidColorBrush(Color.FromArgb(alpha, byte.Parse(coloures.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(coloures.Substring(4, 2), NumberStyles.HexNumber), byte.Parse(coloures.Substring(6, 2), NumberStyles.HexNumber)));
                }
                else if (Environment.OSVersion.Version.Major <= 5)
                {
                    brush = new SolidColorBrush(Color.FromArgb(0xFF, 0, 0x53, 0xE1));
                }
                else if (Environment.OSVersion.Version.Major == 6)
                {
                    if (Environment.OSVersion.Version.Minor == 4)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x65, 0xC0, 0xF2));
                    }
                    if (Environment.OSVersion.Version.Minor == 3)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0xC8, 0x69));
                    }
                    else if (Environment.OSVersion.Version.Minor == 2)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x6B, 0xAD, 0xF6));
                    }
                    else
                    {
                        brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xB9, 0xD1, 0xEA));
                    }
                }
                else
                {
                    brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x18, 0x83, 0xD7));
                }
                return brush;
            }
        }

        public static void LockUserAccount()
        {
            WinApi.LockWorkStation();
        }

        public static void SignOut()
        {
            WinApi.ExitWindowsEx(WinApi.ExitWindowsAction.Force, 0);
        }

        public static void SleepSystem()
        {
            WinApi.SetSuspendState(false, true, true);
        }

        public static void ShutDownSystem()
        {
            WinApi.ExitWindowsEx(WinApi.ExitWindowsAction.Shutdown, 0);
        }

        public static void RestartSystem()
        {
            WinApi.ExitWindowsEx(WinApi.ExitWindowsAction.Reboot, 0);
        }
    }
}
