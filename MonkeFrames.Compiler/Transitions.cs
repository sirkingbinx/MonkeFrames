using System;

namespace MonkeFrames.Compiler;

public static class Transitions
{
    // Line straight up/down
    public static float Linear(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        if (!isRotation)
        {
            float difference = end - start;
            float step = difference / incrementTimes;
            float now = (step * currentPosition);

            return start + now;
        } else {
            float difference = (start - end > 0) ? start - end : end - start;
            float step = difference / incrementTimes;
            float now = (step * currentPosition);

            if (start - end > 0)
                return start - now;
            else
                return start + now;
        }

        return 0f;
    }

    // Stay in place until done
    public static float Cut(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        return currentPosition != incrementTimes ? start : end;
    }

    // Movement determined by sine wave
    public static float Sine(float start, float end, int currentPosition, int incrementTimes, bool isRotation)
    {
        // maybe works? no idea
        
        float difference = end - start;
        double[] sineValues = new double[incrementTimes + 1];

        for (int i = 0; i <= incrementTimes; i++)
        {
            double progress = (double)i / incrementTimes;
            sineValues[i] = Math.Sin(progress * 2 * Math.PI);
        }

        return start + (float)(difference * sineValues[currentPosition]);
    }
}