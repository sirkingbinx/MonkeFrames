using System.Collections.Generic;
using MonkeFrames.Compiler.Serializable;
using MonkeFrames.Models;

namespace MonkeFrames.Compiler.Models;

public struct Project
{
    public string ProjectName;
    public string ProjectVersion;
    public SavableKeyframe[] Keyframes;

    public static Project CreateFromKeyframes(string name, List<Keyframe> keyframes) {
        Project project = new Project(name);
        List<SavableKeyframe> savedKeyframes = [];

        foreach (Keyframe k in keyframes) {
            savedKeyframes.Add(SavableKeyframe.CreateFromKeyframe(k));
        }

        project.Keyframes = savedKeyframes.ToArray();

        return project;
    }

    public void UpdateState(IEnumerable<Keyframe> keyframes)
    {
        List<SavableKeyframe> newKeyframes = [];

        foreach (Keyframe k in keyframes)
        {
            newKeyframes.Add(SavableKeyframe.CreateFromKeyframe(k));
        }

        Keyframes = newKeyframes.ToArray();
    }

    public Project(string name) {
        ProjectName = name;
        ProjectVersion = Constants.Version;
    }
}