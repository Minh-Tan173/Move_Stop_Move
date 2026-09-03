using UnityEngine;

public class Player : CharacterBase
{
    [Header("Player's Info")]
    [SerializeField] private float rotateSpeed;

    [Header("Check Obstacle Behavior")]
    [SerializeField] private ObstacleFade obstacleFade;
    [SerializeField] private Transform obstacleCheckPoint;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float checkDistance;

    [Header("Highlight target")]
    [SerializeField] private Transform highlightTarget;

    private bool isMoving;
    private Vector3 moveDir;
    private bool lastMovingState;

    private int hatID;
    private int pantID;
    private int accessoryID;

    private CharacterBase lastAttackTarget;

    private void Update() {

        if (IsDead() || !LevelManager.Instance.IsGamePlaying()) { return; }

        HandleMovement();

        CheckMovementState();

        if (!IsMoving()) {  

            UpdateAttack();
        }

        UpdateHighLightTarget();
    }

    private void HandleMovement() {

        // Run
        Vector2 inputVector = InputManager.Instance.GetInputVectorNormalized();
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        isMoving = moveDir != Vector3.zero;
        
        if (isMoving) {
            // Cancel attack when moving

            characterCombat.InterruptAttack();
            SetAttackTarget(null);

            lastAttackTarget = null;
        }

        bool canMove = !IsBlockedByObstacle(moveDir);

        if (!canMove) {

            Vector3 moveDirX = new Vector3(inputVector.x, 0f, 0f).normalized;
            canMove = moveDirX != Vector3.zero && !IsBlockedByObstacle(moveDirX);

            if (canMove) {

                moveDir = moveDirX;
            }
            else {

                Vector3 moveDirZ = new Vector3(0f, 0f, inputVector.y).normalized;
                canMove = moveDirZ != Vector3.zero && !IsBlockedByObstacle(moveDirZ);

                if (canMove) {
                    moveDir = moveDirZ;
                }
                else {
                    moveDir = Vector3.zero;
                }
            }
        }

        UnitTF.position += moveDir * GetCharacterStats().GetMoveSpeed() * Time.deltaTime;

        // Rotate
        if (IsMoving()) {
            Quaternion targetLookAt = Quaternion.LookRotation(moveDir);
            UnitTF.rotation = Quaternion.RotateTowards(UnitTF.rotation, targetLookAt, rotateSpeed * Time.deltaTime);
        }
    }

    private void UpdateAttack() {

        if (!characterCombat.IsAttackTargetValid()) {

            SetAttackTarget(null);
            characterCombat.InterruptAttack();

            return;
        }

        if (characterCombat.GetAttackTarget() != lastAttackTarget) {
            // If having New target -> attack immediately

            lastAttackTarget = characterCombat.GetAttackTarget();

            characterCombat.StartAttack();

            return;
        }
        else {
            // If current target not changed

            if (characterCombat.IsAttacking()) {
                // If attack behavior is happening
            }
            else {
                // After attack behavior done --> Wait CD

                characterCombat.UpdateAttackCD(Time.deltaTime);

                if (characterCombat.IsOverAttackCD()) {

                    characterCombat.StartAttack();
                }
            }
        }
    }

    private void UpdateHighLightTarget() {
        
        if (characterCombat.HasAttackTarget()) {
            
            if (!highlightTarget.gameObject.activeSelf) {

                ShowHighlightTarget();
            }

            Vector3 targetPos = characterCombat.GetAttackTarget().UnitTF.position;
            highlightTarget.position = new Vector3(targetPos.x, highlightTarget.position.y, targetPos.z);
        }
        else {
            if (highlightTarget.gameObject.activeSelf) {

                HideHighlightTarget();
            }
        }
    }

    private bool IsBlockedByObstacle(Vector3 checkDir) {

        bool canMoveForward = !(Physics.Raycast(obstacleCheckPoint.position, checkDir, out RaycastHit hitInfo, checkDistance, obstacleLayer));

        if (canMoveForward) {
            return false;
        }

        return true;
    }

    private void CheckMovementState() {

        if (lastMovingState != isMoving) {

            if (isMoving) {

                Run();
            }
            else {
                Idle();
            }
        }

        if (lastMovingState && !isMoving) {

            characterCombat.ResetAttackCD();
        }

        lastMovingState = isMoving;
    }

    private void ShowHighlightTarget() {
        highlightTarget.gameObject.SetActive(true);
    }

    private void HideHighlightTarget() {
        highlightTarget.gameObject.SetActive(false);
    }

    public override void OnInit() {

        base.OnInit();

        HideHighlightTarget();

        PlayerData playerData = DataManager.GetGameData().GetPlayerData();

        // Weapon Prepared
        WeaponType weaponType = playerData.EquippedWeaponType;
        int skinID = playerData.GetEquippedWeaponSkinID(weaponType);

        characterCombat.SetWeaponType(weaponType, skinID);

        // Item Prepared
        hatID = playerData.EquippedHatID;
        pantID = playerData.EquippedPantID;
        accessoryID = playerData.EquippedAccessoryID;

        HatItemData hat = charVisual.ChangeHats(this, hatID);
        if (hat != null) { hat.ApplyBoosterFor(this); }

        PantItemData pant = charVisual.ChangePants(this, pantID);
        if (pant != null) { pant.ApplyBoosterFor(this); }

        charVisual.ChangeAccessories(this, accessoryID);
    }

    public override void OnDespawn() {

        base.OnDespawn();
        obstacleFade.OnDespawn();

        charVisual.OnDespawn();
    }

    public override bool IsMoving() {
        return isMoving;
    }
}
