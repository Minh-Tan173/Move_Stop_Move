using UnityEngine;

public enum BoomerangState {
    Outbound,
    Returning
}

public class Boomerang : BulletBase
{

    [Header("Rotate")]
    [SerializeField] private float rotateSpeed;

    [Header("Return")]
    [SerializeField] private int expRewardPerfectCatch = 1;
    [SerializeField] private float returnSpeedMultiplier = 1.5f;
    [SerializeField] private float returnAcceleration = 10f;
    [SerializeField] private float returnTurnSpeed = 360f;
    [SerializeField] private float catchDistance = 0.2f;

    private Vector3 startPosition;
    private Vector3 lastOwnerPosition;

    private BoomerangState currentState;
    private float currentMoveSpeed;


    public void OnTriggerEnter(Collider characterColl) {

        CharacterBase character = LevelCache<Collider, CharacterBase>.GetValueWithKey(characterColl);
        if (character == null) { return; }

        if (character == bulletOwner) {
            // If trigger bullet owner

            if (currentState != BoomerangState.Returning) { return; } // If bullet is not currently return

            // If bullet is returning
            HandleCatchingByOwner();
            return;
        }
        else {
            // If trigger enemy

            if (character.IsDead() || character.IsImmortal()) { return; }

            int expReward = character.GetCharacterStats().GetExpReward();
            bulletOwner.GetCharacterStats().AddExp(expReward);

            InteractWithCollideChar(character);
        }

    }

    private void Update() {

        if (!CanMove()) { return; }

        HandleRotate();
        HandleMovement();
    }

    private void HandleRotate() {

        this.UnitTF.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    private void HandleOutboundMovement() {
        // Go straight movement

        this.UnitTF.position += moveDir * moveSpeed * Time.deltaTime;

        if ((this.UnitTF.position - startPosition).sqrMagnitude >= sqrAttackRange) {
            // Out of Attack Range

            StartReturn();
        }

    }

    private void HandleReturningMovement() {

        if (!bulletOwner.IsDead()) {
            lastOwnerPosition = bulletOwner.UnitTF.position;
        }

        Vector3 targetPosition = bulletOwner.IsDead() ? lastOwnerPosition : bulletOwner.UnitTF.position;

        Vector3 bulletOwnerDir = (targetPosition - this.UnitTF.position);
        bulletOwnerDir.y = 0f;

        if (bulletOwnerDir.sqrMagnitude <= catchDistance * catchDistance) {
            // Reached bullet owner

            if (bulletOwner.IsDead()) {
                StopMove();
                OnDespawn();
            }
            else {
                return;
            }
        }

        bulletOwnerDir.Normalize();

        // Smoothly curve the flight trajectory toward the owner
        moveDir = Vector3.RotateTowards(moveDir, bulletOwnerDir, returnTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);

        // Accelerate during return
        float maxReturnSpeed = moveSpeed * returnSpeedMultiplier;
        currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, maxReturnSpeed, returnAcceleration * Time.deltaTime);


        // Apply into movement progress
        this.UnitTF.position += moveDir * currentMoveSpeed * Time.deltaTime;
    }

    private void StartReturn() {
        currentState = BoomerangState.Returning;

    }

    private void HandleCatchingByOwner() {

        if (!bulletOwner.IsMoving()) {
            // If bullet owner is not moving --> Perfect catch

            bulletOwner.GetCharacterStats().AddExp(expRewardPerfectCatch);
        }

        StopMove();
        OnDespawn();
    }

    public override void HandleMovement() {

        switch (currentState) {
            case BoomerangState.Outbound:

                HandleOutboundMovement();

                break;

            case BoomerangState.Returning:

                HandleReturningMovement();

                break;

        }
    }

    public override void OnInit(CharacterBase bulletOwner) {

        base.OnInit(bulletOwner);

        startPosition = UnitTF.position;

        currentState = BoomerangState.Outbound;
        currentMoveSpeed = moveSpeed;

        StartMove();
    }

    public override void OnDespawn() {
        base.OnDespawn();
    }

    public override void ActiveThrow(CharacterBase bulletOwner) {

        OnInit(bulletOwner);
    }
}
