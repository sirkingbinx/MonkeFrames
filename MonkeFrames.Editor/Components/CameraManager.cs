using GorillaNetworking;
using MonkeFrames.Editor.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
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

        Console.WriteLine("[MonkeFrames::CameraManager] All camera-based stuff should be set up");
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
        Console.WriteLine($"[MonkeFrames::CameraManager] Cinemachine on TPC is now {(enabled ? "activated" : "deactivated")}");
    }

    int playbackPosition = 0;
    int playbackEnding;

    public bool doRecording = false;

    public Texture2D tex2d;
    public List<Keyframe> kCache;

    public RenderTexture renderTexture;
    public byte[] rBuffer;

    public BinaryWriter frameStream;
    public Process ffmpegProcess;

    public string outputMp4 => Path.Combine(Constants.DataFolder, "exports", KeyframeManager.Instance.Project.Name + ".mp4");

    IEnumerator PlaybackCoroutine()
    {
        var waitFrame = new WaitForEndOfFrame();

        while (InPlayback)
        {
            yield return waitFrame;

            if (playbackPosition >= playbackEnding - 1)
            {
                InPlayback = false;
                UIManager.Instance.Drawing = true;
                KeyframeManager.Instance.RefreshOrbs();
                playbackPosition = 0;
                doRecording = false;

                StopCoroutine("PlaybackCoroutine");

                UIManager.Instance.Status = "Finishing encoding..";

                frameStream.Flush();
                frameStream.Close();
                frameStream = null;

                ffmpegProcess.WaitForExit();
                ffmpegProcess.Dispose();
                ffmpegProcess = null;

                renderTexture.Release();
                Destroy(renderTexture);

                Process.Start("explorer.exe", $"/select,\"{outputMp4}\"");
            }

            Keyframe currentFrame = kCache[playbackPosition];

            Position = currentFrame.Position;
            Rotation = currentFrame.QuatRotation;
            FieldOfView = currentFrame.FieldOfView;

            if (doRecording) {
                ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);

                var request = AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGBA32);

                while (!request.done)
                    yield return null;

                if (request.hasError)
                    continue;

                var nativeArray = request.GetData<byte>();
                nativeArray.CopyTo(rBuffer);

                frameStream.Write(rBuffer, 0, rBuffer.Length);
                frameStream.Flush();

                Console.WriteLine($"Ffmpeg encode ... Flush frame {playbackPosition + 1}");
            }
            
            playbackPosition++;
            yield return new WaitForSeconds(doRecording ? 0f : (1f / KeyframeManager.Instance.Project.FPS));
        }
    }

    public void StartRecording()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        rBuffer = new byte[Screen.width * Screen.height * 4];

        StartFfmpegEncoder();
        doRecording = true;
        StartPlayback();
    }

    public void StartFfmpegEncoder()
    {
        var project = KeyframeManager.Instance.Project;

        Console.WriteLine("==== FFMPEG ENCODE STATS ====");

        string encoder = HEncodeUtilities.GetGoodEncoder();
        Console.WriteLine($"Encoding:     {encoder}");
        Console.WriteLine($"GPU dev name: {SystemInfo.graphicsDeviceName}");

        string arguments = $"-f rawvideo -pix_fmt rgba -s {Screen.width}x{Screen.height} -r {project.FPS} -i - " +
                           $"-c:v libx264 -pix_fmt yuv420p -y \"{outputMp4}\" " +
                           $"-loglevel quiet -preset ultrafast -c:v {encoder} -threads 0 " +
                           $"-crf 28";

        Console.WriteLine($"Arguments: {arguments}");

        Console.WriteLine("==== OK I'M DONE ====");

        ffmpegProcess = Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(Constants.MonkeFramesAssemblyFolder, "ffmpeg.exe"),
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true
        });

        frameStream = new BinaryWriter(ffmpegProcess.StandardInput.BaseStream);

        Task.Run(() => {
            IntPtr hwnd = GetActiveWindow();
            MessageBox(hwnd, "Your video is currently being processed. Please wait for the UI to appear before continuing.", "MonkeFrames Editor", 0x00000000 | 0x00000060);
        });
    }

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    public void StartPlayback()
    {
        kCache = KeyframeManager.Instance.Project.CompiledKeyframes;
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
