using ColorSorting.Mine;
using ColorSorting.Optimized;
using SkiaSharp;
using Colourful;
using ColorSorting.Utils;

namespace ColorSorting.Image
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int size = 500;
            SKColor[] colors = RandomColorGenerator.GetRandomSKColors(size);
            SKBitmap old = GetBitmap(colors);
            SaveImage("before.png", old);

            ColorSorterCIEDE2000.Sort(colors);
            SKBitmap a = GetBitmap(colors);

            SaveImage("after.png", a);
        }

        private static SKBitmap GetBitmap(SKColor[] colors)
        {
            SKBitmap bitmap = new SKBitmap(colors.Length, colors.Length / 4);

            for (int col = 0; col < bitmap.Width; col++)
            {
                //RGBColor colColor = colors[col];
                //SKColor skColor = new SKColor(
                //                              (byte)(colColor.R * 255d),
                //                              (byte)(colColor.G * 255d),
                //                              (byte)(colColor.B * 255d));
                for (int row = 0; row < bitmap.Height; row++)
                {
                    bitmap.SetPixel(col, row, colors[col]);
                }
            }

            return bitmap;
        }

        private static void SaveImage(string path, SKBitmap bitmap)
        {
            using (Stream s = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                SKData d = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100);
                d.SaveTo(s);
            }
        }
    }
}