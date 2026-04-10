using System;
using System.Collections.Generic;
using MonkeFrames.Models;
using UnityEngine;

using Keyframe = MonkeFrames.Models.Keyframe;

namespace MonkeFrames.Components;

public class KeyframeManager : MonoBehaviour
{
    public static KeyframeManager Instance;

    public List<Keyframe> Keyframes = [];
    public float TotalDuration;

    public void Start()
    {
        Instance = this;
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
        UIManager.Instance.CurrentTask = "Creating keyframe";

        Keyframe k = new()
        {
            Position = CameraManager.Instance.Position,
            Rotation = CameraManager.Instance.Rotation.eulerAngles,
            FieldOfView = CameraManager.Instance.FieldOfView,

            KeyframeGUID = Guid.NewGuid().ToString(),

            Transition = Transition.Linear
        };

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