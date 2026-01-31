using UnityEngine;

public class DodgeAction : BaseCombatAction
{
    [SerializeField] float rollSpeed = 8f;
    Vector3 rollDirection;

    private void Start()
    {
        playerAnimation.OnDodgeStart += PlayerAnimation_OnDodgeStart;
    }

    private void PlayerAnimation_OnDodgeStart()
    {
        IsRunning = true;
    }

    private void FixedUpdate()
    {
        HandleDodge();
    }

    private void HandleDodge()
    {
        if (!IsRunning) return;
        base.rb.linearVelocity = rollDirection * rollSpeed;
        transform.forward = rollDirection;
    }

    protected override void Execute()
    {
        playerLocomotion.StopMove();

        playerAnimation.PlayDodge();

        Vector2 moveInput = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 moveDir = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        if (moveDir.sqrMagnitude < 0.01f)
        {
            moveDir = transform.forward;
        }

        rollDirection = moveDir.normalized;
        transform.forward = rollDirection;
    }
    public override void OnFinish()
    {
        IsRunning = false;
        rb.linearVelocity = Vector3.zero;
        playerLocomotion.ResumeMove();
    }
}
