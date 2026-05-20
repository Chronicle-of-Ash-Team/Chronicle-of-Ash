using System;
using UnityEngine;

public class MutantBrain : MonoBehaviour, IDamageable
{
    public Transform target;
    public Animator animator;
    public Rigidbody rigidbody;

    public float rotationSpeed = 10f;
    public float detectRange = 8f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public float runSpeed = 4f;

    public int maxHp = 10;
    public int currentHp;

    public bool wasHit;

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
        // Patrol -> Chase
        FSM.AddTransition(
            patrolState,
            chaseState,
            new FuncPredicate(() => HasTarget())
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
            new FuncPredicate(() => !InAttackRange())
        );

        // Chase -> Patrol
        FSM.AddTransition(
            chaseState,
            patrolState,
            new FuncPredicate(() => !HasTarget())
        );

        // Any -> Stagger
        FSM.AddAnyTransition(
            staggerState,
            new FuncPredicate(() => wasHit && currentHp > 0)
        );

        // Any -> Dead
        FSM.AddAnyTransition(
            deadState,
            new FuncPredicate(() => currentHp <= 0)
        );

        // Chase -> Flee
        FSM.AddTransition(
            chaseState,
            fleeState,
            new FuncPredicate(() => currentHp <= 20)
        );

        // Flee -> Patrol
        FSM.AddTransition(
            fleeState,
            patrolState,
            new FuncPredicate(() => !HasTarget())
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

    public void TakeDamage(DamageContext damageContext)
    {
        currentHp -= damageContext.Damage;
        wasHit = true;
    }
    public void ClearHit()
    {
        wasHit = false;
    }
}
