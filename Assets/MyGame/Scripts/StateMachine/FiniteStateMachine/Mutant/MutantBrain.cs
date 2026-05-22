using UnityEngine;

public class MutantBrain : MonoBehaviour, IDamageable, IWeaponOwner, ILockable
{
    public Transform target;
    public Animator animator;
    public Rigidbody rigidbody;
    public MutantAnimatorHandler mutantAnimatorHandler;
    public Transform lockOnPos;
    public Vector3 patrolPos;

    public float patrolRange = 10f;
    public float attackRange = 2f;
    public float detectRange = 8f;
    public float fleeRange = 20f;

    public float rotationSpeed = 10f;
    public float moveSpeed = 3f;
    public float runSpeed = 4f;

    public int maxHp = 10;
    public float maxStamina = 100f;

    public float currentStamina = 100f;
    public int currentHp;

    public int damage = 2;

    public float staggerDuration = 1f;

    public bool wasHit = false;
    public bool isAttacking = false;

    private FiniteStateMachine FSM;

    // Nghỉ
    public MutantIdleState idleState;
    // Đi tuần
    public MutantPatrolState patrolState;
    // Đuổi target
    public MutantChaseState chaseState;
    // Tấn công
    public MutantAttackState attackState;
    // Bị đánh
    public MutantStaggerState staggerState;
    // Chạy trốn
    public MutantFleeState fleeState;
    // Quay lại đi tuần
    public MutantReturnState returnState;
    // Chết
    public MutantDeadState deadState;

    private void Awake()
    {
        currentHp = maxHp;
        patrolPos = transform.position;

        FSM = new FiniteStateMachine();

        idleState = new MutantIdleState(this, animator);
        patrolState = new MutantPatrolState(this, animator);
        chaseState = new MutantChaseState(this, animator);
        attackState = new MutantAttackState(this, animator);
        staggerState = new MutantStaggerState(this, animator);
        fleeState = new MutantFleeState(this, animator);
        returnState = new MutantReturnState(this, animator);
        deadState = new MutantDeadState(this, animator);

        SetUpTransitions();
    }

    private void Start()
    {
        FSM.SetState(patrolState);
    }

    private void Update()
    {
        FSM.OnUpdate();
    }

    private void FixedUpdate()
    {
        FSM.OnFixedUpdate();
    }

    private void SetUpTransitions()
    {
        // Any -> Dead
        FSM.AddAnyTransition(
            deadState,
            new FuncPredicate(() => currentHp <= 0)
        );

        // Patrol -> Chase
        FSM.AddTransition(
            patrolState,
            chaseState,
            new FuncPredicate(() => HasTarget())
        );

        // Chase -> Flee
        FSM.AddTransition(
            chaseState,
            fleeState,
            new FuncPredicate(() => ((float)currentHp / maxHp) * 100f <= 20f)
        );

        // Chase -> Attack
        FSM.AddTransition(
            chaseState,
            attackState,
            new FuncPredicate(() => InAttackRange())
        );

        // Attack -> Chase
        FSM.AddTransition(
            attackState,
            chaseState,
            new FuncPredicate(() => !InAttackRange() && !isAttacking)
        );

        // Chase -> Patrol
        FSM.AddTransition(
            chaseState,
            patrolState,
            new FuncPredicate(() => !HasTarget())
        );

        // Stagger -> Chase
        FSM.AddTransition(
            staggerState,
            chaseState,
            new FuncPredicate(() => !wasHit)
        );

        // Any -> Stagger
        FSM.AddAnyTransition(
            staggerState,
            new FuncPredicate(() => wasHit && currentHp > 0)
        );

        // Flee -> Patrol
        FSM.AddTransition(
            fleeState,
            patrolState,
            new FuncPredicate(() => !InFleeRange())
        );

        // Patrol -> Idle
        FSM.AddTransition(
            patrolState,
            idleState,
            new FuncPredicate(() => currentStamina <= 0)
        );

        // Idle -> Patrol
        FSM.AddTransition(
            idleState,
            patrolState,
            new FuncPredicate(() => currentStamina >= 90)
        );
    }

    public bool GetIsAlive()
    {
        return currentHp > 0;
    }

    public void SetInvincible(bool invincible)
    {

    }

    public bool HasTarget()
    {
        if (target == null)
            return false;

        return Vector3.Distance(
            transform.position,
            target.position
        ) <= detectRange;
    }

    public bool InAttackRange()
    {
        if (target == null)
            return false;

        return Vector3.Distance(
            transform.position,
            target.position
        ) <= attackRange;
    }

    public bool InFleeRange()
    {
        if (target == null)
            return false;
        return Vector3.Distance(
            transform.position,
            target.position
        ) <= fleeRange;
    }

    public void TakeDamage(DamageContext damageContext)
    {
        currentHp -= damageContext.Damage;
        wasHit = true;
    }
    public void ClearHit()
    {
        wasHit = false;
    }

    public int GetDamage()
    {
        return damage;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnWeaponHit(IDamageable target, Collider other, WeaponHitBox hitbox)
    {
        target.TakeDamage(new DamageContext
        {
            Damage = damage,
            Attacker = gameObject,
            HitDirection = other.transform.position - transform.position,
            HitPosition = other.ClosestPoint(transform.position),
            DamageType = DamageType.Heavy
        });
    }

    public Transform GetLockOnTransform()
    {
        return lockOnPos;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            patrolPos,
            patrolRange
        );
    }
}
