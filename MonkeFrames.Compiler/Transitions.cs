using System;

namespace MonkeFrames.Compiler;

public static class Transitions
{
    private static (float, bool) difference(bool isRotation, float start, float end) {
        float n = isRotation && (start - end > 0) ? (start - end) : (end - start);
        return (n, start - end > 0);
    }

    // Line straight up/down
    public static float Linear(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        var (difference, sub) = difference(isRotation, start, end);
        float step = difference / incrementTimes;
        float now = (step * currentPosition);
        return sub ? start - now : start + now;
    }

    // Stay in place until done
    public static float Cut(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        return currentPosition != incrementTimes ? start : end;
    }

    // Movement determined by sine wave
    public static float Sine(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        var (difference, sub) = difference(isRotation, start, end);
        double[] sineValues = new double[incrementTimes + 1];

        for (int i = 0; i <= incrementTimes; i++)
        {
            double progress = (double)i / incrementTimes;
            sineValues[i] = Math.Sin(progress * 2 * Math.PI);
        }

        float now = (float)(difference * sineValues[currentPosition]);
        return sub ? start - now : start + now;
    }
}