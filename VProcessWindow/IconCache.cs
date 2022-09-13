using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VProcessWindow
{
    public class IconCache
    {
        Dictionary<string, BitmapImage> iconMap = new Dictionary<string, BitmapImage>();

        [DllImport("shell32.dll")]
        static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        public BitmapImage findIcon(string iconKey)
        {
            BitmapImage bi;
            if (iconMap.TryGetValue(iconKey, out bi))
            {
                return bi;
            }
            return null;
        }

        public bool loadIcon(string fileName, string iconKey = null)
        {
            var key = iconKey;
            if (null == key)
            {
                key = fileName;
            }

            var icon = getIcon(fileName);
            try
            {
                if (null != icon)
                {
                    if (iconMap.ContainsKey(fileName))
                    {
                        return true;
                    }
                    var bm = icon.ToBitmap();
                    var bi = toBitmapImage(bm);
                    iconMap[key] = bi;
                    return true;
                }
            }
            finally
            {
                if (null != icon)
                {
                    //Kernel32.CloseHandle(icon.Handle);
                }
            }
            return false;
        }

        static System.Drawing.Icon getIcon(string fileName)
        {
            if (null == fileName)
            {
                return null;
            }
            var iconHandle = ExtractIcon(IntPtr.Zero, fileName, 0);
            if (IntPtr.Zero == iconHandle)
            {
                return null;
            }
            var icon = System.Drawing.Icon.FromHandle(iconHandle);
            return icon;
        }

        static BitmapImage toBitmapImage(Bitmap bm)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bm.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();

                return bi;
            }
        }

    } // end - class IconCache
}
