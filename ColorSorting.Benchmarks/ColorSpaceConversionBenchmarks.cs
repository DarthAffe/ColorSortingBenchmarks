using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using ColorSorting;
using Colourful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorSorting.Benchmarks;

[ShortRunJob]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[MemoryDiagnoser]
public class ColorSpaceConversionBenchmarks
{
    private readonly RGBColor rgbColor;
    private readonly XYZColor xyzColor;
    private readonly LabColor labColor;

    public ColorSpaceConversionBenchmarks()
    {
        rgbColor = new RGBColor(255, 0, 0);
        xyzColor = Mine.ColorConverter.RgbToXyz(rgbColor);
        labColor = Mine.ColorConverter.XyzToLab(xyzColor);
    }

    //[BenchmarkCategory("RgbToXyz"), Benchmark]
    //public XYZColor RgbToXyzMine()
    //{
    //    return Mine.ColorConverter.RgbToXyz(rgbColor);
    //}

    //[BenchmarkCategory("RgbToXyz"), Benchmark(Baseline = true)]
    //public XYZColor RgbToXyzLib()
    //{
    //    return Colorful.ColorConverter.RgbToXyz(rgbColor);
    //}

    //[BenchmarkCategory("XyzToRgb"), Benchmark]
    //public RGBColor XyzToRgbMine()
    //{
    //    return Mine.ColorConverter.XyzToRgb(xyzColor);
    //}

    //[BenchmarkCategory("XyzToRgb"), Benchmark(Baseline = true)]
    //public RGBColor XyzToRgbLib()
    //{
    //    return Colorful.ColorConverter.XyzToRgb(xyzColor);
    //}

    //[BenchmarkCategory("XyzToLab"), Benchmark]
    //public LabColor XyzToLabMine()
    //{
    //    return Mine.ColorConverter.XyzToLab(xyzColor);
    //}

    //[BenchmarkCategory("XyzToLab"), Benchmark(Baseline = true)]
    //public LabColor XyzToLabLib()
    //{
    //    return Colorful.ColorConverter.XyzToLab(xyzColor);
    //}

    //[BenchmarkCategory("XyzToLab"), Benchmark]
    //public LabColor XyzToLabLibOptimized()
    //{
    //    return Optimized.ColorConverter2.XyzToLab(xyzColor);
    //}

    //[BenchmarkCategory("LabToXyz"), Benchmark]
    //public XYZColor LabToXyzMine()
    //{
    //    return Mine.ColorConverter.LabToXyz(labColor);
    //}

    //[BenchmarkCategory("LabToXyz"), Benchmark(Baseline = true)]
    //public XYZColor LabToXyzLib()
    //{
    //    return Colorful.ColorConverter.LabToXyz(labColor);
    //}

    //[BenchmarkCategory("RgbToLab"), Benchmark]
    //public LabColor RgbToLabMine()
    //{
    //    return Mine.ColorConverter.RgbToLab(rgbColor);
    //}

    //[BenchmarkCategory("RgbToLab"), Benchmark(Baseline = true)]
    //public LabColor RgbToLabLib()
    //{
    //    return Colorful.ColorConverter.RgbToLab(rgbColor);
    //}

    //[BenchmarkCategory("LabToRgb"), Benchmark]
    //public RGBColor LabToRgbMine()
    //{
    //    return Mine.ColorConverter.LabToRgb(labColor);
    //}

    //[BenchmarkCategory("LabToRgb"), Benchmark(Baseline = true)]
    //public RGBColor LabToRgbLib()
    //{
    //    return Colorful.ColorConverter.LabToRgb(labColor);
    //}
}
