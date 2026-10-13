using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;

    [SerializeField] private float staminaDrainRate = 30f;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float staminaRegenDelay = 2f;

    [Header("UI")]
    [SerializeField] private Image staminaBar;

    private Coroutine staminaCoroutine;
    private float lastStaminaUseTime;

    void Awake()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        // Atualiza a barra de stamina
        if (staminaBar != null)
        {
            staminaBar.fillAmount = Mathf.Clamp01(
                currentStamina / maxStamina
            );
        }
    }

    // Consumo contínuo (Sprint)
    public void DrainStamina()
    {
        SpendStamina(staminaDrainRate * Time.deltaTime);
    }

    // Consumo de stamina por qualquer ação
    public void SpendStamina(float amount)
    {
        if (amount <= 0f || currentStamina <= 0f)
            return;

        currentStamina = Mathf.Max(
            0f,
            currentStamina - amount
        );

        // Reinicia o delay sempre que gastar stamina
        lastStaminaUseTime = Time.time;

        // Garante que apenas uma Coroutine esteja ativa
        if (staminaCoroutine == null)
        {
            staminaCoroutine = StartCoroutine(RegenerateStamina());
        }
    }

    private IEnumerator RegenerateStamina()
    {
        while (currentStamina < maxStamina)
        {
            // Espera o tempo necessário desde o último gasto
            if (Time.time - lastStaminaUseTime >= staminaRegenDelay)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;

                currentStamina = Mathf.Min(
                    currentStamina,
                    maxStamina
                );
            }

            yield return null;
        }

        staminaCoroutine = null;
    }
}