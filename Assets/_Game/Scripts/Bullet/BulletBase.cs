using UnityEngine;

public class BulletBase : PoolUnit
{
    [Header("Data")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected Renderer bulletRenderer;

    protected bool canMove;
    protected Vector3 moveDir;
    protected float sqrAttackRange;
    protected CharacterBase bulletOwner;

    private MaterialPropertyBlock propertyBlock;

    private void Awake() {
        propertyBlock = new MaterialPropertyBlock();
    }


    public void StartMove() {
        canMove = true;
    }

    public void StopMove() {
        canMove = false;
    }

    public bool CanMove() { 
        return canMove;
    }

    public void ApplySkin(Texture2D texture) {

        int materialCount = bulletRenderer.sharedMaterials.Length;

        for (int i = 0; i < materialCount; i++) {

            bulletRenderer.GetPropertyBlock(propertyBlock, i);

            propertyBlock.SetTexture(CharacterConst.BASE_MAP, texture);

            propertyBlock.SetColor("_BaseColor", Color.white);

            bulletRenderer.SetPropertyBlock(propertyBlock, i);
        }
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

        sqrAttackRange = bulletOwner.GetCharacterCombat().GetTrueAttackRange() * bulletOwner.GetCharacterCombat().GetTrueAttackRange();
    }

    public virtual void OnDespawn() {
        SimplePool.Despawn(this);
    }

    public virtual void ActiveThrow(CharacterBase bulletOwner) {
        Debug.LogError("Trigger Bullet Base");
    }
}
