using UnityEngine;

public class BulletBase : PoolUnit
{
    [Header("Data")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float despawnDuration = 3f;
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected Renderer bulletRenderer;

    protected bool canMove;
    protected bool isDamageActive;
    protected Vector3 moveDir;
    protected float sqrAttackRange;
    protected CharacterBase bulletOwner;

    private MaterialPropertyBlock propertyBlock;

    private void Awake() {
        propertyBlock = new MaterialPropertyBlock();
    }


    protected void StartMove() {
        canMove = true;
    }

    protected void StopMove() {
        canMove = false;
    }

    protected bool CanMove() { 
        return canMove;
    }

    protected void EnableDamage() {
        isDamageActive = true;
    }

    protected void DisableDamage() {
        isDamageActive = false;
    }

    protected bool IsDamageActive() {
        return isDamageActive;
    }

    public void ApplySkin(Texture2D texture) {

        int materialCount = bulletRenderer.sharedMaterials.Length;

        for (int i = 0; i < materialCount; i++) {

            bulletRenderer.GetPropertyBlock(propertyBlock, i);

            propertyBlock.SetTexture(CharacterConst.BASE_MAP, texture);

            propertyBlock.SetColor(CharacterConst.BASE_COLOR, Color.white);

            bulletRenderer.SetPropertyBlock(propertyBlock, i);
        }
    }

    public void HandleCharacterHit(CharacterBase collideChar) {

        Player player = CharacterManager.Instance.GetPlayer();

        if (collideChar == player) {
            // If collide Player

            CharacterManager.Instance.SetKilledPlayerIs(bulletOwner as Bot);
        }

        if (bulletOwner == player) {
            // If Owner is Player --> Player just kill someone

            int levelOfCollideChar = collideChar.GetCharacterStats().GetCurrentLevel();

            player.GetPlayerScore().AddKill(levelOfCollideChar);
        }

        CharacterManager.Instance.DeadCharacter(collideChar);
    }

    public virtual void HandleMovement() {

        Debug.LogError("Trigger baseBullet");
    }

    public virtual void OnInit(CharacterBase bulletOwner) {

        CancelInvoke(nameof(OnDespawn));

        EnableDamage();

        this.bulletOwner = bulletOwner;

        Vector3 forward = bulletOwner.UnitTF.forward;
        moveDir = new Vector3(forward.x, 0f, forward.z).normalized;

        sqrAttackRange = bulletOwner.GetCharacterCombat().GetTrueAttackRange() * bulletOwner.GetCharacterCombat().GetTrueAttackRange();
    }

    public virtual void OnDespawn() {
        SimplePool.Despawn(this);
    }

    public virtual void ActiveThrow(CharacterBase bulletOwner) {
        Debug.LogError("Trigger Bullet Base");
    }
}
