using UnityEngine;

public class WeaponPreview : MonoBehaviour
{
    [SerializeField] private Transform previewRoot;
    [SerializeField] private LayerMask previewLayer;

    [Header("Preview Anim")]
    [SerializeField] private float rotateSpeed = 90f;
    [SerializeField] private float floatHeight = 0.15f;
    [SerializeField] private float floatSpeed = 2f;

    private Weapon currentWeapon;
    private Vector3 weaponStartPosition;

    private void Update() {

        if (currentWeapon == null) return;

        // Rotate
        currentWeapon.RotateVisual(rotateSpeed * Time.deltaTime);

        // Floating
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        currentWeapon.transform.localPosition = weaponStartPosition + Vector3.up * offset;
    }

    private void Clear() {

        if (currentWeapon == null) return;

        Destroy(currentWeapon.gameObject);
        currentWeapon = null;
    }

    private void SetLayerRecursively(GameObject obj, int previewLayer) {
        
        obj.layer = previewLayer;

        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, previewLayer);
        }
    }

    private int GetLayerIndex(LayerMask mask) {
        return (int)Mathf.Log(mask.value, 2);
    }

    public void ShowWeapon(WeaponItemData weaponData) {

        Clear();

        currentWeapon = Instantiate(weaponData.GetPrefab(), previewRoot);
        currentWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        currentWeapon.SetupVisualForPreview();

        weaponStartPosition = currentWeapon.transform.localPosition;

        SetLayerRecursively(currentWeapon.gameObject, GetLayerIndex(previewLayer));
    }

    public void ApplySkin(Texture2D skinTexture) {

        if (currentWeapon == null) return;

        currentWeapon.ApplySkin(skinTexture);
    }
}
