using Newtonsoft.Json;
using System;
using UnityEngine;

namespace MonkeFrames.Compiler.Models;

public struct Keyframe
{
    public Vector3 Position = new Vector3(0, 0, 0);
    public Vector3 Rotation = new Vector3(0, 0, 0);
    public float FieldOfView = 60;
    public Transition Transition = Transition.Linear;
    
    [JsonIgnore]
    public string GUID = Guid.NewGuid().ToString();

    [JsonIgnore]
    public bool Compiled = false;

    // The keyframe's rotation converted into a Quaternion.
    [JsonIgnore]
    public readonly Quaternion QuatRotation => Quaternion.Euler(Rotation);

    public static bool operator ==(Keyframe? left, Keyframe? right)
    {
        if (left is null) return right is null;
        return left.Value.GUID == right.Value.GUID;
    }

    public static bool operator !=(Keyframe? left, Keyframe? right) => !(left == right);

    public override bool Equals(object obj)
    {
        if (obj is not Keyframe other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return this.GUID == other.GUID;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public Keyframe() { }
}
