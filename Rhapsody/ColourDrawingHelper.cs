using System.Windows;
using System.Windows.Media.Imaging;

namespace org.btg.Star.Rhapsody
{
    public static class ColourDrawingHelper
    {
        public static void Draw(ColourFrame frame, WriteableBitmap bitmap)
        {
            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), frame.Data, bitmap.PixelWidth * sizeof(int), 0);
        }
    }
}
