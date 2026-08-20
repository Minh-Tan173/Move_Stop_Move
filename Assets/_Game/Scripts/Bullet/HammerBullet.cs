using UnityEngine;

public class HammerBullet : BulletBase
{
    [SerializeField] private float rotateSpeed;
    private Vector3 startPosition;

    private void OnTriggerEnter(Collider other) {

        CharacterBase collideChar = LevelCache<Collider, CharacterBase>.GetValueWithKey(other);

        if (collideChar == null || collideChar == bulletOwner || collideChar.IsDead() || collideChar.IsImmortal()) { return; }

        int expReward = collideChar.GetCharacterStats().GetExpReward();
        bulletOwner.GetCharacterStats().AddExp(expReward);

        InteractWithCollideChar(collideChar);

        StopMove();
        OnDespawn();
    }

    private void Update() {

        if (!CanMove()) { return; }

        HandleRotate();
        HandleMovement();
    }

    private void HandleRotate() {

        this.UnitTF.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    public override void HandleMovement() {
        
        this.UnitTF.position += moveDir * moveSpeed * Time.deltaTime;

        if ((UnitTF.position - startPosition).sqrMagnitude >= sqrAttackRange) {

            StopMove();
            OnDespawn();
        }
    }

    public override void OnInit(CharacterBase bulletOwner) {

        base.OnInit(bulletOwner);

        startPosition = this.UnitTF.position;

        StartMove();
    }

    public override void OnDespawn() {

        base.OnDespawn();
    }

    public override void ActiveThrow(CharacterBase bulletOwner) {

        OnInit(bulletOwner);
    }
}
