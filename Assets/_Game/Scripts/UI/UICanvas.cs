using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [Header("")]
    [SerializeField] private UIElementAnim[] uiElementAnims;

    [SerializeField] private bool isdestroyOnClose = false;
    [SerializeField] protected RectTransform canvasRect;

    private void Awake() {

        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 2.1f) {

            Vector2 leftBottom = canvasRect.offsetMin;
            Vector2 rightTop = canvasRect.offsetMax;

            leftBottom.y = 0f;
            rightTop.y = -100f;

            canvasRect.offsetMin = leftBottom;
            canvasRect.offsetMax = rightTop;
        }
    }


    public void ShowUIElements() {

        //foreach (UIElementAnimation elementUI in elementUIAnimationArray) {

        //    elementUI.ShowElement();
        //}
    }

    public void HideUIElements() {

        //foreach (UIElementAnimation elementUI in elementUIAnimationArray) {

        //    elementUI.HideElement();
        //}
    }

    public virtual void SetUp() {

    }

    public virtual void Open() {
        ShowUIElements();
    }

    public virtual void CloseUI(float time) {

        // Before close UI
        HideUIElements();

        Invoke(nameof(CloseDirectly), time);
    }

    public virtual void CloseDirectly() {

        if (isdestroyOnClose) {

            Destroy(gameObject);
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
