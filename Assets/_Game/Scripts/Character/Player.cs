using UnityEngine;

public class Player : BaseCharacter
{
    [Header("Player's Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("Check Obstacle Behavior")]
    [SerializeField] private Transform obstacleCheckPoint;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float checkDistance;

    private bool isMoving;
    private Vector3 moveDir;

    private void Update() {

        HandleMovement();

        HandleAttackBehavior();
    }

    private void HandleMovement() {

        // Run
        Vector2 inputVector = InputManager.Instance.GetInputVectorNormalized();
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        isMoving = moveDir != Vector3.zero;

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

        charTF.position += moveDir * moveSpeed * Time.deltaTime;

        // Rotate
        if (IsMoving()) {
            Quaternion targetLookAt = Quaternion.LookRotation(moveDir);
            charTF.rotation = Quaternion.RotateTowards(charTF.rotation, targetLookAt, rotateSpeed * Time.deltaTime);
        }
    }

    private void HandleAttackBehavior() {


        if (!IsMoving()) {
            // When player is not moving

            ScanTarget();
        }
    }

    private bool IsBlockedByObstacle(Vector3 checkDir) {

        bool canMoveForward = !(Physics.Raycast(obstacleCheckPoint.position, checkDir, out RaycastHit hitInfo, checkDistance, obstacleLayer));

        if (canMoveForward) {
            return false;
        }

        return true;
    }

    private void ScanTarget() {

    }
    
    public override bool IsMoving() {
        return isMoving;
    }
}
