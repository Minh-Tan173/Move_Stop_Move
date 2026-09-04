using System.Collections;
using UnityEngine;

public class KnifeBullet : BulletBase
{
    private Vector3 startPosition;

    private void OnTriggerEnter(Collider other) {

        CharacterBase character = LevelCache<Collider, CharacterBase>.GetValueWithKey(other);

        if (character == null || character == bulletOwner || character.IsDead() || character.IsImmortal()) { return; }

        int expReward = character.GetCharacterStats().GetExpReward();
        bulletOwner.GetCharacterStats().AddExp(expReward);


        InteractWithCollideChar(character);

        StopMove();
        OnDespawn();
    }   

    private void Update() {

        if (!CanMove()) { return; }

        HandleMovement();
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

        UnitTF.rotation = Quaternion.LookRotation(moveDir);

        StartMove();

        // TODO: Update Knife skin
    }

    public override void OnDespawn() {
        base.OnDespawn();
    }

    public override void ActiveThrow(CharacterBase bulletOwner) {

        OnInit(bulletOwner);


        if (bulletOwner == CharacterManager.Instance.GetPlayer()) {

            int totalAudioOfKnife = SoundManager.Instance.GetAudioClipRefsSO().GetAudioClipListWithType(SFXType.KnifeThrow).Count;
            int audioIndex = Random.Range(0, totalAudioOfKnife);

            SoundManager.Instance.PlaySound(this.UnitTF.position, SFXType.KnifeThrow, audioIndex);
        }
    }
}
