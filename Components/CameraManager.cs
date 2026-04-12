using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonkeFrames.Components;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Camera Camera;

    public Vector3 Position;
    public Quaternion Rotation;
    public float FieldOfView = 70f;

    public bool Playing = false;
    public float Timestamp = 0;

    private void Start()
    {
        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;

        SetCinemachineState(false);

        Instance = this;

        Debug.Log("[MonkeFrames::CameraManager] All camera-based stuff should be set up");
    }

    private void Update()
    {
        float speed = Keyboard.current.shiftKey.isPressed ? 0.25f : 0.05f;

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

        Cursor.lockState = Mouse.current.rightButton.isPressed ? CursorLockMode.Locked : CursorLockMode.None;

        if (Mouse.current.rightButton.isPressed)
        {
            mousePos += Mouse.current.delta.ReadValue() / 5f;
            Rotation = Quaternion.Euler(-mousePos.y * 0.5f, mousePos.x * 0.5f, 0f);
        }

        // Update values
        gameObject.transform.position = Position;
        gameObject.transform.rotation = Rotation;
    }

    Vector2 mousePos = new Vector2(0, 0);

    public void SetCinemachineState(bool enabled)
    {
        if (gameObject.TryGetComponent<CinemachineBrain>(out var cinemachine))
        {
            cinemachine.enabled = false;
        }

        Debug.Log($"[MonkeFrames::CameraManager] Cinemachine on TPC is now {(enabled ? "activated" : "deactivated")}");
    }
}
