
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpike : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawn;

    private PlayerAim playerAim;

    [Header("Spike Settings")]
    [SerializeField] private float spikeSpeed = 22f;

    [Header("Spike Accuracy")]
    [Range(0f, 20f)]
    [SerializeField] private float spikeSpread = 3f;

    private void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
    }

    private void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Spike();
        }
    }

    private void Spike()
    {
        if (ballPrefab == null ||
            ballSpawn == null ||
            playerAim == null)
            return;

        Vector3 spawnPosition = ballSpawn.position;

        GameObject ball = Instantiate(
            ballPrefab,
            spawnPosition,
            Quaternion.identity
        );

        if (!ball.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Debug.LogError("Ball prefab sem Rigidbody!");
            Destroy(ball);
            return;
        }

        Vector3 direction =
    playerAim.GetAimDirection(spawnPosition);

        // Gera uma variação aleatória em torno da mira
        Vector2 randomSpread = Random.insideUnitCircle * spikeSpread;

        // Aplica a variação horizontal e vertical
        Quaternion spreadRotation = Quaternion.Euler(
            randomSpread.y,
            randomSpread.x,
            0f
        );

        // Rotaciona a direção em relação à própria mira
        direction = (
            Quaternion.LookRotation(direction) *
            spreadRotation *
            Vector3.forward
        ).normalized;

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(
            direction * spikeSpeed,
            ForceMode.VelocityChange
        );

        // Evita que a bola bata imediatamente no jogador.
        Collider[] ballColliders =
            ball.GetComponentsInChildren<Collider>();

        Collider[] playerColliders =
            GetComponentsInChildren<Collider>();

        foreach (Collider ballCollider in ballColliders)
        {
            foreach (Collider playerCollider in playerColliders)
            {
                Physics.IgnoreCollision(
                    ballCollider,
                    playerCollider
                );
            }
        }

        Debug.DrawRay(
            spawnPosition,
            direction * 10f,
            Color.red,
            3f
        );

        Debug.Log(
            $"Spike Direction: {direction} | " +
            $"Speed: {rb.linearVelocity.magnitude}"
        );
    }
}
