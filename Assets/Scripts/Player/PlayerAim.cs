using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] private Transform aimOrigin;
    [SerializeField] private float aimDistance = 1.2f;

    public Vector3 AimDirection => transform.forward;

    public Vector3 AimPoint =>
        aimOrigin.position + AimDirection * aimDistance;

    private void OnDrawGizmosSelected()
    {
        if (aimOrigin == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(aimOrigin.position, AimPoint);
        Gizmos.DrawSphere(AimPoint, 0.08f);
    }
}