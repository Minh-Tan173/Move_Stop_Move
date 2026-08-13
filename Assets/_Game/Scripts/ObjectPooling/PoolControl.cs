using UnityEngine;

public enum PoolType {
    None = 0,
    Character = 1,
    Knife = 2,
}

public class PoolControl : MonoBehaviour
{
    [SerializeField] private PoolAmount[] poolAmountArray;

    private void Awake() {

        PoolUnit[] gameUnits = Resources.LoadAll<PoolUnit>("Pool/");

        for (int i = 0; i < gameUnits.Length; i++) {

            SimplePool.Preload(gameUnits[i], 0, new GameObject(gameUnits[i].name).transform);
        }

        for (int i = 0; i < poolAmountArray.Length; i++) {

            SimplePool.Preload(poolAmountArray[i].prefab, poolAmountArray[i].amount, poolAmountArray[i].parent);
        }
    }
}

[System.Serializable]
public class PoolAmount {

    public PoolUnit prefab;
    public Transform parent;
    public int amount;
}