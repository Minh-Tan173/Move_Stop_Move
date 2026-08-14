using UnityEngine;

public class PoolUnit : MonoBehaviour
{
    [Header("Pooling")]
    public PoolUnit prefabKey;
    
    private Transform unitTF;

    public Transform UnitTF => unitTF == null ? unitTF = this.transform : unitTF;
}
