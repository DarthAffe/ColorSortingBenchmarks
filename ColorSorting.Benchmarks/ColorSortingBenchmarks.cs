using BenchmarkDotNet.Attributes;
using ColorSorting.Utils;
using ColorSorting.Optimized;
using SkiaSharp;

namespace ColorSorting.Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class ColorSortingBenchmarks
{
    //private static readonly IColorConverter<RGBColor, LabColor> _rgbToLab = new ConverterBuilder().FromRGB().ToLab().Build();
    //private static readonly IColorConverter<LabColor, RGBColor> _labToRgb = new ConverterBuilder().FromLab().ToRGB().Build();
    //private static readonly CIEDE2000ColorDifference _difference = new ();
    private SKColor[] _data = Array.Empty<SKColor>();

    [Params(/*4, 6, 8, 16, 32, 64,*/ 128/*, 256, 512, 1024, 2048 */)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _data = RandomColorGenerator.GetRandomSKColors(Count);
    }

    [Benchmark]
    public void CIEDE2000()
    {
        ColorSorterCIEDE2000.Sort(_data);
    }

    [Benchmark]
    public void CIE94()
    {
        ColorSorterCIE94.Sort(_data);
    }

    [Benchmark]
    public void CIE76()
    {
        ColorSorterCIE76.Sort(_data);
    }

    //private static readonly CIEDE2000ColorDifference _diff = new();

    //[Benchmark]
    //public void DiffColorful()
    //{
    //    _diff.ComputeDifference(new LabColor(1, 0.5, 0.5), new LabColor(1, 1, 1));
    //}

    //[Benchmark]
    //public void Reference()
    //{
    //    CIEDE2000_Reference.ComputeDifference(new LabColor(1, 0.5, 0.5), new LabColor(1, 1, 1));
    //}

    //[Benchmark]
    //public void Optimized()
    //{
    //    CIEDE2000_2.ComputeDifference(new LabColor(1, 0.5, 0.5), new LabColor(1, 1, 1));
    //}


    //[Benchmark]
    //public void DiffMine()
    //{
    //    ColorSorting.Mine.CIEDE2000.ComputeDifference(new LabColor(1, 0.5, 0.5), new LabColor(1, 1, 1));
    //}

    //[Benchmark]
    //public void DiffMine2()
    //{
    //    CIEDE2000_2.ComputeDifference(new LabColor(1, 0.5, 0.5), new LabColor(1, 1, 1));
    //}

    //public void SortNew(in Span<LabColor> result, in Span<LabColor> input)
    //{
    //    Span<bool> visited = stackalloc bool[input.Length];
    //    int amountVisited = 0;

    //    var minDistanceToBlack = double.MaxValue;
    //    var closestToBlack = new LabColor();
    //    var closestToBlackIndex = int.MaxValue;
    //    for (int i = 0; i < input.Length; i++)
    //    {
    //        var testColor = input[i];

    //        var testDistance = GetDistance(testColor, new LabColor());
    //        if (testDistance < minDistanceToBlack)
    //        {
    //            minDistanceToBlack = testDistance;
    //            closestToBlack = testColor;
    //            closestToBlackIndex = i;
    //        }
    //    }

    //    result[amountVisited++] = closestToBlack;
    //    visited[closestToBlackIndex] = true;

    //    while (amountVisited < input.Length)
    //    {
    //        LabColor nearest = new LabColor();
    //        double nearestDistance = double.MaxValue;
    //        int chosen = int.MaxValue;

    //        for (int i = 0; i < input.Length; i++)
    //        {
    //            //we visited this color already, skip.
    //            if (visited[i])
    //                continue;

    //            var color = input[i];

    //            var distance = GetDistance(color, closestToBlack);
    //            if (distance < nearestDistance)
    //            {
    //                nearest = color;
    //                nearestDistance = distance;
    //                chosen = i;
    //            }
    //        }
    //        closestToBlack = nearest;

    //        result[amountVisited] = closestToBlack;
    //        if (chosen != int.MaxValue)
    //            visited[chosen] = true;

    //        amountVisited++;
    //    }
    //}

    //[Benchmark]
    //public void BenchOld()
    //{

    //    var sorted = SortOld(span.ToArray());

    //    int i = sorted.Length;
    //}

    //private RGBColor[] SortOld(RGBColor[] swatch)
    //{
    //    var sorted = new List<RGBColor>(swatch.Length);
    //    var unvisited = new List<RGBColor>(swatch.Length);

    //    var current = swatch[0];
    //    unvisited.AddRange(swatch);
    //    unvisited.Remove(current);
    //    sorted.Add(current);

    //    while (unvisited.Count > 0)
    //    {
    //        var nearest = unvisited[0];
    //        var nearestDistance = double.MaxValue;
    //        foreach (var color in unvisited)
    //        {
    //            var distance = GetDistance(color, current);
    //            if (distance < nearestDistance)
    //            {
    //                nearest = color;
    //                nearestDistance = distance;
    //            }
    //        }
    //        current = nearest;
    //        unvisited.Remove(current);
    //        sorted.Add(current);
    //    }
    //    return sorted.ToArray();
    //}

    //private static double GetDistance(RGBColor color, RGBColor current)
    //{
    //    //return _difference.ComputeDifference(_rgbToLab.Convert(color), _rgbToLab.Convert(current));
    //    return _difference.ComputeDifference(ColorConverter.RgbToLab(color), ColorConverter.RgbToLab(current));
    //}

    //private static double GetDistance(in LabColor a, in LabColor b)
    //{
    //    return _difference.ComputeDifference(a, b);
    //}
}
