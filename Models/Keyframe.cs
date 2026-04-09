using UnityEngine;

namespace MonkeFrames.Models;

public struct Keyframe
{
    public string KeyframeID; // eg. K<x>
    public int KeyframeIndex; // index in KeyframeManager

    public Vector3 Position;
    public Vector3 Rotation;
    public float FieldOfView;

    public Transition PositionTransition;
    public Transition RotationTransition;
    public Transition FieldOfViewTransition;

    public Quaternion QuatRotation => Quaternion.Euler(Rotation);
}
