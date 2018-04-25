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

namespace Start9.Api
{
    public static class SystemState
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
