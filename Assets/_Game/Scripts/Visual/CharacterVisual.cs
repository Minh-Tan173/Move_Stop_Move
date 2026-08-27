using UnityEngine;
using UnityEngine.UI;

public class CharacterVisual : MonoBehaviour
{

    [Header("Skin Data")]
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private PantSO pantSO;
    [SerializeField] private HatSO hatSO;
    [SerializeField] private AccessorySO accessorySO;

    [Header("Child Ref")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform rightHandPlacedHolder;
    [SerializeField] private SkinnedMeshRenderer pantsRenderer;
    [SerializeField] private Transform topHeadPlaceholder;
    [SerializeField] private Transform leftHandPlaceholder;

    private MaterialPropertyBlock propertyBlock;
    private PoolUnit currentWeapon;
    private PoolUnit currentHat;
    private PoolUnit currentAccessory;

    private void Awake() {
        propertyBlock = new MaterialPropertyBlock();
    }

    private void ResetItem(PoolUnit item) {

        item.UnitTF.localPosition = Vector3.zero;
        item.UnitTF.localRotation = Quaternion.identity;
        item.UnitTF.localScale = Vector3.one;
    }

    public void OnDespawn() {
        
        if (currentWeapon != null) {
            SimplePool.Despawn(currentWeapon);
            currentWeapon = null;
        }
        
        if (currentHat != null) {
            SimplePool.Despawn(currentHat);
            currentHat = null;
        }

        if (currentAccessory != null) {
            SimplePool.Despawn(currentAccessory);
            currentAccessory = null;
        }

    }

    public void UpdateSize(float newSize) {
        visualTransform.localScale = Vector3.one * newSize;
    }

    public void UpdateVisual() {

        
    }

    public void ChangeWeapon(WeaponType weaponType) {

        PoolUnit weaponPrefab = weaponSO.GetWeaponPrefab(weaponType);
        
        currentWeapon = SimplePool.Spawn<PoolUnit>(weaponPrefab, rightHandPlacedHolder.position, Quaternion.identity);
        ResetItem(currentWeapon);
    }

    public PantItemData ChangePants(CharacterBase parentChar ,int pantID = -1) {

        Texture2D pantsTexture;

        // Get Pant Texture
        if (pantID < 0) {

            if (parentChar is Player) {

                pantsRenderer.SetPropertyBlock(null);
                return null;

            }
            else {
                // If is Bot
                int totalPant = pantSO.pantItemDataList.Count;
                pantID = Random.Range(0, totalPant);
            }
        }

        pantsTexture = pantSO.GetPantTexture(pantID);

        // Setup Pant Visual
        pantsRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture(CharacterConst.BASE_MAP, pantsTexture);
        pantsRenderer.SetPropertyBlock(propertyBlock);

        return pantSO.GetPantItemData(pantID);
    }

    public HatItemData ChangeHats(CharacterBase parentChar, int hatID = -1) {

        if (hatID < 0) {
            
            if (parentChar is Player) {

                if (currentHat != null) {

                    SimplePool.Despawn(currentHat);
                    currentHat = null;
                }

                return null;
            }
            else {

                if (Random.Range(0, 2) == 0) { return null; }

                int totalHat = hatSO.hatItemDataList.Count;
                hatID = Random.Range(0, totalHat);
            }
        }

        PoolUnit hatPrefab = hatSO.GetHatPrefab(hatID);

        // Setup Hat
        currentHat = SimplePool.Spawn<PoolUnit>(hatPrefab, topHeadPlaceholder.position, Quaternion.identity);

        currentHat.UnitTF.SetParent(topHeadPlaceholder);
        ResetItem(currentHat);
        

        return hatSO.GetHatData(hatID);
    }

    public void ChangeAccessories(CharacterBase parentChar, int accessoryID = -1) {

        if (accessoryID < 0) {

            if (parentChar is Player) {

                if (currentAccessory != null) {

                    SimplePool.Despawn(currentAccessory);
                    currentAccessory = null;
                }

                return;
            }
            else {

                int totalAccessory = accessorySO.accessoryItemDataList.Count;
                accessoryID = Random.Range(0, totalAccessory);
            }
        }

        PoolUnit accessoryPrefab = accessorySO.GetAccesoryPrefab(accessoryID);

        // Setup Accessory
        currentAccessory = SimplePool.Spawn<PoolUnit>(accessoryPrefab, leftHandPlaceholder.position, Quaternion.identity);

        currentAccessory.UnitTF.SetParent(leftHandPlaceholder);
        ResetItem(currentAccessory);

    }
}
