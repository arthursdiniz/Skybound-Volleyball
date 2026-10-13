
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform ball;

    [Header("Input")]
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference ballCamAction;

    [Header("Camera Settings")]
    [SerializeField] private float distance = 6f;
    [SerializeField] private float height = 1.5f;
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 70f;

    [Header("Sensitivity")]
    [SerializeField] private float mouseSensitivity = 0.12f;
    [SerializeField] private float gamepadSensitivity = 140f;

    [Header("Follow")]
    [SerializeField] private float followSmoothTime = 0f;

    private float yaw;
    private float pitch;

    private bool ballCamActive;

    private Vector3 smoothPivot;
    private Vector3 followVelocity;

    // Direções estáveis para a movimentação do jogador
    public Vector3 MovementForward =>
        Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;

    public Vector3 MovementRight =>
        Quaternion.Euler(0f, yaw, 0f) * Vector3.right;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false ;
    }

    private void Awake()
    {
        yaw = transform.eulerAngles.y;
        pitch = Mathf.DeltaAngle(0f, transform.eulerAngles.x);

        if (player != null)
            smoothPivot = player.position + Vector3.up * height;
    }

    private void OnEnable()
    {
        lookAction.action.Enable();
        ballCamAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
        ballCamAction.action.Disable();
    }

    private void LateUpdate()
    {
        if (player == null)
            return;

        if (ballCamAction.action.WasPressedThisFrame())
            ballCamActive = !ballCamActive;

        Quaternion cameraRotation;

        if (ballCamActive && ball != null)
        {
            Vector3 pivot = player.position + Vector3.up * height;
            Vector3 direction = ball.position - pivot;

            if (direction.sqrMagnitude > 0.001f)
                cameraRotation = Quaternion.LookRotation(direction);
            else
                cameraRotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        else
        {
            HandleFreeLook();
            cameraRotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        UpdateCamera(cameraRotation);
    }

    private void HandleFreeLook()
    {
        Vector2 look = lookAction.action.ReadValue<Vector2>();

        bool usingMouse =
            lookAction.action.activeControl?.device is Mouse;

        float sensitivity = usingMouse
            ? mouseSensitivity
            : gamepadSensitivity * Time.deltaTime;

        yaw += look.x * sensitivity;
        pitch -= look.y * sensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void UpdateCamera(Quaternion rotation)
    {
        Vector3 targetPivot =
            player.position + Vector3.up * height;

        if (followSmoothTime <= 0f)
        {
            smoothPivot = targetPivot;
        }
        else
        {
            smoothPivot = Vector3.SmoothDamp(
                smoothPivot,
                targetPivot,
                ref followVelocity,
                followSmoothTime
            );
        }

        Vector3 cameraPosition =
            smoothPivot - rotation * Vector3.forward * distance;

        transform.SetPositionAndRotation(
            cameraPosition,
            rotation
        );
    }
}
