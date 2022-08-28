using ColorSorting.Utils;
using Colourful;
using Xunit;

namespace ColorSorting.Tests
{
    public class ColorConverterTests
    {
        //meh, good enough
        const double tolerance = 0.1;
        const int size = 100;

        [Fact]
        public void EnsureThatRgbToXyzIsWithinToleranceOfColorful()
        {
            double highestDifference = 0;
            RGBColor[] colors = RandomColorGenerator.GetRandomRgbColors(size);

            for (int i = 0; i < colors.Length; i++)
            {
                RGBColor color = colors[i];
                XYZColor xyz1 = Mine.ColorConverter.RgbToXyz(color);
                XYZColor xyz2 = Colorful.ColorConverter.RgbToXyz(color);

                void UpdateDiff(double diff)
                {
                    if (diff > highestDifference)
                        highestDifference = diff;
                }

                double xdiff = Math.Abs(xyz1.X - xyz2.X);
                double ydiff = Math.Abs(xyz1.Y - xyz2.Y);
                double zdiff = Math.Abs(xyz1.Z - xyz2.Z);

                UpdateDiff(xdiff);
                UpdateDiff(ydiff);
                UpdateDiff(zdiff);
            }

            Assert.InRange(highestDifference, 0, tolerance);
        }

        [Fact]
        public void EnsureThatXyzToRgbIsWithinToleranceOfColorful()
        {
            double highestDifference = 0;
            XYZColor[] colors = RandomColorGenerator.GetRandomXyzColors(size);

            for (int i = 0; i < colors.Length; i++)
            {
                XYZColor color = colors[i];
                RGBColor rgb1 = Mine.ColorConverter.XyzToRgb(color);
                RGBColor rgb2 = Colorful.ColorConverter.XyzToRgb(color);

                void UpdateDiff(double diff)
                {
                    if (diff > highestDifference)
                        highestDifference = diff;
                }

                double rdiff = Math.Abs(rgb1.R - rgb2.R);
                double gdiff = Math.Abs(rgb1.G - rgb2.G);
                double bdiff = Math.Abs(rgb1.B - rgb2.B);

                UpdateDiff(rdiff);
                UpdateDiff(gdiff);
                UpdateDiff(bdiff);
            }
            
            Assert.InRange(highestDifference, 0, tolerance);
        }

        [Fact]
        public void EnsureThatXyzToLabIsWithinToleranceOfColorful()
        {
            double highestDifference = 0;
            XYZColor[] colors = RandomColorGenerator.GetRandomXyzColors(size);

            for (int i = 0; i < colors.Length; i++)
            {
                XYZColor color = colors[i];
                LabColor lab1 = Mine.ColorConverter.XyzToLab(color);
                LabColor lab2 = Colorful.ColorConverter.XyzToLab(color);

                void UpdateDiff(double diff)
                {
                    if (diff > highestDifference)
                        highestDifference = diff;
                }

                double rdiff = Math.Abs(lab1.L - lab2.L);
                double gdiff = Math.Abs(lab1.a - lab2.a);
                double bdiff = Math.Abs(lab1.b - lab2.b);

                UpdateDiff(rdiff);
                UpdateDiff(gdiff);
                UpdateDiff(bdiff);
            }

            Assert.InRange(highestDifference, 0, tolerance);
        }

        [Fact]
        public void EnsureThatLabToXyzIsWithinToleranceOfColorful()
        {
            double highestDifference = 0;
            LabColor[] colors = RandomColorGenerator.GetRandomLabColors(size);

            for (int i = 0; i < colors.Length; i++)
            {
                LabColor color = colors[i];
                XYZColor lab1 = Mine.ColorConverter.LabToXyz(color);
                XYZColor lab2 = Colorful.ColorConverter.LabToXyz(color);
                
                void UpdateDiff(double diff)
                {
                    if (diff > highestDifference)
                        highestDifference = diff;
                }

                double rdiff = Math.Abs(lab1.X - lab2.X);
                double gdiff = Math.Abs(lab1.Y - lab2.Y);
                double bdiff = Math.Abs(lab1.Z - lab2.Z);

                UpdateDiff(rdiff);
                UpdateDiff(gdiff);
                UpdateDiff(bdiff);
            }

            Assert.InRange(highestDifference, 0, tolerance);
        }
    }
}