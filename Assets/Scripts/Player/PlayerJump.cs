using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private CharacterController controller;
    private PlayerStamina playerStamina;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Jump Charge")]
    [SerializeField] private float chargeSpeed = 0.6f;
    [SerializeField] private float maxJumpMultiplier = 4f;

    private float multiplyJumpHeight = 1f;

    [Header("Input")]
    [SerializeField] private InputActionReference jumpAction;

    [SerializeField] private bool isGrounded;

    private float verticalVelocity;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI jumpForceTxt;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerStamina = GetComponent<PlayerStamina>();
    }

    void OnEnable()
    {
        jumpAction.action.Enable();
    }

    void OnDisable()
    {
        jumpAction.action.Disable();
    }

    void Update()
    {
        // Mantém o personagem no chão
        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        // Carregamento do salto
        if (jumpAction.action.IsPressed() && isGrounded)
        {
            multiplyJumpHeight += chargeSpeed * Time.deltaTime;

            multiplyJumpHeight = Mathf.Clamp(
                multiplyJumpHeight,
                1f,
                maxJumpMultiplier
            );
        }

        // Executa o salto quando soltar o botão
        if (jumpAction.action.WasReleasedThisFrame())
        {
            if (isGrounded && playerStamina.currentStamina >= jumpHeight * 1.5f)
            {
                // Calcula o impulso vertical
                verticalVelocity = Mathf.Sqrt(
                    jumpHeight * multiplyJumpHeight * -2f * gravity
                );

                // Consome stamina e reinicia o delay
                playerStamina.SpendStamina(
                    jumpHeight * multiplyJumpHeight * 1.5f
                );
            }

            // Reseta o carregamento
            multiplyJumpHeight = 1f;
        }

        // Aplica gravidade
        verticalVelocity += gravity * Time.deltaTime;

        // Movimento vertical
        CollisionFlags flags = controller.Move(
            Vector3.up * verticalVelocity * Time.deltaTime
        );

        // Detecta contato com chão
        isGrounded = (flags & CollisionFlags.Below) != 0;

        // Interrompe subida ao bater no teto
        if ((flags & CollisionFlags.Above) != 0 && verticalVelocity > 0f)
        {
            verticalVelocity = 0f;
        }

        Debugs();
    }

    void Debugs()
    {
        if (jumpForceTxt == null)
            return;

        float jumpForce = Mathf.Sqrt(
            jumpHeight * multiplyJumpHeight * -2f * gravity
        );

        jumpForceTxt.text = "Jump Force : " + jumpForce.ToString("F2");
    }
}