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
    }

    private void PlayerAnimation_OnDodgeEnd()
    {
        OnFinish();
    }

    private void PlayerAnimation_OnDodgeStart()
    {
        damageable.SetInvincible(true);
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
        damageable.SetInvincible(false);
        IsRunning = false;
        rb.linearVelocity = Vector3.zero;
        locomotion.ResumeMove();
        actionHandler.OnActionFinished(this);
    }
}
