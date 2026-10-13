using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class BallCameraController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera ballCamera;

    [Header("Input")]
    [SerializeField] private InputActionReference ballCamAction;

    private bool isBallCamActive = false;

    private void OnEnable()
    {
        ballCamAction.action.Enable();
    }

    private void OnDisable()
    {
        ballCamAction.action.Disable();
    }

    private void Start()
    {
        UpdateCameraMode();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        if (ballCamAction.action.WasPressedThisFrame())
        {
            ToggleBallCam();
        }
    }

    private void ToggleBallCam()
    {
        isBallCamActive = !isBallCamActive;

        UpdateCameraMode();
    }

    private void UpdateCameraMode()
    {
        normalCamera.Priority = isBallCamActive ? 10 : 20;
        ballCamera.Priority = isBallCamActive ? 20 : 10;
    }
}