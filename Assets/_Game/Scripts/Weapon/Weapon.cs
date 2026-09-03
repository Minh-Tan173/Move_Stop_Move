using UnityEngine;

public class Weapon : PoolUnit
{
    [Header("Visual")]
    [SerializeField] private Transform weaponVisual;
    [SerializeField] private MeshRenderer weaponMesh;

    private MaterialPropertyBlock propertyBlock;

    private void Awake() {
        propertyBlock = new MaterialPropertyBlock();
    }

    public void ApplySkin(Texture2D texture) {

        int materialCount = weaponMesh.sharedMaterials.Length;

        for (int i = 0; i < materialCount; i++) {

            weaponMesh.GetPropertyBlock(propertyBlock, i);

            propertyBlock.SetTexture(CharacterConst.BASE_MAP, texture);

            propertyBlock.SetColor("_BaseColor", Color.white);

            weaponMesh.SetPropertyBlock(propertyBlock, i);
        }
    }

    public void RotateVisual(float value) {
        weaponVisual.Rotate(Vector3.up, value, Space.Self);
    }

    public void SetupVisualForPreview() {

        weaponVisual.localPosition = Vector3.zero;
        weaponVisual.localRotation = Quaternion.Euler(180f, 0f, 0f);
    }
}
