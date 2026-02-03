using UnityEngine;

public class AttackAction : BaseCombatAction
{
    [Header("Attack Settings")]
    [SerializeField] private float comboResetTime = 1.2f;
    [SerializeField] int attackComboCount = 1;

    private float comboTimer;

    private void Start()
    {
        baseAnimation.OnActionEventStart += BaseAnimation_OnActionEventStart;
        baseAnimation.OnActionEventEnd += BaseAnimation_OnActionEventEnd;
    }

    private void BaseAnimation_OnActionEventEnd()
    {
        if (!IsThisAction) return;
        OnFinish();
    }

    private void BaseAnimation_OnActionEventStart()
    {
        if (!IsThisAction) return;
        IsRunning = true;
    }

    private void Update()
    {
        HandleComboTimer();
    }

    private void HandleComboTimer()
    {
        if (attackComboCount == 0) return;

        comboTimer += Time.deltaTime;

        if (comboTimer >= comboResetTime)
        {
            attackComboCount = 0;
            comboTimer = 0f;
        }
    }

    protected override void Execute()
    {
        IsThisAction = true;

        locomotion.StopMove();
        comboTimer = 0f;

        attackComboCount++;

        if (attackComboCount > 3)
        {
            attackComboCount = 1;
        }
        baseAnimation.PlayThisAnimation("Attack" + attackComboCount.ToString());

    }

    public override void OnFinish()
    {
        IsThisAction = false;

        locomotion.ResumeMove();
        IsRunning = false;
        actionHandler.OnActionFinished(this);
    }
}
