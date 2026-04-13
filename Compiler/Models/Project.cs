using System.Collections.Generic;
using MonkeFrames.Compiler.Serializable;

namespace MonkeFrames.Compiler.Models;

public struct Project
{
    public string MonkeFramesVersion;
    public SavableKeyframe[] Keyframes;

    public static Project CreateFromKeyframes(List<Keyframe> keyframes) {
        List<SavableKeyframe> nkf = [];

        foreach (Keyframe k in keyframes) {
            nkf.Add(SavableKeyframe.CreateFromKeyframe(k));
        }

        Keyframes = nkf.ToArray();
        MonkeFramesVersion = Constants.Version;
    }
}