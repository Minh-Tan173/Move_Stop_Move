using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CharacterVisual : MonoBehaviour
{

    [Header("Skin Data")]
    [SerializeField] private ColorSO colorSO;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private PantSO pantSO;
    [SerializeField] private HatSO hatSO;
    [SerializeField] private AccessorySO accessorySO;

    [Header("Child Ref")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private SkinnedMeshRenderer modelSkinMesh;
    [SerializeField] private Transform rightHandPlacedHolder;
    [SerializeField] private SkinnedMeshRenderer pantsRenderer;
    [SerializeField] private Transform topHeadPlaceholder;
    [SerializeField] private Transform leftHandPlaceholder;

    [Header("VFX")]
    [SerializeField] private ParticleSystem bloodVFX;

    private MaterialPropertyBlock propertyBlock;
    private Weapon currentWeapon;
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

    public void OnInit() {

        bloodVFX.Stop();
        bloodVFX.Clear();
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

    public void ApplySkinColorFor(CharacterBase parentChar) {

        int colorID = 0;

        if (parentChar is Player) {

            colorID = DataManager.GetGameData().GetPlayerData().EquippedColorID;
        }
        else {
            // Is Bot

            colorID = Random.Range(0, colorSO.GetTotalColor());
        }

        Color skinColor = colorSO.GetColorWithType((ColorType)colorID);

        // Update Skin For Char
        modelSkinMesh.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(CharacterConst.BASE_COLOR, skinColor);
        modelSkinMesh.SetPropertyBlock(propertyBlock);

    }

    public void ChangeWeapon(WeaponType weaponType, int skinID) {

        if (currentWeapon != null) {
            // Holding old weapon before

            SimplePool.Despawn(currentWeapon);
            currentWeapon = null;
        }

        Weapon weaponPrefab = weaponSO.GetWeaponPrefab(weaponType);

        currentWeapon = SimplePool.Spawn<Weapon>(weaponPrefab, rightHandPlacedHolder.position, Quaternion.identity);
        currentWeapon.UnitTF.SetParent(rightHandPlacedHolder);

        ResetItem(currentWeapon);

        WeaponSkinData skinData = weaponSO.GetWeaponSkinData(weaponType, skinID);

        currentWeapon.ApplySkin(skinData.GetTexture());
    }

    public PantItemData ChangePants(CharacterBase parentChar, int pantID = -1) {

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

        Texture2D pantsTexture = pantSO.GetPantTexture(pantID);

        // Setup Pant Visual
        pantsRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetTexture(CharacterConst.BASE_MAP, pantsTexture);
        propertyBlock.SetColor(CharacterConst.BASE_COLOR, Color.white);
        pantsRenderer.SetPropertyBlock(propertyBlock);

        return pantSO.GetPantItemData(pantID);
    }

    public HatItemData ChangeHats(CharacterBase parentChar, int hatID = -1) {

        if (currentHat != null) {

            SimplePool.Despawn(currentHat);
            currentHat = null;
        }

        if (hatID < 0) {
            
            if (parentChar is Player) {

                return null;
            }
            else {
                // If character is Bot

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

        if (currentAccessory != null) {

            SimplePool.Despawn(currentAccessory);
            currentAccessory = null;
        }

        if (accessoryID < 0) {

            if (parentChar is Player) {

                return;
            }
            else {
                // If character is Bot

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

    public void PlayBlood() {

        bloodVFX.Stop();
        bloodVFX.Clear();
        bloodVFX.Play();
    }
}
