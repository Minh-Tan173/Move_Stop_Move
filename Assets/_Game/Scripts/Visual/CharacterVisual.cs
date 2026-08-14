using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    [Header("Skin Data")]
    [SerializeField] private PantSO pantSO;
    [SerializeField] private HatSO hatSO;

    [Header("Ref")]
    [SerializeField] private SkinnedMeshRenderer pantsRenderer;
    [SerializeField] private Transform topHeadPlaceholder;

    private MaterialPropertyBlock propertyBlock;
    private PoolUnit currentHat;

    private void Awake() {
        propertyBlock = new MaterialPropertyBlock();
    }

    public void OnDespawn() {

        if (currentHat != null) {
            SimplePool.Despawn(currentHat);
            currentHat = null;
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

    public void ChangeHats(int hatID = -1) {

        PoolUnit hatPrefab;

        // Get Hat Prefab
        if (hatID >= 0) {
            // If having hatID

            hatPrefab = hatSO.GetHatPrefab(hatID);
        }
        else {

            int totalHat = hatSO.hatItemDataList.Count;
            int randomHatID = Random.Range(0, totalHat);
            hatPrefab = hatSO.GetHatPrefab(randomHatID);
        }

        if (Random.Range(0, 2) == 0) { return; } // 50% chance not spawn

        // Setup Hat
        Quaternion quaternion = Quaternion.Euler(-90f, 0f, 0f);
        currentHat = SimplePool.Spawn<PoolUnit>(hatPrefab, topHeadPlaceholder.position, quaternion);
        currentHat.UnitTF.SetParent(topHeadPlaceholder);
        currentHat.UnitTF.localPosition = Vector3.zero;
    }
}
