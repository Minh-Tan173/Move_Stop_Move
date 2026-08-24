using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{

    [SerializeField] private Vector3 baseOffset;

    private Vector3 offset;

    private Transform target;

    private void Awake() {

        offset = baseOffset;
    }


    private void LateUpdate() {


        Vector3 targetPos = target.position + offset;


        transform.position = target.position + offset;

    }
    public void UpdateZoom(float currentValue, float defaultValue) {

        float newScale = currentValue / defaultValue;
        offset = baseOffset * newScale;
    }

    public void SetTracking(Transform targetTracking) {

        target = targetTracking;
    }
}
