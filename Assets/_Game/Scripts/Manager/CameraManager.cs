using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{

    [SerializeField] private Vector3 baseOffset;
    [SerializeField] private float zoomSpeed = 5f;

    private Vector3 offset;
    private Vector3 targetOffset;

    private Transform target;


    //private void Awake() {

    //    offset = baseOffset;
    //}


    private void LateUpdate() {


        if (target == null) { return; }

        offset = Vector3.Lerp(offset, targetOffset, zoomSpeed * Time.deltaTime);

        transform.position = target.position + offset;

    }

    public void UpdateZoom(float currentValue, float oldValue) {

        if (target == null || oldValue <= 0f) { return; }

        targetOffset *= currentValue / oldValue;
    }

    public void SetTracking(Transform targetTracking) {

        target = targetTracking;
        offset = baseOffset;
        targetOffset = baseOffset;
    }
}
