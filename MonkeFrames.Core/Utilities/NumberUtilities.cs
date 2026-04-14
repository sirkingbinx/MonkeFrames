namespace MonkeFrames.Core.Utilities;

public static class NumberUtilities
{
    // Return greatest number in list.
    public static float Max(params float[] nums)
    {
        float max = float.MinValue;
        foreach (float n in nums)
            if (n > max) max = n;
        return max;
    }

    // Assure (number < max) & (number > min).
    public static float Bounds(float number, float min, float max)
    {
        float c = number;

        if (c > max)
            c = max;
        if (c < min)
            c = min;

        return c;
    }
}