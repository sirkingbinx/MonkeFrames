using UnityEngine;
using Unity.Cinemachine;

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
