using MonkeFrames.Models;
using UnityEngine;

using Keyframe = MonkeFrames.Models.Keyframe;

namespace MonkeFrames.Compiler.Serializable;

public struct SavableKeyframe
{
    public Vec3 Position;
    public Vec3 Rotation;
    public float FieldOfView;
    public Transition Transition;

    public static SavableKeyframe CreateFromKeyframe(Keyframe k) {
        SavableKeyframe nk = new SavableKeyframe();

        nk.Position = new Vec3(k.Position.x, k.Position.y, k.Position.z);
        nk.Rotation = new Vec3(k.Rotation.x, k.Rotation.y, k.Rotation.z);
        nk.FieldOfView = k.FieldOfView;
        nk.Transition = k.Transition;

        return nk;
    }

    public Keyframe ToKeyframe()
    {
        Keyframe k = new Keyframe();

        k.Position = new Vector3(Position.X, Position.Y, Position.Z);
        k.Rotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z);
        k.FieldOfView = FieldOfView;
        k.Transition = Transition;

        return k;
    }
}