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
    public static float Cut(float start, float end, int currentPosition, int incrementTimes) {
        return currentPosition != incrementTimes ? start : end;
    }

    // Movement determined by sine wave
    public static float Sine(float start, float end, int currentPosition, int incrementTimes)
    {
        // placeholder linear function 
        float difference = end - start;
        float step = difference / incrementTimes;
        float now = difference + (step * currentPosition);

        return start + now;
    }
}