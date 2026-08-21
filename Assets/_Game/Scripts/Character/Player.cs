using UnityEngine;

public class Player : CharacterBase
{
    [Header("Player's Info")]
    [SerializeField] private float rotateSpeed;

    [Header("Check Obstacle Behavior")]
    [SerializeField] private Transform obstacleCheckPoint;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float checkDistance;

    private bool isMoving;
    private Vector3 moveDir;
    private bool lastMovingState;

    #region Test
    private int hatID = 0;
    private int pantID = 0;
    private int accessoryID = 0;
    #endregion

    private CharacterBase lastAttackTarget;

    private void Update() {

        HandleMovement();

        CheckMovementState();

        if (!IsMoving()) {

            UpdateAttack();
        }
    }

    private void HandleMovement() {

        // Run
        Vector2 inputVector = InputManager.Instance.GetInputVectorNormalized();
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        isMoving = moveDir != Vector3.zero;
        
        if (isMoving) {
            // Cancel attack when moving

            CancelAttack();
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

        if (!IsAttackTargetValid()) {

            SetAttackTarget(null);
            CancelAttack();

            return;
        }

        // New target -> attack immediately
        if (attackTarget != lastAttackTarget) {

            lastAttackTarget = attackTarget;

            StartAttack();

            return;
        }


        if (IsInAttackDuration()) {

            UpdateAttackDuration(Time.deltaTime);

            if (IsOverAttackDuration()) {

                FinishAttack();
            }

            return;
        }


        UpdateAttackCD(Time.deltaTime);

        if (IsOverAttackCD()) {

            StartAttack();
        }
    }

    private void StartAttack() {
        LookAttackTarget();

        Attack();
    }

    private bool IsBlockedByObstacle(Vector3 checkDir) {

        bool canMoveForward = !(Physics.Raycast(obstacleCheckPoint.position, checkDir, out RaycastHit hitInfo, checkDistance, obstacleLayer));

        if (canMoveForward) {
            return false;
        }

        return true;
    }

    private void CheckMovementState() {

        if (lastMovingState && !isMoving) {

            ResetAttackTimers();
        }

        lastMovingState = isMoving;
    }



    public override void OnInit() {

        base.OnInit();

        HatItemData hat = charVisual.ChangeHats(hatID);
        if (hat != null) { hat.ApplyBoosterFor(this); }

        PantItemData pant = charVisual.ChangePants(pantID);
        if (pant != null) { pant.ApplyBoosterFor(this); }

        charVisual.ChangeAccessories(accessoryID);
    }

    public override void OnDespawn() {

    }

    public override bool IsMoving() {
        return isMoving;
    }
}
