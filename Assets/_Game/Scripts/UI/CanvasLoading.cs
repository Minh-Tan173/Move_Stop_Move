using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLoading : UICanvas
{
    [Header("Load Info")]
    [SerializeField] private float loadingTimeMax = 1f;
    [SerializeField] private float rotateDuration = 0.1f;
    [SerializeField] private RectTransform loadingImageRect;

    private IEnumerator IELoading(Action callback, bool isDelayClose = true) {

        float loadingTime = 0f;
        float elapsedRotate = 0f;
        
        while (loadingTime <= loadingTimeMax) {

            elapsedRotate += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedRotate / rotateDuration);

            float zRot = Mathf.Lerp(0f, -360f, t);
            loadingImageRect.localRotation = Quaternion.Euler(0f, 0f, zRot);

            if (t >= 1f) {
                elapsedRotate -= rotateDuration;
            }

            loadingTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
        callback?.Invoke();

        if (isDelayClose) {

            Debug.Log("Delay close");
            yield return new WaitForSeconds(0.25f);
        }

        UIManager.Instance.CloseUI<CanvasLoading>(0f);
    }

    public void ActiveLoading(Action callback, bool isDelayClose = true) {
        StartCoroutine(IELoading(callback, isDelayClose));
    }

}
