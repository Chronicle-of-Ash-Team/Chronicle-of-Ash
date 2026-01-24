using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;

    private PlayerAnimation animationHandler;

    private bool isRunning = false;

    void Start()
    {
        animationHandler = GetComponent<PlayerAnimation>();
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

            if (moveDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
        else
        {
            // Dừng di chuyển
            Vector3 stopVelocity = rb.linearVelocity;
            stopVelocity.x = 0f;
            stopVelocity.z = 0f;
            rb.linearVelocity = stopVelocity;
        }

        UpdateAnimation(currentSpeed);
    }

    private void UpdateAnimation(float currentSpeed)
    {
        // Tính normalized speed (0 = idle, 0.5 = walk, 1 = run)
        float normalizedSpeed = currentSpeed / runSpeed;

        // Gọi AnimationHandler để update animation
        animationHandler.UpdateLocomotionAnimation(normalizedSpeed);
    }
}
