using System.Collections;
using UnityEngine;

public class Knife : BulletBase
{
    [SerializeField] private BoxCollider boxCollider;

    private Vector3 startPosition;
    private Vector3 moveDir;
    private float sqrAttackRange;

    private void OnTriggerEnter(Collider other) {

        CharacterBase character = LevelCache<Collider, CharacterBase>.GetValueWithKey(other);
        if (character == null) { return; }

        
        OnDespawn();
    }

    private void Update() {

        if (!canMove) return;

        UnitTF.position += moveDir * moveSpeed * Time.deltaTime;

        if ((UnitTF.position - startPosition).sqrMagnitude >= sqrAttackRange) {

            OnDespawn();
        }
    }

    public override void OnInit(Transform target, float attackRange) {

        startPosition = this.UnitTF.position;

        moveDir = target.position - startPosition;
        moveDir.y = 0f;
        moveDir.Normalize();

        sqrAttackRange = attackRange * attackRange;

        StartMove();

        // TODO: Update Knife skin
    }

    public override void OnDespawn() {

        StopMove();

        SimplePool.Despawn(this);
    }

    public override void ActiveMovement(Transform target, float attackRange) {

        OnInit(target, attackRange);

    }
}
