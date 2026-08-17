using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    [Header("Skin Data")]
    [SerializeField] private PantSO pantSO;
    [SerializeField] private HatSO hatSO;
    [SerializeField] private AccessorySO accessorySO;

    [Header("Ref")]
    [SerializeField] private SkinnedMeshRenderer pantsRenderer;
    [SerializeField] private Transform topHeadPlaceholder;
    [SerializeField] private Transform leftHandPlaceholder;

    private MaterialPropertyBlock propertyBlock;
    private PoolUnit currentHat;
    private PoolUnit currentAccessory;

    private void Awake() {
        propertyBlock = new MaterialPropertyBlock();
    }

    public void OnDespawn() {

        if (currentHat != null) {
            SimplePool.Despawn(currentHat);
            currentHat = null;
        }

        if (currentAccessory != null) {
            SimplePool.Despawn(currentAccessory);
            currentAccessory = null;
        }

    }

    public void ChangePants(int pantID = -1) {

        Texture2D pantsTexture;

        // Get Pant Texture
        if (pantID >= 0) {
            // If having pantID

            pantsTexture = pantSO.GetPantTexture(pantID);
        }
        else {
            int totalPant = pantSO.pantItemDataList.Count;
            int randomPantID = Random.Range(0, totalPant);

            pantsTexture = pantSO.GetPantTexture(randomPantID);
        }

        // Setup Pant Visual
        pantsRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture(CharacterConst.BASE_MAP, pantsTexture);
        pantsRenderer.SetPropertyBlock(propertyBlock);
    }

    public HatItemData ChangeHats(int hatID = -1) {

        if (hatID < 0) {

            if (Random.Range(0, 2) == 0) { return null; }

            int totalHat = hatSO.hatItemDataList.Count;
            hatID = Random.Range(0, totalHat);
        }

        PoolUnit hatPrefab = hatSO.GetHatPrefab(hatID);

        // Setup Hat
        currentHat = SimplePool.Spawn<PoolUnit>(hatPrefab, topHeadPlaceholder.position, Quaternion.identity);

        currentHat.UnitTF.SetParent(topHeadPlaceholder);
        currentHat.UnitTF.localPosition = Vector3.zero;
        currentHat.UnitTF.localRotation = Quaternion.identity;

        return hatSO.GetHatData(hatID);
    }

    public void ChangeAccessories(int acessoryID = -1) {

        //if (Random.value <= 0.3f) { return; }

        PoolUnit accessoryPrefab;

        // Get Accessory Prefab
        if (acessoryID >= 0) {
            // If having ID

            accessoryPrefab = accessorySO.GetAccesoryPrefab(acessoryID);
        }
        else {

            int totalAccessory = accessorySO.accessoryItemDataList.Count;
            int randomAccessoryID = Random.Range(0, totalAccessory);
            accessoryPrefab = accessorySO.GetAccesoryPrefab(randomAccessoryID);
        }

        // Setup accessory
        currentAccessory = SimplePool.Spawn<PoolUnit>(accessoryPrefab, leftHandPlaceholder.position, Quaternion.identity);
        currentAccessory.UnitTF.SetParent(leftHandPlaceholder);
        currentAccessory.UnitTF.localPosition = Vector3.zero;
        currentAccessory.UnitTF.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

    }
}
