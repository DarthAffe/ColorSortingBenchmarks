using BenchmarkDotNet.Running;
using Colourful;

namespace ColorSorting.Benchmarks;

internal class Program
{
    static void Main(string[] args)
    {
        //Profile();
        BenchmarkRunner.Run<ColorSortingBenchmarks>();
        //BenchmarkRunner.Run<ColorSpaceConversionBenchmarks>();
    }

    private static void Profile()
    {
        //var csb = new ColorSortingBenchmarks();
        //for (int i = 0; i < 100; i++)
        //{
        //    csb.BenchNew();
        //}
        LabColor lab = new LabColor(50, 0, 50);
        for (int i = 0; i < 100_000_000; i++)
        {
            Mine.ColorConverter.LabToXyz(lab);
        }

        for (int i = 0; i < 100_000_000; i++)
        {
            Colorful.ColorConverter.LabToXyz(lab);
        }
    }
}