
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallNetCollision : MonoBehaviour
{
    private Rigidbody rb;
    private bool hitNet = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitNet) return;

        if (collision.gameObject.CompareTag("Net"))
        {
            HitNet();
        }
    }

    private void HitNet()
    {
        hitNet = true;

        // Remove toda a velocidade da bola
        rb.linearVelocity = Vector3.zero;

        // Interrompe qualquer rotação
        rb.angularVelocity = Vector3.zero;

        // A bola continua caindo pela gravidade
        rb.useGravity = true;

        Debug.Log("A bola bateu na rede!");
    }
}
