
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float walkSpeed = 5f;

    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float deceleration = 80f;
    [SerializeField] private float turnAcceleration = 110f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 1200f;
    [SerializeField] private bool instantRotation = false;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedForce = -2f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private PlayerCamera playerCamera;
    private CharacterController controller;

    private Vector2 moveInput;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;

    private float moveSpeed;

    public bool IsMoving => moveInput.sqrMagnitude > 0.01f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        moveSpeed = walkSpeed;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform != null)
        {
            playerCamera = cameraTransform.GetComponent<PlayerCamera>();
        }
    }

    // New Input System
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 direction = GetMoveDirection();

        Vector3 targetVelocity = direction * moveSpeed;

        float currentAcceleration;

        if (!IsMoving)
        {
            currentAcceleration = deceleration;
        }
        else
        {
            currentAcceleration = acceleration;

            if (horizontalVelocity.sqrMagnitude > 0.01f)
            {
                float dot = Vector3.Dot(
                    horizontalVelocity.normalized,
                    direction.normalized
                );

                if (dot < 0.75f)
                {
                    currentAcceleration = turnAcceleration;
                }
            }
        }

        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetVelocity,
            currentAcceleration * Time.deltaTime
        );

        // Gravidade
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedForce;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalVelocity = horizontalVelocity;
        finalVelocity.y = verticalVelocity;

        controller.Move(finalVelocity * Time.deltaTime);
    }

    private void HandleRotation()
    {
        // Rotaciona para onde a FreeLook está apontando
        Vector3 cameraForward = GetCameraForward();

        if (cameraForward.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(
            cameraForward,
            Vector3.up
        );

        if (instantRotation)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private Vector3 GetMoveDirection()
    {
        Vector2 input = Vector2.ClampMagnitude(moveInput, 1f);

        Vector3 forward = GetCameraForward();

        Vector3 right = Vector3.Cross(
            Vector3.up,
            forward
        ).normalized;

        Vector3 direction =
            forward * input.y +
            right * input.x;

        return Vector3.ClampMagnitude(direction, 1f);
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward;

        // Usa a orientação da FreeLook personalizada
        if (playerCamera != null)
        {
            forward = playerCamera.MovementForward;
        }
        else if (cameraTransform != null)
        {
            forward = cameraTransform.forward;
        }
        else
        {
            forward = Vector3.forward;
        }

        // Ignora a inclinação vertical da câmera
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.001f)
            return Vector3.forward;

        return forward.normalized;
    }

    public void Sprint()
    {
        moveSpeed = runSpeed;
    }

    public void Walk()
    {
        moveSpeed = walkSpeed;
    }

    private void OnDisable()
    {
        moveInput = Vector2.zero;
        horizontalVelocity = Vector3.zero;
    }
}
