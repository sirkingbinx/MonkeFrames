using System.Collections;
using GorillaExtensions;
using MonkeFrames.Editor.Utilities;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

using Keyframe = MonkeFrames.Compiler.Models.Keyframe;

namespace MonkeFrames.Editor.Components;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Camera Camera;

    public Vector3 Position;
    public Quaternion Rotation;
    public float FieldOfView = 70f;

    public bool InPlayback = false;

    public CameraManager()
    {
        Instance = this;
    }

    private void Start()
    {
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;

        SetCinemachineState(false);

        Debug.Log("[MonkeFrames::CameraManager] All camera-based stuff should be set up");
    }

    public void SetModEnabled(bool enabled)
    {
        SetCinemachineState(!enabled);

        if (enabled)
            KeyframeManager.Instance.RefreshOrbs();
        else
            KeyframeManager.Instance.DeleteOrbs();

        UIManager.Instance.ShowingUI = enabled;
    }

    private void LateUpdate()
    {
        if (CinemachineState)
            return; // Don't mess with camera while not managing the freecam

        if (Camera == null)
            Camera = gameObject.GetComponent<Camera>();

        // Update values
        gameObject.transform.position = Position;
        gameObject.transform.rotation = Rotation;

        Camera?.fieldOfView = FieldOfView;

        if (UIManager.Instance.AllowKeybinds && !InPlayback)
        {
            float speed = 0.05f;

            if (Keyboard.current.shiftKey.isPressed)
                speed = 0.25f;
            if (Keyboard.current.ctrlKey.isPressed)
                speed = 0.005f;

            // Check keybinds
            if (Keyboard.current.wKey.isPressed)
                Position += transform.forward * speed;

            if (Keyboard.current.sKey.isPressed)
                Position -= transform.forward * speed;

            if (Keyboard.current.dKey.isPressed)
                Position += transform.right * speed;

            if (Keyboard.current.aKey.isPressed)
                Position -= transform.right * speed;

            if (Keyboard.current.eKey.isPressed)
                Position += transform.up * speed;

            if (Keyboard.current.qKey.isPressed)
                Position -= transform.up * speed;
        }

        if (!InPlayback)
        {
            FieldOfView += Mouse.current.scroll.ReadValue().y * 5; // Increment by 5
            FieldOfView = NumberUtilities.Bounds(FieldOfView, 15, 150);

            Cursor.lockState = Mouse.current.rightButton.isPressed ? CursorLockMode.Locked : CursorLockMode.None;

            if (Mouse.current.rightButton.isPressed)
            {
                mousePos += Mouse.current.delta.ReadValue() / 5f;
                Rotation = Quaternion.Euler(-mousePos.y * 0.5f, mousePos.x * 0.5f, 0f);
            }
        }

        if (InPlayback && Keyboard.current.spaceKey.wasPressedThisFrame)
            StopPlayback();
    }

    Vector2 mousePos = new Vector2(0, 0);
    public bool CinemachineState = true;

    public void SetCinemachineState(bool enabled)
    {
        CinemachineBrain brain = gameObject.GetComponent<CinemachineBrain>();
        gameObject.transform.Find("CM vcam1").gameObject.SetActive(enabled);
        brain.enabled = enabled;

        // GameObject gameCamera = GameObject.Find("LCKTablet");
        // gameCamera.SetActive(enabled);

        CinemachineState = enabled;
        Debug.Log($"[MonkeFrames::CameraManager] Cinemachine on TPC is now {(enabled ? "activated" : "deactivated")}");
    }

    // Playback shit
    // Don't touch this its weird

    int playbackPosition = 0;
    int playbackEnding;

    IEnumerator PlaybackCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f / KeyframeManager.Instance.Project.FPS);

        while (InPlayback)
        {
            if (playbackEnding == playbackEnding - 1)
            {
                InPlayback = false;
                UIManager.Instance.ShowingUI = true;
                StopCoroutine("PlaybackCoroutine");
            }

            Keyframe currentFrame = KeyframeManager.Instance.Project.CompiledKeyframes[playbackPosition];

            Position = currentFrame.Position;
            Rotation = currentFrame.QuatRotation;
            FieldOfView = currentFrame.FieldOfView;

            playbackPosition++;
            yield return wait;
        }
    }

    public void StartPlayback()
    {
        InPlayback = true;
        UIManager.Instance.ShowingUI = false;
        KeyframeManager.Instance.DeleteOrbs();
        playbackPosition = 0;
        playbackEnding = KeyframeManager.Instance.Project.CompiledKeyframes.Count;

        StartCoroutine("PlaybackCoroutine");
    }

    public void StopPlayback()
    {
        InPlayback = false;
        UIManager.Instance.ShowingUI = true;
        KeyframeManager.Instance.RefreshOrbs();
        playbackPosition = 0;

        StopCoroutine("PlaybackCoroutine");
    }
}
