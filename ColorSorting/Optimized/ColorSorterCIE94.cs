﻿using System.Buffers;
using SkiaSharp;

namespace ColorSorting.Optimized;

public static class ColorSorterCIE94
{
    private readonly record struct SortColor(SKColor RGB, LabColor Lab);

    private const int STACK_ALLOC_LIMIT = 1024;

    public static void Sort(in Span<SKColor> colors, SKColor startColor = new())
    {
        LabColor referenceColor = new(startColor);

        if (colors.Length < STACK_ALLOC_LIMIT)
        {
            Span<SortColor> sortColors = stackalloc SortColor[colors.Length];
            Sort(colors, sortColors, referenceColor);
        }
        else
        {
            SortColor[] sortColorArray = ArrayPool<SortColor>.Shared.Rent(colors.Length);
            Span<SortColor> sortColors = sortColorArray.AsSpan(0, colors.Length);
            Sort(colors, sortColors, referenceColor);
            ArrayPool<SortColor>.Shared.Return(sortColorArray);
        }
    }

    private static void Sort(in Span<SKColor> colors, Span<SortColor> sortColors, LabColor referenceColor)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            SKColor color = colors[i];
            sortColors[i] = new SortColor(color, new LabColor(color));
        }

        for (int i = 0; i < colors.Length; i++)
        {
            float closestDistance = float.MaxValue;
            int closestIndex = -1;
            for (int j = 0; j < sortColors.Length; j++)
            {
                float distance = CIE94.ComputeDifference(sortColors[j].Lab, referenceColor);
                if (distance == 0f)
                {
                    closestIndex = j;
                    break;
                }

                if (distance < closestDistance)
                {
                    closestIndex = j;
                    closestDistance = distance;
                }
            }

            SortColor closestColor = sortColors[closestIndex];
            colors[i] = closestColor.RGB;
            referenceColor = closestColor.Lab;

            sortColors[closestIndex] = sortColors[^1];
            sortColors = sortColors[..^1];
        }
    }
}
