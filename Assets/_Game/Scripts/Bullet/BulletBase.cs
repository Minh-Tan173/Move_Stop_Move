using UnityEngine;

public class BulletBase : PoolUnit
{
    [Header("Data")]
    [SerializeField] protected float moveSpeed;

    protected bool canMove;

    public void StartMove() {
        canMove = true;
    }

    public void StopMove() {
        canMove = false;
    }

    public void InteractWithCharacter(CharacterBase character) {

        CharacterManager.Instance.DeadCharacter(character);
    }

    public virtual void OnInit(CharacterBase bulletOwner) {

    }

    public virtual void OnDespawn() {

    }

    public virtual void ActiveMovement(CharacterBase bulletOwner) {
        Debug.LogError("Trigger Bullet Base");
    }
}
