using UnityEngine;

public class TheEvilNun : BaseLocomotion, ILockable, IActionHandler, IDamageable
{
    [Header("Common Settings")]
    [SerializeField] private Transform lockOnPos;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isAlive = true;

    [Header("Attack Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange = 10f;

    [Header("Action Settings")]
    [SerializeField] private Hit_Event hitEvent;
    public BaseCombatAction CurrentAction { get; private set; }
    private AttackAction attackAction;
    //private HitAction hitAction;

    private BaseAnimation baseAnimation;

    protected override void Awake()
    {
        base.Awake();
        baseAnimation = GetComponentInChildren<BaseAnimation>();
        //hitAction = GetComponent<HitAction>();
        attackAction = GetComponent<AttackAction>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        baseAnimation.OnAttackStart += BaseAnimation_OnAttackStart;
        baseAnimation.OnAttackEnd += BaseAnimation_OnAttackEnd;
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

    private void BaseAnimation_OnAttackEnd()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Enemy attack number " + attackAction.attackComboCount + " end");
    }

    private void BaseAnimation_OnAttackStart()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Enemy attack number " + attackAction.attackComboCount + " start");
    }

    private Vector3 GetMoveDirToTarget(Transform self, Transform target)
    {
        Vector3 dir = target.position - self.position;
        dir.y = 0f;
        return dir.normalized;
    }

    private void FaceTarget(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotationSpeed
        );
    }

    public void TryAction(BaseCombatAction action)
    {
        if (!isAlive) return;
        //if (action == hitAction)
        //{
        //    if (CurrentAction != null)
        //    {
        //        CurrentAction.OnFinish();
        //        CurrentAction = null;
        //    }

        //    action.TryExecute();
        //    CurrentAction = action;

        //    Debug.Log("Enemy hit");
        //    return;
        //}


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

    public void TakeDamage(DamageContext damageContext)
    {
        hitEvent.Raise(damageContext);
        currentHealth -= damageContext.Damage;

        // Nếu attacker có thể bị damage, thì set target thành attacker
        var checkAttacker = damageContext.Attacker.GetComponent<IDamageable>();
        if (checkAttacker != null)
        {
            target = damageContext.Attacker.transform;
        }

        if (currentHealth <= 0)
        {
            isAlive = false;
            baseAnimation.PlayThisAnimation("Die", 0f);
        }
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public Transform GetLockOnTransform()
    {
        return lockOnPos;
    }

    public void OnActionFinished(BaseCombatAction action)
    {
        if (CurrentAction == action)
            CurrentAction = null;
    }

    public void SetInvincible(bool invincible)
    {
        //throw new System.NotImplementedException();
    }
}
