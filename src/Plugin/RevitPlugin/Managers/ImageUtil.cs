using System;
using System.Windows.Media.Imaging;

namespace RevitPlugin.Managers
{
    internal static class ImageUtil
    {
        public static BitmapImage LoadScaledImage(string path, int size)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path);
            image.DecodePixelWidth = size;
            image.DecodePixelHeight = size;
            image.EndInit();
            return image;
        }
    }
}
