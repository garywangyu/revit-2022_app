using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace RevitPlugin.Managers
{
    internal static class ImageUtil
    {
        /// <summary>
        /// 讀取並縮放圖片，避免過大影響介面。
        /// </summary>
        public static BitmapSource? LoadImage(string path, int size = 32)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(path);
            img.DecodePixelWidth = size;
            img.DecodePixelHeight = size;
            img.EndInit();
            return img;
        }
    }
}
