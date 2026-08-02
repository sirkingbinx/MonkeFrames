using MonkeFrames.Editor.Components;
using MonkeFrames.Editor.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonkeFrames.Editor.Windows;

public class Player : IEditorWindow
{
    public string Name => "Player";
    public Rect Rect => new Rect(500, 100, 1200, 80);

    public Compiler.Models.Project Project => KeyframeManager.Instance.Project;

    public bool IsPlaying = false;
    public int HeadPosition = 0;
    private int _LastHeadPosition = 0;

    public void OnDraw()
    {
        if (CameraManager.Instance.InPlayback)
            return;
        
        HeadPosition = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(10, 30, Rect.width - 20, 20), HeadPosition, 0, Project.CompiledKeyframes.Count));

        if (GUI.Button(new Rect(10, 55, 100, 20), IsPlaying ? "Pause" : "Play"))
            IsPlaying = !IsPlaying;

        GUI.Label(new Rect(120, 55, 250, 20), $"Frame {HeadPosition + 1}/{Project.CompiledKeyframes.Count} ({Project.FPS} FPS)");
        
        if (HeadPosition != _LastHeadPosition)
        {
            IsPlaying = false;
            CameraManager.Instance.Position = Project.CompiledKeyframes[HeadPosition].Position;
            CameraManager.Instance.Rotation = Project.CompiledKeyframes[HeadPosition].QuatRotation;
            CameraManager.Instance.FieldOfView = Project.CompiledKeyframes[HeadPosition].FieldOfView;
        }

        if (IsPlaying) {
            HeadPosition++;

            if (HeadPosition >= Project.CompiledKeyframes.Count)
                HeadPosition = 0;

            CameraManager.Instance.Position = Project.CompiledKeyframes[HeadPosition].Position;
            CameraManager.Instance.Rotation = Project.CompiledKeyframes[HeadPosition].QuatRotation;
            CameraManager.Instance.FieldOfView = Project.CompiledKeyframes[HeadPosition].FieldOfView;
        }

        _LastHeadPosition = HeadPosition;
    }

    public void OnOpen()
    {
        Project.Build().Wait();
    }
}