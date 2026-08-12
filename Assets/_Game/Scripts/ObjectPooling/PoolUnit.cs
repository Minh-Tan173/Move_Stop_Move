using UnityEngine;

public class PoolUnit : MonoBehaviour
{
    public PoolType poolType;
    private Transform unitTF;

    public Transform UnitTF => unitTF == null ? unitTF = this.transform : unitTF;
}
