using System;
using System.Collections.Generic;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Core.Models;
using UnityEngine;

using Keyframe = MonkeFrames.Core.Models.Keyframe;

namespace MonkeFrames.Core.Components;

public class KeyframeManager : MonoBehaviour
{
    public static KeyframeManager Instance;

    public List<Keyframe> Keyframes = [];
    public Project CurrentProject;

    public void Start()
    {
        Instance = this;
        Debug.Log("[MonkeFrames::KeyframeManager] creating new project \"new project\"");

        CurrentProject = new Project("new project", Constants.Exporter);

        Debug.Log("[MonkeFrames::KeyframeManager] Keyframe manager is running");
    }

    public Keyframe CreateKeyframe(int replaceKeyframeIdx = -1)
    {
        Keyframe k = new Keyframe();

        k.Position = CameraManager.Instance.Position;
        k.Rotation = CameraManager.Instance.Rotation.eulerAngles;
        k.FieldOfView = CameraManager.Instance.FieldOfView;

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

        CurrentProject.Keyframes = Keyframes.ToArray();

        return k;
    }
}