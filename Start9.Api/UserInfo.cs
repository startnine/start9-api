using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Environment;

namespace Start9.Api
{
    public static class UserInfo
    {
        public static ImageBrush UserAvatar
        {
            get
            {
                ImageBrush brush = new ImageBrush();
                try
                {
                    if (File.Exists(GetFolderPath(SpecialFolder.LocalApplicationData) + @"\Temp\" + UserName + ".bmp"))
                    {
                        brush = new ImageBrush(new BitmapImage(new Uri(GetFolderPath(SpecialFolder.LocalApplicationData) + @"\Temp\" + UserName + ".bmp")));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return brush;
            }
        }
    }
}
