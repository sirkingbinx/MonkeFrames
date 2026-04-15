using MonkeFrames.Editor.Utilities;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonkeFrames.Editor.Components;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Camera Camera;

    public Vector3 Position;
    public Quaternion Rotation;
    public float FieldOfView = 70f;

    private void Start()
    {
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;

        SetCinemachineState(false);

        Instance = this;

        Debug.Log("[MonkeFrames::CameraManager] All camera-based stuff should be set up");
    }

    public void SetModEnabled(bool enabled)
    {
        SetCinemachineState(!enabled);
        UIManager.Instance.ShowingUI = enabled;
    }

    private void FixedUpdate()
    {
        if (CinemachineState)
            return; // Don't mess with camera while not managing the freecam

        if (Camera == null)
            Camera = gameObject.GetComponent<Camera>();

        // Update values
        gameObject.transform.position = Position;
        gameObject.transform.rotation = Rotation;

        if (UIManager.Instance.AllowKeybinds)
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

        FieldOfView += Mouse.current.scroll.ReadValue().y * 5; // Increment by 5
        FieldOfView = NumberUtilities.Bounds(FieldOfView, 15, 150);

        Cursor.lockState = Mouse.current.rightButton.isPressed ? CursorLockMode.Locked : CursorLockMode.None;

        if (Mouse.current.rightButton.isPressed)
        {
            mousePos += Mouse.current.delta.ReadValue() / 5f;
            Rotation = Quaternion.Euler(-mousePos.y * 0.5f, mousePos.x * 0.5f, 0f);
        }

        Camera?.fieldOfView = FieldOfView;
    }

    Vector2 mousePos = new Vector2(0, 0);
    public bool CinemachineState = true;

    public void SetCinemachineState(bool enabled)
    {
        if (gameObject.TryGetComponent<CinemachineBrain>(out var cinemachine))
        {
            cinemachine.enabled = enabled;
        }

        CinemachineState = enabled;
        Debug.Log($"[MonkeFrames::CameraManager] Cinemachine on TPC is now {(enabled ? "activated" : "deactivated")}");
    }
}
