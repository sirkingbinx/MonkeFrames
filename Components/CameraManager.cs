using Photon.Voice;
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
        Camera = gameObject.GetComponent<Camera>();

        Position = gameObject.transform.position;
        Rotation = gameObject.transform.rotation;

        SetCinemachineState(false);

        Instance = this;
    }

    private void Update()
    {
        float speed = Keyboard.current.shiftKey.isPressed ? 0.05f : 0.25f;

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

        // Cursor.lockState = Mouse.current.rightButton.isPressed ? CursorLockMode.Locked : CursorLockMode.None;

        if (Mouse.current.rightButton.isPressed)
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed);
        }

        // Update values
        Camera.fieldOfView = FieldOfView;

        gameObject.transform.position = Position;
        gameObject.transform.rotation = Rotation;
    }

    public void SetCinemachineState(bool enabled)
    {
        if (gameObject.TryGetComponent<CinemachineBrain>(out var cinemachine))
        {
            cinemachine.enabled = false;
        }
    }
}
