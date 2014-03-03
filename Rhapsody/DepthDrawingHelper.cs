using System.Windows;
using System.Windows.Media.Imaging;

namespace org.btg.Star.Rhapsody
{
    public static class DepthDrawingHelper
    {
        public static void Draw(DepthFrame frame, WriteableBitmap bitmap)
        {
            //System.Console.WriteLine("Drawing depth to " + bitmap.ToString());
            System.Console.WriteLine("Depth length: " + frame.Data.Length);

            short[] depthPixels = new short[frame.Data.Length];
            byte[] colourPixels = new byte[frame.Data.Length * sizeof(int)];

            depthPixels = frame.Data;

            int colorPixelIndex = 0;

            for (int i = 0; i < depthPixels.Length; ++i)
            {
                short depth = frame.Data[i];
                byte intensity = (byte)((depth + 1) & byte.MaxValue);

                colourPixels[colorPixelIndex++] = intensity;
                colourPixels[colorPixelIndex++] = intensity;
                colourPixels[colorPixelIndex++] = intensity;

                ++colorPixelIndex;
            }

            bitmap.WritePixels(sourceRect: new Int32Rect(0, 0, 640, 480), pixels: colourPixels, stride: bitmap.PixelWidth * sizeof(int), offset: 0);
        }
    }
}