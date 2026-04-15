using System;
using System.Collections.Generic;
using MonkeFrames.Compiler.Models;
using MonkeFrames.Editor.Utilities;
using UnityEngine;

using Keyframe = MonkeFrames.Compiler.Models.Keyframe;

namespace MonkeFrames.Editor.Components;

public class KeyframeManager : MonoBehaviour
{
    public static KeyframeManager Instance;
    public Project Project;

    public Dictionary<Keyframe, GameObject> Objects = new Dictionary<Keyframe, GameObject>();

    public void CreateOrb(Keyframe keyframe)
    {
        GameObject mainOrb = new GameObject($"Keyframe Visual {keyframe.GUID}");

        mainOrb.transform.position = keyframe.Position;
        mainOrb.transform.rotation = keyframe.QuatRotation;

        LineRenderer line = mainOrb.AddComponent<LineRenderer>();

        line.startColor = Color.blue;
        line.endColor = Color.blue;
        line.startWidth = 0.05f;
        line.endWidth = 0.15f;

        line.material.shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");

        line.SetPosition(0, mainOrb.transform.position);
        line.SetPosition(1, mainOrb.transform.position + (mainOrb.transform.forward * 0.25f));

        Objects.TryAdd(keyframe, mainOrb);
    }

    public void Start()
    {
        Instance = this;
        Debug.Log("[MonkeFrames::KeyframeManager] creating new project \"new project\"");

        Project = new Project("new project", Constants.Exporter);

        Debug.Log("[MonkeFrames::KeyframeManager] Keyframe manager is running");
    }

    public Keyframe CreateKeyframe(int replaceKeyframeIdx = -1, bool lookAtPlayer = false)
    {
        Keyframe k = new Keyframe();

        k.Position = CameraManager.Instance.Position;
        k.Rotation = CameraManager.Instance.Rotation.eulerAngles;
        k.FieldOfView = CameraManager.Instance.FieldOfView;

        if (lookAtPlayer)
            k.Rotation = Quaternion.LookRotation(GorillaTagger.Instance.headCollider.transform.position - k.Position).eulerAngles;

        k.Position = new Vector3(MathF.Round(k.Position.x, 2), MathF.Round(k.Position.y, 2), MathF.Round(k.Position.z, 2));
        k.Rotation = new Vector3(MathF.Round(k.Rotation.x, 2), MathF.Round(k.Rotation.y, 2), MathF.Round(k.Rotation.z, 2));

        if (replaceKeyframeIdx != -1)
        {
            Project.Keyframes.RemoveAt(replaceKeyframeIdx);
            Project.Keyframes.Insert(replaceKeyframeIdx, k);
        } else
        {
            Project.Keyframes.Add(k);
            UIManager.Instance.SelectedKeyframeIndex = Project.Keyframes.IndexOf(k);
        }

        CreateOrb(k);
        
        string posStr = $"Position: {UnityUtilities.Vector3ToString(k.Position)}";
        string rotStr = $"Rotation: {UnityUtilities.Vector3ToString(k.Rotation)}";

        UIManager.Instance.CurrentStatus = $"Created keyframe {Project.Keyframes.IndexOf(k)} with properties {{ {posStr}, {rotStr}, FOV: {k.FieldOfView} }} ";

        return k;
    }

    public void DeleteKeyframe(int index)
    {
        try {
            Objects[Project.Keyframes[index]].Destroy();
            Objects.Remove(Project.Keyframes[index]);
            Project.Keyframes.RemoveAt(index);
        } catch { };
        UIManager.Instance.SelectedKeyframeIndex = -1;
    }

    public void RefreshOrbs()
    {
        Objects.Values.ForEach(g => g.Destroy());
        Objects.Clear();
        Project.Keyframes.ForEach(CreateOrb);
    }
}