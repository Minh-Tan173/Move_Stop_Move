using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{

    [SerializeField] private Vector3 offset;
    [SerializeField] private float baseSize = 0.15f;


    private Transform target;
    private Vector3 velocity;


    private void LateUpdate() {


        Vector3 targetPos = target.position + offset;


        transform.position = target.position + offset;

    }

    public void SetTracking(Transform targetTracking) {

        target = targetTracking;
    }
}
