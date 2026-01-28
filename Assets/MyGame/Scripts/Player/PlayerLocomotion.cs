using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerAnimation animationHandler;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;

    private PlayerTargetLock targetLockHandler;

    private bool isRunning = false;

    void Start()
    {
        targetLockHandler = GetComponent<PlayerTargetLock>();
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

    void Update()
    {
    }

    void FixedUpdate()
    {

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (PlayerCombat.Instance.GetIsRolling() || PlayerCombat.Instance.GetIsAttacking())
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

        Vector3 moveDir = (cameraForward * inputDir.z + cameraRight * inputDir.x).normalized;
        moveDir.y = 0f;

        // Tính tốc độ
        float moveValue = inputVector.magnitude;
        float currentSpeed = 0f;


        if (moveValue > 0.1f)
        {
            currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Di chuyển bằng Rigidbody
            Vector3 targetVelocity = moveDir * currentSpeed;
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



        if (targetLockHandler.GetIsTargeting())
        {
            Vector3 lookPos = targetLockHandler.GetCurrentTarget().position;
            lookPos.y = 0f;
            transform.LookAt(lookPos);
            UpdateAnimation(moveDir);
        }
        else if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            UpdateAnimation(currentSpeed);
        }
        else
        {
            UpdateAnimation(currentSpeed);
        }
    }


    private void UpdateAnimation(float currentSpeed)
    {
        float normalizedSpeed = currentSpeed / runSpeed;
        animationHandler.UpdateLocomotionAnimation(normalizedSpeed);
    }
    private void UpdateAnimation(Vector3 moveDir)
    {
        animationHandler.UpdateLockOnLocomotion(moveDir);
    }
}
