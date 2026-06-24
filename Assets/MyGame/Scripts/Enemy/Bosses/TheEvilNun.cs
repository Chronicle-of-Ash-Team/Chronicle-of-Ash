using UnityEngine;

public class TheEvilNun : BaseBoss, IActionHandler
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private GameObject attack1Pref;
    [SerializeField] private GameObject attack2Pref;
    [SerializeField] private GameObject attack3Pref;

    public BaseCombatAction CurrentAction { get; private set; }
    private AttackAction attackAction;
    //private HitAction hitAction;

    protected override void Awake()
    {
        base.Awake();
        //hitAction = GetComponent<HitAction>();
        attackAction = GetComponent<AttackAction>();
    }

    private void Start()
    {
        baseAnimation.OnAttackStart += BaseAnimation_OnAttackStart;
        baseAnimation.OnAttackEnd += BaseAnimation_OnAttackEnd;
        baseAnimation.OnActionEventStart += BaseAnimation_OnActionEventStart;
        baseAnimation.OnActionEventEnd += BaseAnimation_OnActionEventEnd;
    }

    private void Update()
    {
        if (!isAlive) return;
        if (target == null) return;

        if (!target.GetComponent<IDamageable>().GetIsAlive())
        {
            target = null;
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        FaceTarget(target);

        float currentSpeed = 0f;

        if (distance > attackRange)
        {
            currentSpeed = walkSpeed;
            Vector3 moveDir = GetMoveDirToTarget(transform, target);
            HandleMovement(moveDir, false);
        }
        else
        {
            // Trong tầm đánh
            HandleMovement(Vector3.zero, false);
            TryAction(attackAction);
        }
        baseAnimation.UpdateLocomotionAnimation(currentSpeed);
    }

    private void BaseAnimation_OnActionEventStart()
    {
        //throw new NotImplementedException();
    }

    private void BaseAnimation_OnAttackStart()
    {
        switch (attackAction.attackComboCount)
        {
            case 1:
                Instantiate(attack1Pref, transform.position, Quaternion.identity);
                break;
            case 2:

                break;
            case 3:

                break;
        }
    }

    private void BaseAnimation_OnAttackEnd()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Enemy attack number " + attackAction.attackComboCount + " end");
    }

    private void BaseAnimation_OnActionEventEnd()
    {
        //throw new NotImplementedException();
    }

    public void TryAction(BaseCombatAction action)
    {
        if (!isAlive) return;

        if (CurrentAction != null && CurrentAction.IsRunning)
            return;
        // Không chạy lại cùng một action
        if (CurrentAction == action)
            return;

        // Execute action mới
        CurrentAction?.OnFinish();
        action.TryExecute();
        CurrentAction = action;
    }

    public void OnActionFinished(BaseCombatAction action)
    {
        if (CurrentAction == action)
            CurrentAction = null;
    }
}
