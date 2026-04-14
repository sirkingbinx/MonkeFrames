using System;
using UnityEngine;

namespace MonkeFrames.Compiler.Models;

public struct Keyframe
{
    public Vector3 Position;
    public Vector3 Rotation;
    public float FieldOfView;
    public Transition Transition = Transition.Linear;
    
    public string GUID = Guid.NewGuid().ToString();

    // The keyframe's rotation converted into a Quaternion.
    public readonly Quaternion QuatRotation => Quaternion.Euler(Rotation);

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

    public Keyframe() { }
}
