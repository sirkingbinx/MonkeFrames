using System;
using System.Collections.Generic;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Models;
using UnityEngine;

using Keyframe = MonkeFrames.Models.Keyframe;

namespace MonkeFrames.Components;

public class KeyframeManager : MonoBehaviour
{
    public static KeyframeManager Instance;

    public List<Keyframe> Keyframes = [];
    public Project CurrentProject;
    public float TotalDuration;

    public void Start()
    {
        Instance = this;
        Debug.Log("[MonkeFrames::KeyframeManager] creating new project \"new project\"");

        CurrentProject = new Project("new project");

        Debug.Log("[MonkeFrames::KeyframeManager] Keyframe manager is running");
    }

    public bool TryGetKeyframe(float timestamp, out Keyframe keyframe)
    {
        if (timestamp > TotalDuration || timestamp < 0) {
            keyframe = new Keyframe();
            return false; // Looking for timestamp outside of playback range
        }

        foreach (Keyframe currentKeyframe in Keyframes) {
            float start = currentKeyframe.Playback_RelationalStart;
            float end = currentKeyframe.Playback_RelationalEnd;

            if (timestamp >= start && timestamp <= end)
            {
                keyframe = currentKeyframe;
                return true;
            }
        }

        keyframe = new Keyframe();
        return false; // Unknown error
    }

    public Keyframe CreateKeyframe(int replaceKeyframeIdx = -1)
    {
        Keyframe k = new()
        {
            Position = CameraManager.Instance.Position,
            Rotation = CameraManager.Instance.Rotation.eulerAngles,
            FieldOfView = CameraManager.Instance.FieldOfView,

            KeyframeGUID = Guid.NewGuid().ToString(),

            Transition = Transition.Linear
        };

        k.Position = new Vector3(MathF.Round(k.Position.x, 2), MathF.Round(k.Position.y, 2), MathF.Round(k.Position.z, 2));
        k.Rotation = new Vector3(MathF.Round(k.Rotation.x, 2), MathF.Round(k.Rotation.y, 2), MathF.Round(k.Rotation.z, 2));

        if (replaceKeyframeIdx != -1)
        {
            Keyframes.RemoveAt(replaceKeyframeIdx);
            Keyframes.Insert(replaceKeyframeIdx, k);
        } else
        {
            Keyframes.Add(k);
        }

        return k;
    }
}