using UnityEngine;

public class PlayerLocomotion : MonoBehaviour, IMove
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;

    private PlayerTargetLock targetLockHandler;
    private PlayerAnimation playerAnimation;
    private Rigidbody rb;

    private bool isRunning = false;
    private float currentSpeed = 0f;
    private Vector3 currentMoveDir = Vector3.zero;
    private bool canMove = true;

    void Start()
    {
        targetLockHandler = GetComponent<PlayerTargetLock>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        GameInput.Instance.OnRunPerformed += GameInput_OnRunPerformed;
    }

    private void GameInput_OnRunPerformed(bool obj)
    {
        isRunning = obj;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!canMove)
        {
            Vector3 stopVelocity = rb.linearVelocity;
            stopVelocity.x = 0f;
            stopVelocity.z = 0f;
            rb.linearVelocity = stopVelocity;
            return;
        }

        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 inputDir = new Vector3(inputVector.x, 0f, inputVector.y);


        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        currentMoveDir = (cameraForward * inputDir.z + cameraRight * inputDir.x).normalized;
        currentMoveDir.y = 0f;

        // Tính tốc độ
        float moveValue = inputVector.magnitude;
        currentSpeed = 0f;


        if (moveValue > 0.1f)
        {
            currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Di chuyển bằng Rigidbody
            Vector3 targetVelocity = currentMoveDir * currentSpeed;
            targetVelocity.y = rb.linearVelocity.y; // Giữ velocity Y (gravity)
            rb.linearVelocity = targetVelocity;
        }
        else
        {
            // Dừng di chuyển
            Vector3 stopVelocity = rb.linearVelocity;
            stopVelocity.x = 0f;
            stopVelocity.z = 0f;
            rb.linearVelocity = stopVelocity;
        }

        if (currentMoveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentMoveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        if (targetLockHandler.GetIsTargeting())
        {
            Vector3 lookPos = targetLockHandler.GetCurrentTarget().position;
            lookPos.y = 0f;
            transform.LookAt(lookPos);
            playerAnimation.UpdateLockOnLocomotion(currentMoveDir);
        }
        else
        {
            playerAnimation.UpdateLocomotionAnimation(currentSpeed / runSpeed);
        }
    }

    public Vector3 GetCurrentMoveDir()
    {
        return currentMoveDir;
    }
    public void StopMove()
    {
        canMove = false;
    }
    public void ResumeMove()
    {
        canMove = true;
    }
}
