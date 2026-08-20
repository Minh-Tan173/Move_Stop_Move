using UnityEngine;

public class BulletBase : PoolUnit
{
    [Header("Data")]
    [SerializeField] protected float moveSpeed;

    protected bool canMove;
    protected Vector3 moveDir;
    protected float sqrAttackRange;
    protected CharacterBase bulletOwner;


    public void StartMove() {
        canMove = true;
    }

    public void StopMove() {
        canMove = false;
    }

    public bool CanMove() { 
        return canMove;
    }

    public void InteractWithCollideChar(CharacterBase character) {

        CharacterManager.Instance.DeadCharacter(character);
    }

    public virtual void HandleMovement() {
        Debug.LogError("Trigger baseBullet");
    }

    public virtual void OnInit(CharacterBase bulletOwner) {

        this.bulletOwner = bulletOwner;

        Vector3 forward = bulletOwner.UnitTF.forward;
        moveDir = new Vector3(forward.x, 0f, forward.z).normalized;

        sqrAttackRange = bulletOwner.GetTrueAttackRange() * bulletOwner.GetTrueAttackRange();
    }

    public virtual void OnDespawn() {
        SimplePool.Despawn(this);
    }

    public virtual void ActiveThrow(CharacterBase bulletOwner) {
        Debug.LogError("Trigger Bullet Base");
    }
}
