using UnityEngine;

namespace MonkeFrames.Models;

public struct Keyframe
{
    public string KeyframeID; // eg. K<x>
    public int KeyframeIndex; // index in KeyframeManager

    public Vector3 Position;
    public Vector3 Rotation;
    public float FieldOfView;

    public Transition Transition;

    // The keyframe's rotation converted into a Quaternion.
    public readonly Quaternion QuatRotation => Quaternion.Euler(Rotation);

    // Playback information
    public float Playback_RelationalStart;
    public float Playback_RelationalEnd;
}
