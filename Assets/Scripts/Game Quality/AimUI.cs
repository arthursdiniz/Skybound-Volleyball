using UnityEngine;

[DefaultExecutionOrder(200)]
public class AimUI : MonoBehaviour
{
    /*
    [Header("References")]
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform aimImage;

    private RectTransform canvasRect;

    private void Awake()
    {
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (playerAim == null || playerCamera == null)
            return;

         Vector3 screenPosition =
            playerCamera.WorldToScreenPoint(playerAim.AimPoint);

        bool visible =
            screenPosition.z > 0f &&
            screenPosition.x >= 0f &&
            screenPosition.x <= playerCamera.pixelWidth &&
            screenPosition.y >= 0f &&
            screenPosition.y <= playerCamera.pixelHeight;

        aimImage.gameObject.SetActive(visible);

        if (!visible)
            return;

        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            uiCamera,
            out Vector2 localPosition))
        {
            aimImage.anchoredPosition = localPosition;
        }
    }
    */
}
    