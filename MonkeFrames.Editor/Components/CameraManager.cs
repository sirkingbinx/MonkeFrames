using GorillaNetworking;
using MonkeFrames.Editor.Utilities;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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

    public GameObject CameraMarker;

    public bool InPlayback = false;
    public bool Manual = false;

    public CameraManager()
    {
        Instance = this;
    }

    private void Start()
    {
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;

        UnityEngine.Debug.Log("[MonkeFrames::CameraManager] All camera-based stuff should be set up");
    }

    public void SetModEnabled(bool enabled)
    {
        SetCinemachineState(!enabled);

        if (enabled)
            KeyframeManager.Instance.RefreshOrbs();
        else
            KeyframeManager.Instance.DeleteOrbs();

        if (CameraMarker == null)
            CameraMarker = KeyframeManager.Instance.CreateOrb("MonkeFrames Spectator Camera");

        CameraMarker.SetActive(enabled);
        UIManager.Instance.Drawing = enabled;
    }

    private void LateUpdate()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame && CinemachineState)
            SetModEnabled(true);

        PhotonNetworkController.Instance.disableAFKKick = !CinemachineState;

        if (CinemachineState)
            return;

        if (Camera == null)
            Camera = gameObject.GetComponent<Camera>();

        // Update values
        if (!Manual)
        {
            gameObject.transform.position = Position;
            gameObject.transform.rotation = Rotation;

            CameraMarker.transform.position = Position;
            CameraMarker.transform.rotation = Rotation;

            Camera?.fieldOfView = FieldOfView;
        }

        if (!InPlayback)
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

            if (Keyboard.current.leftArrowKey.isPressed)
            {
                Vector3 eulers = Rotation.eulerAngles;
                eulers.z -= speed;

                Rotation = Quaternion.Euler(eulers);
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                Vector3 eulers = Rotation.eulerAngles;
                eulers.z += speed;

                Rotation = Quaternion.Euler(eulers);
            }

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

        CinemachineState = enabled;
        UnityEngine.Debug.Log($"[MonkeFrames::CameraManager] Cinemachine on TPC is now {(enabled ? "activated" : "deactivated")}");
    }

    int playbackPosition = 0;
    int playbackEnding;

    public bool doRecording = false;
    public string recordingOutputFolder;

    public Texture2D tex;

    IEnumerator PlaybackCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f / KeyframeManager.Instance.Project.FPS);

        while (InPlayback)
        {
            if (playbackPosition == playbackEnding - 1)
            {
                InPlayback = false;
                UIManager.Instance.Drawing = true;
                KeyframeManager.Instance.RefreshOrbs();
                playbackPosition = 0;
                
                StopCoroutine("PlaybackCoroutine");

                if (doRecording)
                    BuildVideo();
            }

            Keyframe currentFrame = KeyframeManager.Instance.Project.CompiledKeyframes[playbackPosition];

            Position = currentFrame.Position;
            Rotation = currentFrame.QuatRotation;
            FieldOfView = currentFrame.FieldOfView;

            if (doRecording)
            {
                tex = ScreenCapture.CaptureScreenshotAsTexture();
                File.WriteAllBytes(Path.Combine(recordingOutputFolder, $"frame_{playbackPosition:D4}.jpg"), tex.EncodeToJPG());
            }

            playbackPosition++;
            yield return wait;
        }
    }

    public void StartRecording()
    {
        recordingOutputFolder = Path.Combine(Constants.DataFolder, "tmp", $"{KeyframeManager.Instance.Project.Name}.exdat");
        Directory.CreateDirectory(recordingOutputFolder);

        doRecording = true;

        StartPlayback();
    }

    public void BuildVideo()
    {
        var outputMp4 = Path.Combine(Constants.DataFolder, "exports", KeyframeManager.Instance.Project.Name + ".mp4");
        var ffmpegBuildProcess = Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(Constants.MonkeFramesAssemblyFolder, "ffmpeg.exe"),
            Arguments = $"-framerate {KeyframeManager.Instance.Project.FPS} -y -i frame_%04d.jpg -c:v libx264 -pix_fmt yuv420p \"{outputMp4}\"",
            WorkingDirectory = recordingOutputFolder,
            UseShellExecute = false,
            CreateNoWindow = false
        });

        ffmpegBuildProcess.WaitForExit();

        Process.Start(new ProcessStartInfo()
        {
            FileName = Path.Combine(Constants.DataFolder, "exports", KeyframeManager.Instance.Project.Name + ".mp4"),
            UseShellExecute = true
        });
    }

    public void StartPlayback()
    {
        InPlayback = true;
        UIManager.Instance.Drawing = false;
        KeyframeManager.Instance.DeleteOrbs();
        playbackPosition = 0;
        playbackEnding = KeyframeManager.Instance.Project.CompiledKeyframes.Count;

        StartCoroutine("PlaybackCoroutine");
    }

    public void StopPlayback()
    {
        InPlayback = false;
        UIManager.Instance.Drawing = true;
        KeyframeManager.Instance.RefreshOrbs();
        playbackPosition = 0;

        StopCoroutine("PlaybackCoroutine");
    }
}
