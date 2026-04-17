using System;

namespace MonkeFrames.Compiler;

public static class Transitions
{
    // Line straight up/down
    public static float Linear(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        var difference = end - start;
        float step = difference / incrementTimes;
        float now = (step * currentPosition);
        return start + now;
    }

    // Stay in place until done
    public static float Cut(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        return currentPosition != incrementTimes ? start : end;
    }

    // Movement determined by sine wave
    public static float Sine(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        var difference = end - start;
        double[] sineValues = new double[incrementTimes + 1];

        for (int i = 0; i <= incrementTimes; i++)
        {
            double progress = (double)i / incrementTimes;
            sineValues[i] = Math.Sin(progress * 2 * Math.PI);
        }

        float now = (float)(difference * sineValues[currentPosition]);
        return start + now;
    }
}