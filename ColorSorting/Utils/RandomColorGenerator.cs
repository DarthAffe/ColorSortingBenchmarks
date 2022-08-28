using Colourful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace ColorSorting.Utils
{
    public static class RandomColorGenerator
    {
        private static readonly Random _random = new(100);

        public static RGBColor[] GetRandomRgbColors(int length)
        {
            RGBColor[]? ret = new RGBColor[length];

            for (int i = 0; i < ret.Length; i++)
            {
                //(float r, float g, float b) = CalculateRGBFromHSV(_random.NextSingle() * 360, 1, 1);
                //(float r, float g, float b) = CalculateRGBFromHSV(i, 1, 1);
                //ret[i] = new RGBColor(r, g, b);
                ret[i] = new RGBColor(_random.NextDouble(), _random.NextDouble(), _random.NextDouble());
            }

            return ret;
        }

        public static SKColor[] GetRandomSKColors(int length)
        {
            SKColor[] ret = new SKColor[length];

            for (int i = 0; i < ret.Length; i++)
            {
                //(float r, float g, float b) = CalculateRGBFromHSV(_random.NextSingle() * 360, 1, 1);
                //(float r, float g, float b) = CalculateRGBFromHSV(i, 1, 1);
                //ret[i] = new RGBColor(r, g, b);
                ret[i] = new SKColor((byte)_random.Next(byte.MaxValue), (byte)_random.Next(byte.MaxValue), (byte)_random.Next(byte.MaxValue));
            }

            return ret;
        }

        public static XYZColor[] GetRandomXyzColors(int length)
        {
            XYZColor[]? ret = new XYZColor[length];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new XYZColor(_random.NextDouble(), _random.NextDouble(), _random.NextDouble());
            }

            return ret;
        }

        public static LabColor[] GetRandomLabColors(int length)
        {
            LabColor[]? ret = new LabColor[length];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new LabColor(_random.Next(100), _random.Next(200) - 100, _random.Next(200) - 100);
            }

            return ret;
        }

        private static (float r, float g, float b) CalculateRGBFromHSV(float h, float s, float v)
        {
            h = h.Wrap(0, 360);
            s = s.Clamp(0, 1);
            v = v.Clamp(0, 1);

            if (s <= 0.0)
                return (v, v, v);

            float hh = h / 60.0f;
            int i = (int)hh;
            float ff = hh - i;
            float p = v * (1.0f - s);
            float q = v * (1.0f - (s * ff));
            float t = v * (1.0f - (s * (1.0f - ff)));

            return i switch
            {
                0 => (v, t, p),
                1 => (q, v, p),
                2 => (p, v, t),
                3 => (p, q, v),
                4 => (t, p, v),
                _ => (v, p, q)
            };
        }

        public static float Clamp(this float value, float min, float max)
        {
            // ReSharper disable ConvertIfStatementToReturnStatement - I'm not sure why, but inlining this statement reduces performance by ~10%
            if (value < min) return min;
            if (value > max) return max;
            return value;
            // ReSharper restore ConvertIfStatementToReturnStatement
        }

        public static float Wrap(this float value, float min, float max)
        {
            float range = max - min;

            while (value >= max)
                value -= range;

            while (value < min)
                value += range;

            return value;
        }

        public static byte GetByteValueFromPercentage(this float percentage)
        {
            if (float.IsNaN(percentage)) return 0;

            percentage = percentage.Clamp(0, 1.0f);
            return (byte)(percentage >= 1.0f ? 255 : percentage * 256.0f);
        }
    }
}
