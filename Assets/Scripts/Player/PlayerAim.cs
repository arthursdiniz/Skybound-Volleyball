
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private RectTransform crosshair;
    [SerializeField] private Canvas crosshairCanvas;

    [Header("Aim Settings")]
    [SerializeField] private float aimDistance = 40f;
    [SerializeField] private LayerMask aimLayers = ~0;

    private void Awake()
    {
        if (gameplayCamera == null)
            gameplayCamera = Camera.main;
    }


    public Vector3 GetAimDirection(Vector3 ballPosition)
    {
        if (gameplayCamera == null)
            return transform.forward;

        Vector2 screenPosition = new Vector2(
            Screen.width * 0.5f,
            Screen.height * 0.5f
        );

        if (crosshair != null)
        {
            Camera uiCamera = null;

            if (crosshairCanvas != null &&
                crosshairCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                uiCamera = crosshairCanvas.worldCamera;
            }

            screenPosition =
                RectTransformUtility.WorldToScreenPoint(
                    uiCamera,
                    crosshair.position
                );
        }

        Ray ray = gameplayCamera.ScreenPointToRay(screenPosition);

        // Alvo virtual distante, sem colisões.
        Vector3 targetPoint = ray.GetPoint(60f);

        return (targetPoint - ballPosition).normalized;
    }

}
