using UnityEngine;

public class DodgeAction : BaseCombatAction
{
    [SerializeField] float rollSpeed = 8f;
    Vector3 rollDirection;
    IDamageable damageable;

    private void Start()
    {
        damageable = GetComponent<IDamageable>();

        baseAnimation.OnDodgeStart += PlayerAnimation_OnDodgeStart;
        baseAnimation.OnDodgeEnd += PlayerAnimation_OnDodgeEnd;

        baseAnimation.OnActionEventStart += BaseAnimation_OnActionEventStart;
        baseAnimation.OnActionEventEnd += BaseAnimation_OnActionEventEnd;
    }

    private void BaseAnimation_OnActionEventEnd()
    {
        IsRunning = false;
        if (!IsThisAction) return;
        OnFinish();
    }

    private void BaseAnimation_OnActionEventStart()
    {
        if (!IsThisAction) return;
        IsRunning = true;
    }

    private void PlayerAnimation_OnDodgeEnd()
    {
        damageable.SetInvincible(false);
    }

    private void PlayerAnimation_OnDodgeStart()
    {
        damageable.SetInvincible(true);

    }

    private void FixedUpdate()
    {
        HandleDodge();
    }

    private void HandleDodge()
    {
        if (!IsRunning || !IsThisAction) return;
        base.rb.linearVelocity = rollDirection * rollSpeed;
        transform.forward = rollDirection;
    }

    protected override void Execute()
    {
        IsThisAction = true;

        locomotion.StopMove();

        baseAnimation.PlayThisAnimation("Roll");

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
        IsThisAction = false;

        IsRunning = false;
        rb.linearVelocity = Vector3.zero;
        locomotion.ResumeMove();
        actionHandler.OnActionFinished(this);
    }
}
