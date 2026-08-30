using UnityEngine;

public enum CameraType {

    GamePlayCamera,
    MainMenuCamera,
    ShopCamera
}

public class CameraManager : Singleton<CameraManager>
{
    [Header("GamePlay Camera")]
    [SerializeField] private Vector3 baseOffset;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private Vector3 gamePlayCamRotation;

    [Header("Main Menu Camera")]
    [SerializeField] private Vector3 mainMenuOffset;
    [SerializeField] private float mainMenuLookHeight = 1f;

    [Header("Shop Camera")]
    [SerializeField] private Vector3 shopCameraOffset;
    [SerializeField] private float shopLookHeight = 1f;

    [SerializeField] private float switchSpeed = 5f;

    private Transform camTransform;
    private Transform CamTransform => camTransform == null ? camTransform = this.transform : camTransform;

    private Vector3 offset;
    private Vector3 targetOffset;

    private Transform target;

    private CameraType currentCameraType;

    private bool isSwitching;


    private void LateUpdate() {


        if (target == null) { return; }

        if (currentCameraType == CameraType.MainMenuCamera) {

            UpdateMainMenuCamera();
        }
        else if (currentCameraType == CameraType.GamePlayCamera) {

            UpdateGameplayCamera();
        }
        else if (currentCameraType == CameraType.ShopCamera) {

            UpdateShopCamera();
        }
    }

    private void UpdateMainMenuCamera() {

        Vector3 cameraPos = target.position + target.TransformDirection(mainMenuOffset);

        Vector3 lookTarget = target.position + Vector3.up * mainMenuLookHeight;

        Quaternion cameraRot = Quaternion.LookRotation(lookTarget - cameraPos);


        MoveCamTo(cameraPos, cameraRot);
    }

    private void UpdateShopCamera() {


        Vector3 cameraPos = target.position + target.TransformDirection(shopCameraOffset);

        Vector3 lookTarget = target.position + Vector3.up * shopLookHeight;

        Quaternion cameraRot = Quaternion.LookRotation(lookTarget - cameraPos);

        MoveCamTo(cameraPos, cameraRot);
    }

    private void UpdateGameplayCamera() {

        offset = Vector3.Lerp(offset, targetOffset, zoomSpeed * Time.deltaTime);

        Vector3 cameraPos = target.position + offset;

        MoveCamTo(cameraPos, Quaternion.Euler(gamePlayCamRotation));
    }

    private void MoveCamTo(Vector3 targetPos, Quaternion targetRot) {

        if (!isSwitching) {

            CamTransform.SetPositionAndRotation(targetPos, targetRot);
            return;
        }

        CamTransform.position = Vector3.Lerp(CamTransform.position, targetPos,switchSpeed * Time.deltaTime);

        CamTransform.rotation = Quaternion.Slerp(CamTransform.rotation, targetRot,switchSpeed * Time.deltaTime);

        if (Vector3.Distance(CamTransform.position, targetPos) < 0.05f) {

            isSwitching = false;
        }
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

    public void SwitchCam(CameraType cameraType) {

        currentCameraType = cameraType;

        isSwitching = true;

        switch (cameraType) {

            case CameraType.MainMenuCamera: break;

            case CameraType.GamePlayCamera:

                break;

            case CameraType.ShopCamera: break;
        }
    }
}
