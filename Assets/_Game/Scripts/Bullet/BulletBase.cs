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

        if (character is Bot) {

            CharacterManager.Instance.DespawnBot(character as Bot);
            return;
        }
    }

    public virtual void OnInit(Transform target, float attackRange) {

    }

    public virtual void OnDespawn() {

    }

    public virtual void ActiveMovement(Transform target, float attackRange) {
        Debug.LogError("Trigger Bullet Base");
    }
}
