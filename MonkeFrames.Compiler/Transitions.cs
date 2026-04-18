using UnityEngine;

namespace MonkeFrames.Compiler;

internal static class Transitions
{
    public static Vector3 Linear(Vector3 start, Vector3 end, int currentPosition, int incrementTimes) =>
        LinearVector3(start, end, currentPosition, incrementTimes);

    public static Quaternion Linear(Quaternion start, Quaternion end, int currentPosition, int incrementTimes) =>
        LinearQuaternion(start, end, currentPosition, incrementTimes);

    public static float Linear(float start, float end, int currentPosition, int incrementTimes) =>
        LinearFloat(start, end, currentPosition, incrementTimes);

    private static Vector3 LinearVector3(Vector3 start, Vector3 end, int currentPosition, int incrementTimes)
    {
        return Vector3.Lerp(start, end, (float)currentPosition / incrementTimes);
    }

    private static Quaternion LinearQuaternion(Quaternion start, Quaternion end, int currentPosition, int incrementTimes)
    {
        return Quaternion.Lerp(start, end, (float)currentPosition / incrementTimes);
    }

    private static float LinearFloat(float start, float end, int currentPosition, int incrementTimes)
    {
        var difference = end - start;
        float step = difference / incrementTimes;
        float now = (step * currentPosition);
        return start + now;
    }

    public static Vector3 Cut(Vector3 start, Vector3 end, int currentPosition, int incrementTimes) =>
        CutVector3(start, end, currentPosition, incrementTimes);

    public static Quaternion Cut(Quaternion start, Quaternion end, int currentPosition, int incrementTimes) =>
       CutQuaternion(start, end, currentPosition, incrementTimes);

    public static float Cut(float start, float end, int currentPosition, int incrementTimes) =>
        CutFloat(start, end, currentPosition, incrementTimes);

    private static Vector3 CutVector3(Vector3 start, Vector3 end, int currentPosition, int incrementTimes)
    {
        return currentPosition != incrementTimes ? start : end;
    }

    private static Quaternion CutQuaternion(Quaternion start, Quaternion end, int currentPosition, int incrementTimes)
    {
        return currentPosition != incrementTimes ? start : end;
    }

    private static float CutFloat(float start, float end, int currentPosition, int incrementTimes)
    {
        return currentPosition != incrementTimes ? start : end;
    }

    public static Vector3 Sine(Vector3 start, Vector3 end, int currentPosition, int incrementTimes) =>
        SineVector3(start, end, currentPosition, incrementTimes);

    public static Quaternion Sine(Quaternion start, Quaternion end, int currentPosition, int incrementTimes) =>
        SineQuaternion(start, end, currentPosition, incrementTimes);

    public static float Sine(float start, float end, int currentPosition, int incrementTimes) =>
        SineFloat(start, end, currentPosition, incrementTimes);

    private static Vector3 SineVector3(Vector3 start, Vector3 end, int currentPosition, int incrementTimes)
    {
        return Vector3.Slerp(start, end, (float)currentPosition / incrementTimes);
    }

    private static Quaternion SineQuaternion(Quaternion start, Quaternion end, int currentPosition, int incrementTimes)
    {
        return Quaternion.Slerp(start, end, (float)currentPosition / incrementTimes);
    }

    private static float SineFloat(float start, float end, int currentPosition, int incrementTimes)
    {
        // shh dont tell anyone that this is really just linear transitioning
        var difference = end - start;
        float step = difference / incrementTimes;
        float now = (step * currentPosition);
        return start + now;
    }
}