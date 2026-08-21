using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IDragHandler
{
    [Header("References")]
    [SerializeField] private CanvasGroup joystickCanvas;

    [Header("Joystick Rect")]
    [SerializeField] private RectTransform backgroundRect;
    [SerializeField] private RectTransform handleRect;
    [SerializeField] private RectTransform touchZoneRect;

    [Header("On Screen Stick")]
    [SerializeField] private GameObject onScreenStickObj;

    private void Awake() {

        OnDespawn();
    }

    private void OnDespawn() {

        SetActiveJoystick(false);
    }

    private void SetActiveJoystick(bool isActive) {

        if (isActive) {

            joystickCanvas.alpha = 1f;
        }
        else {

            joystickCanvas.alpha = 0f;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {

        if (!LevelManager.Instance.IsGamePlaying()) { return; }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(touchZoneRect, eventData.position, eventData.pressEventCamera, out Vector2 localPressPoint);

        backgroundRect.anchoredPosition = localPressPoint;
        handleRect.anchoredPosition = Vector2.zero;

        SetActiveJoystick(true);

        ExecuteEvents.Execute(onScreenStickObj, eventData, ExecuteEvents.pointerDownHandler);
    }

    public void OnDrag(PointerEventData eventData) {

        if (!LevelManager.Instance.IsGamePlaying()) { return; }

        ExecuteEvents.Execute(onScreenStickObj, eventData, ExecuteEvents.dragHandler);
    }

    public void OnPointerUp(PointerEventData eventData) {

        if (!LevelManager.Instance.IsGamePlaying()) { return; }

        OnDespawn();

        ExecuteEvents.Execute(onScreenStickObj, eventData, ExecuteEvents.pointerUpHandler);
    }
}
