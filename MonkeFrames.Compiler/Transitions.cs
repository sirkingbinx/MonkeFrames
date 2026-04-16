using System;
using MonkeFrames.Compiler.Models;
using UnityEngine;

namespace MonkeFrames.Compiler;

public static class Transitions
{
    // Line straight up/down
    public static float Linear(float start, float end, int currentPosition, int incrementTimes)
    {
        float difference = end - start;
        float step = difference / incrementTimes;
        float now = difference + (step * currentPosition);

        return start + now;
    }

    // Stay in place until done
    public static float Cut(float start, float end, int currentPosition, int incrementTimes)
    {
        return currentPosition != incrementTimes ? start : end;
    }

    // Movement determined by sine wave
    public static float Sine(float start, float end, int currentPosition, int incrementTimes)
    {
        // maybe works? no idea
        
        float difference = end - start;
        double[] sineValues = new double[incrementTimes + 1];

        for (int i = 0; i <= incrementTimes; i++)
        {
            double progress = (double)i / incrementTimes;
            sineValues[i] = Math.Sin(progress * 2 * Math.PI);
        }

        return (float)(difference * sineValues[currentPosition]);
    }
}