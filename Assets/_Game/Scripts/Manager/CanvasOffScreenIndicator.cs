using UnityEngine;

public class CanvasOffScreenIndicator : UICanvas {

    [Header("Indicators")]
    [SerializeField] private OffscreenIndicator[] indicatorArray;

    [Header("Screen Edge")]
    [SerializeField] private Vector2 edgePadding = new Vector2(50f, 50f);

    private Camera mainCamera;
    private Camera MainCamera => mainCamera == null ? mainCamera = Camera.main : mainCamera;

    private void LateUpdate() {

        for (int i = 0; i < indicatorArray.Length; i++) {

            OffscreenIndicator indicator = indicatorArray[i];

            if (indicator.IsCharTargetAvailable()) {
                continue;
            }

            UpdateIndicator(indicator);
        }
    }

    #region Register / UnRegister
    public void Register(CanvasCharacter targetChar) {

        if (targetChar == null) { return; }

        if (!this.gameObject.activeSelf) { return; }

        for (int i = 0; i < indicatorArray.Length; i++) {

            if (indicatorArray[i].GetTargetChar() == targetChar) {
                // Not register with same char 2 times
                return;
            }
        }

        for (int i = 0; i < indicatorArray.Length; i++) {

            OffscreenIndicator indicator = indicatorArray[i];

            if (!indicator.IsCharTargetAvailable()) {
                continue;
            }

            indicator.Bind(targetChar);

            return;
        }
    }

    public void UnRegister(CanvasCharacter targetChar) {
        
        if (targetChar == null) { return; }

        for (int i = 0; i < indicatorArray.Length; i++) {

            OffscreenIndicator indicator = indicatorArray[i];

            if (indicator.GetTargetChar() != targetChar) {
                continue;
            }

            indicator.Release();

            return;
        }
    }
    #endregion

    #region Update Indicator
    private bool IsCharOnScreen(Vector3 viewPortPosition) {

        if (viewPortPosition.z <= 0f) { return false; }

        if (viewPortPosition.x < 0f || viewPortPosition.x > 1f) { return false; }

        if (viewPortPosition.y < 0f || viewPortPosition.y > 1f) { return false; }

        return true;

    }

    private void UpdateIndicator(OffscreenIndicator indicator) {

        CanvasCharacter charTartget = indicator.GetTargetChar();
        Vector3 viewportPos = MainCamera.WorldToViewportPoint(charTartget.GetNametagPosition());

        if (IsCharOnScreen(viewportPos)) {
            // If character is on screen (not outside screen)


            indicator.Hide();
        }
        else {
            // If character is not on screen (outside screen)

            indicator.Show();

            Vector2 directionToIndicator = GetScreenDirection(viewportPos);

            Vector2 position = CalculateEdgePosition(directionToIndicator, indicator.GetIndicatorRect());

            indicator.GetIndicatorRect().anchoredPosition = position;
        }
    }

    #endregion

    #region Direction Caculation
    private Vector2 GetScreenDirection(Vector3 viewPortPos) {

        Vector2 viewportCenter = new Vector2(0.5f, 0.5f);
        Vector2 direction = (Vector2)viewPortPos - viewportCenter;

        if (viewPortPos.z < 0f) {
            // If view pos is behind camera

            direction = -direction;
        }

        return direction;
    }
    #endregion

    #region Edge Pos Caculation
    private Vector2 CalculateEdgePosition(Vector2 viewportDirection, RectTransform indicatorRect) {

        if (viewportDirection.sqrMagnitude <= Mathf.Epsilon) {
            // if viewportDir ~ Vector3.zero

            viewportDirection = Vector2.down;
        }

        Rect containerRect = canvasRect.rect;

        // Convert direction from viewport space to canvas space
        Vector2 localDir = new Vector2(viewportDirection.x * containerRect.width, viewportDirection.y * containerRect.height);
        Vector2 indicatorHalfSize = indicatorRect.rect.size * 0.5f;

        float maxX = containerRect.width * 0.5f - edgePadding.x - indicatorHalfSize.x;
        float maxY = containerRect.height * 0.5f - edgePadding.y - indicatorHalfSize.y;

        float scaleX = Mathf.Abs(localDir.x) > Mathf.Epsilon ? maxX / Mathf.Abs(localDir.x) : float.MaxValue;
        float scaleY = Mathf.Abs(localDir.y) > Mathf.Epsilon ? maxY / Mathf.Abs(localDir.y) : float.MaxValue;

        float scale = Mathf.Min(scaleX, scaleY);

        return localDir * scale;
    }
    #endregion
}
