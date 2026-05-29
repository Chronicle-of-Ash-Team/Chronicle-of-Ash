using System;
using UnityEngine;

public class LandKnightBrain : MonoBehaviour, IDamageable, ILockable
{
    public LandKnightContext context;
    public Rigidbody rigidbody;
    public Transform lockOnTransform;

    private HierarchicalStateMachine stateMachine;
    private HierarchicalState rootState;

    private void Awake()
    {
        context.currentHealth = context.maxHealth;

        rigidbody = GetComponent<Rigidbody>();

        rootState = new LK_RootState(stateMachine, null, this, context);

        var builder = new StateMachineBuilder(rootState);
        stateMachine = builder.Build();
    }

    private void Update()
    {
        stateMachine.Tick(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        ApplyMovement(Time.fixedDeltaTime);
    }

    void ApplyMovement(float dt)
    {
        Vector3 targetVelocity =
            context.moveDirection * context.moveSpeed;

        targetVelocity.y = rigidbody.linearVelocity.y;

        rigidbody.linearVelocity = Vector3.MoveTowards(
            rigidbody.linearVelocity,
            targetVelocity,
            20f * dt);

        RotateTowardsMoveDirection(dt);
    }

    void RotateTowardsMoveDirection(float dt)
    {
        Vector3 dir = context.moveDirection;

        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRot =
            Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            context.rotationSpeed * dt);
    }

    public bool GetIsAlive()
    {
        return context.currentHealth > 0;
    }

    public void SetInvincible(bool invincible)
    {

    }

    public void TakeDamage(DamageContext damageContext)
    {
        context.currentHealth -= damageContext.Damage;

        context.posture += damageContext.Damage;

        context.staggerDirection =
            damageContext.HitDirection;

        if (context.posture >=
            context.maxPosture)
        {
            context.posture =
                context.maxPosture;

            context.postureBroken = true;
        }
    }

    public Transform GetLockOnTransform()
    {
        return lockOnTransform;
    }
}

[Serializable]
public class LandKnightContext
{
    public Animator animator;
    public AnimationEventRelay animationEventRelay;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Posture")]
    public float posture;
    public float maxPosture = 100f;

    public float postureRecoverRate = 15f;

    public bool postureBroken;

    [Header("Stagger")]
    public Vector3 staggerDirection;
    public float staggerForce = 4f;

    [Header("Movement")]
    public Vector3 moveDirection;
    public float moveSpeed = 3f;
    public float rotationSpeed = 3.5f;

    [Header("Patrol")]
    public Vector3 patrolCenter;
    public float patrolRange = 10f;
    public float patrolStamina = 90f;
    public float staminaDrainRate = 1f;
    public float staminaRecoverRate = 5f;
    public float maxPatrolStamina = 100f;

    [NonSerialized]
    public Vector3 currentPatrolPoint;
}
