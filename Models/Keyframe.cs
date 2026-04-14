using System;
using MonkeFrames.Components;
using UnityEngine;

namespace MonkeFrames.Models;

public struct Keyframe
{
    public string KeyframeGUID;

    public Vector3 Position;
    public Vector3 Rotation;
    public float FieldOfView;

    public Transition Transition;

    // The keyframe's rotation converted into a Quaternion.
    public readonly Quaternion QuatRotation => Quaternion.Euler(Rotation);

    // Playback information
    public float Playback_RelationalStart;
    public float Playback_RelationalEnd;

    public static bool operator ==(Keyframe? left, Keyframe? right)
    {
        if (left is null) return right is null;
        return left.Value.KeyframeGUID == right.Value.KeyframeGUID;
    }

    public static bool operator !=(Keyframe? left, Keyframe? right) => !(left == right);

    public override bool Equals(object obj)
    {
        if (obj is not Keyframe other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return this.KeyframeGUID == other.KeyframeGUID;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public Keyframe()
    {
        KeyframeGUID = Guid.NewGuid().ToString();
    }
}
