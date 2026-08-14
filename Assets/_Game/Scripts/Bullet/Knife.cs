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

        if (character.IsDead()) { return; }

        CharacterManager.Instance.DeadCharacter(character); 
        OnDespawn();
    }   

    private void Update() {

        if (!canMove) return;

        UnitTF.position += moveDir * moveSpeed * Time.deltaTime;

        if ((UnitTF.position - startPosition).sqrMagnitude >= sqrAttackRange) {

            OnDespawn();
        }
    }

    public override void OnInit(CharacterBase bulletOwner) {

        startPosition = this.UnitTF.position;

        moveDir = bulletOwner.UnitTF.forward;
        moveDir.y = 0f;
        moveDir.Normalize();

        sqrAttackRange = bulletOwner.GetTrueAttackRange() * bulletOwner.GetTrueAttackRange();

        StartMove();

        // TODO: Update Knife skin
    }

    public override void OnDespawn() {

        StopMove();

        SimplePool.Despawn(this);
    }

    public override void ActiveMovement(CharacterBase bulletOwner) {

        OnInit(bulletOwner);

    }
}
