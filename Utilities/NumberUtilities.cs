namespace MonkeFrames.Utilities;

public static class NumberUtilities
{
    public static float Max(params float[] nums)
    {
        float max = float.MinValue;
        foreach (float n in numbers)
            if (n > max) max = n;
        return max;
    }
}