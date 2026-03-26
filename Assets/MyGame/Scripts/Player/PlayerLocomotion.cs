using UnityEngine;

public class PlayerLocomotion : BaseLocomotion
{
    [SerializeField] private Transform cameraTransform;

    private PlayerTargetLock targetLockHandler;
    private PlayerAnimation playerAnimation;
    private PlayerHealth playerHealth;

    private bool isRunning;

    void Start()
    {
        targetLockHandler = GetComponent<PlayerTargetLock>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        if (!playerHealth.GetIsAlive()) return;
        Vector3 moveDir = CalculateMoveDirection();
        HandleMovement(moveDir, isRunning);
        UpdateAnimation(moveDir);
    }

    private void OnEnable()
    {
        GameInput.Instance.OnRunPerformed += GameInput_OnRunPerformed;
    }
    private void OnDisable()
    {
        GameInput.Instance.OnRunPerformed -= GameInput_OnRunPerformed;
    }

    private void GameInput_OnRunPerformed(bool obj)
    {
        isRunning = obj;
    }

    private Vector3 CalculateMoveDirection()
    {
        Vector2 input = GameInput.Instance.GetMovementVectorNormalized();
        if (input.magnitude < 0.1f) return Vector3.zero;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;

        return (camForward * input.y + camRight * input.x).normalized;
    }

    private void UpdateAnimation(Vector3 moveDir)
    {
        if (targetLockHandler != null && targetLockHandler.GetIsTargeting())
        {
            Vector3 lookPos = targetLockHandler.GetCurrentTarget().position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);

            playerAnimation.UpdateLockOnLocomotion(moveDir);
        }
        else
        {
            playerAnimation.UpdateLocomotionAnimation(base.currentSpeed / base.runSpeed);
        }
    }
}
