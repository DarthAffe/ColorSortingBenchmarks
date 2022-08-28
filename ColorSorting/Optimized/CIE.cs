using System.Runtime.CompilerServices;
using static System.MathF;

namespace ColorSorting.Optimized;

internal static class CIE76
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ComputeDifference(in LabColor x, in LabColor y)
    {
        float distanceL = x.L - y.L;
        float distanceA = x.A - y.A;
        float distanceB = x.B - y.B;
        return Sqrt((distanceL * distanceL) + (distanceA * distanceA) + (distanceB * distanceB));
    }
}

internal static class CIE94
{
    const float KL = 1.0f;
    const float K1 = 0.045f;
    const float K2 = 0.015f;
    const float SL = 1.0f;
    const float KC = 1.0f;
    const float KH = 1.0f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ComputeDifference(in LabColor x, in LabColor y)
    {
        float deltaL = x.L - y.L;
        float deltaA = x.A - y.A;
        float deltaB = x.B - y.B;

        float c1 = Sqrt(Pow2(x.A) + Pow2(x.B));
        float c2 = Sqrt(Pow2(y.A) + Pow2(y.B));
        float deltaC = c1 - c2;

        float deltaH = (Pow2(deltaA) + Pow2(deltaB)) - Pow2(deltaC);
        deltaH = deltaH < 0f ? 0f : Sqrt(deltaH);

        float sc = 1.0f + (K1 * c1);
        float sh = 1.0f + (K2 * c1);

        float i = Pow2(deltaL / (KL * SL)) + Pow2(deltaC / (KC * sc)) + Pow2(deltaH / (KH * sh));

        return i < 0f ? 0f : Sqrt(i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Pow2(in float x) => x * x;
}

// Based on the implementation of the Colourful-library
// https://github.com/tompazourek/Colourful/blob/11401fea462505317669fc509d9618344344cb9e/src/Colourful/Difference/CIEDE2000ColorDifference.cs
internal static class Ciede2000
{
    private const float TWO_PI = 2f * PI;

    private const float K_H = 1f;
    private const float K_L = 1f;
    private const float K_C = 1f;

    private static readonly float POW7_25 = Pow7(25f);

    public static float ComputeDifference(in LabColor x, in LabColor y)
    {
        // 1. Calculate C_prime, h_prime
        Calculate_a_prime(x.A, y.A, x.B, y.B, out float aPrime0, out float aPrime1);
        Calculate_C_prime(aPrime0, aPrime1, x.B, y.B, out float cPrime0, out float cPrime1);
        Calculate_h_prime(aPrime0, aPrime1, x.B, y.B, out float hPrime0, out float hPrime1);

        // 2. Calculate dL_prime, dC_prime, dH_prime
        float dLPrime = y.L - x.L; // eq. (8)
        float dCPrime = cPrime1 - cPrime0; // eq. (9)
        float dhPrime = Calculate_dh_prime(cPrime0, cPrime1, hPrime0, hPrime1);
        float dHPrime = 2f * Sqrt(cPrime0 * cPrime1) * SinDeg(dhPrime / 2f);

        // 3. Calculate CIEDE2000 Color-Difference dE00
        float lPrimeMean = (x.L + y.L) / 2f;
        float cPrimeMean = (cPrime0 + cPrime1) / 2f;
        float hPrimeMean = Calculate_h_prime_mean(hPrime0, hPrime1, cPrime0, cPrime1);
        float T = ((1f - (0.17f * CosDeg(hPrimeMean - 30f))) + (0.24f * CosDeg(2f * hPrimeMean))
                  + (0.32f * CosDeg((3 * hPrimeMean) + 6f))) - (0.20f * CosDeg((4f * hPrimeMean) - 63f));
        float dTheta = 30f * Exp(-Pow2((hPrimeMean - 275f) / 25f));
        float rC = 2f * Sqrt(Pow7(cPrimeMean) / (Pow7(cPrimeMean) + POW7_25));
        float sL = 1f + ((0.015f * Pow2(lPrimeMean - 50f)) / Sqrt(20f + Pow2(lPrimeMean - 50f)));
        float sC = 1f + (0.045f * cPrimeMean);
        float sH = 1f + (0.015f * cPrimeMean * T);
        float rT = -SinDeg(2f * dTheta) * rC;

        float dE00 = Sqrt(Pow2(dLPrime / (K_L * sL))
                        + Pow2(dCPrime / (K_C * sC))
                        + Pow2(dHPrime / (K_H * sH))
                        + (rT * (dCPrime / (K_C * sC)) * (dHPrime / (K_H * sH))));

        return dE00;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Calculate_a_prime(float a0, float a1, float b0, float b1, out float aPrime0, out float aPrime1)
    {
        float cAb0 = Sqrt((a0 * a0) + (b0 * b0));
        float cAb1 = Sqrt((a1 * a1) + (b1 * b1));

        float cAbMean = (cAb0 + cAb1) / 2;

        float g = 0.5f * (1f - Sqrt(Pow7(cAbMean) / (Pow7(cAbMean) + POW7_25)));

        aPrime0 = (1f + g) * a0;
        aPrime1 = (1f + g) * a1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Calculate_C_prime(float aPrime0, float aPrime1, float b0, float b1, out float cPrime0, out float cPrime1)
    {
        cPrime0 = Sqrt((aPrime0 * aPrime0) + (b0 * b0));
        cPrime1 = Sqrt((aPrime1 * aPrime1) + (b1 * b1));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Calculate_h_prime(float aPrime0, float aPrime1, float b0, float b1, out float hPrime0, out float hPrime1)
    {
        float hRadians = Atan2(b0, aPrime0);
        float hDegrees = NormalizeDegree(RadianToDegree(hRadians));
        hPrime0 = hDegrees;

        hRadians = Atan2(b1, aPrime1);
        hDegrees = NormalizeDegree(RadianToDegree(hRadians));
        hPrime1 = hDegrees;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Calculate_dh_prime(float cPrime0, float cPrime1, float hPrime0, float hPrime1)
    {
        if ((cPrime0 * cPrime1) == 0f)
            return 0f;

        float delta = hPrime1 - hPrime0;
        if (Abs(delta) <= 180f)
            return delta;

        if (delta > 180f)
            return delta - 360f;

        return delta + 360f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Calculate_h_prime_mean(float hPrime0, float hPrime1, float cPrime0, float cPrime1)
    {
        float sum = hPrime0 + hPrime1;
        if ((cPrime0 * cPrime1) == 0f)
            return sum;

        float delta = hPrime0 - hPrime1;
        if (Abs(delta) <= 180f)
            return sum / 2f;

        if ((Abs(delta) > 180f) && (sum < 360f))
            return (sum + 360f) / 2f;

        return (sum - 360f) / 2f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Pow2(float x) => x * x;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Pow7(float x) => x * x * x * (x * x * x) * x;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float SinDeg(float x)
    {
        float xRad = DegreeToRadian(x);
        return Sin(xRad);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float CosDeg(float x)
    {
        float xRad = DegreeToRadian(x);
        return Cos(xRad);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float RadianToDegree(float rad) => 360f * (rad / TWO_PI);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float DegreeToRadian(float deg) => TWO_PI * (deg / 360f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float NormalizeDegree(float deg)
    {
        float d = deg % 360f;
        return d >= 0 ? d : d + 360f;
    }
}