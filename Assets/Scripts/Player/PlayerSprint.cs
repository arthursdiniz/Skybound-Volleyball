using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprint : MonoBehaviour
{
    private PlayerStamina playerStamina;
    private PlayerMovement playerMovement;

    public bool isSprinting;

    [Header("Input")]
    [SerializeField] private InputActionReference sprintAction;

    void Awake()
    {
        playerStamina = GetComponent<PlayerStamina>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        sprintAction.action.Enable();
    }

    void OnDisable()
    {
        sprintAction.action.Disable();
    }

    void Update()
    {
        isSprinting = sprintAction.action.IsPressed() && CanRun();

        if (isSprinting)
        {
            playerMovement.Sprint();
            playerStamina.DrainStamina();
        }
        else
        {
            playerMovement.Walk();
        }
    }

    public bool CanRun()
    {
        return playerStamina.currentStamina > 0f;
    }
}