using System;
using UnityEngine;

public class LandKnightBrain : MonoBehaviour, IDamageable, ILockable, IWeaponOwner
{
    public LandKnightContext context;
    public Rigidbody rigidbody;
    public Transform lockOnTransform;
    public WeaponHitBox[] weaponHitBoxes;

    private HierarchicalStateMachine stateMachine;
    private HierarchicalState rootState;

    public Action<OnHit> OnHitEvent;

    public class OnHit
    {
        public DamageContext DamageContext;

    }

    private void Awake()
    {
        context.currentHealth = context.maxHealth;

        rigidbody = GetComponent<Rigidbody>();

        rootState = new LK_RootState(stateMachine, null, this, context);

        var builder = new StateMachineBuilder(rootState);
        stateMachine = builder.Build();

        foreach (var hitbox in weaponHitBoxes)
        {
            hitbox.Init(this);
        }
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

    public bool IsPlayerInRange(float range)
    {
        if (context.target == null)
            return false;

        return
            (context.target.position -
             transform.position).sqrMagnitude
            <= range * range;
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
        OnHitEvent?.Invoke(new OnHit { DamageContext = damageContext });

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

    public int GetDamage()
    {
        return 2;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public void OnWeaponHit(IDamageable target, Collider other, WeaponHitBox hitbox)
    {
        target.TakeDamage(new DamageContext
        {
            Damage = context.damage,
            Attacker = gameObject,
            HitDirection = other.transform.position - transform.position,
            HitPosition = other.ClosestPoint(transform.position),
            DamageType = DamageType.Heavy
        });
    }

    public void EnableHitbox()
    {
        foreach (var hitBox in weaponHitBoxes)
        {
            hitBox.EnableHitbox();
        }
    }
    public void DisableHitbox()
    {
        foreach (var hitBox in weaponHitBoxes)
        {
            hitBox.DisableHitbox();
        }
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

    [Header("Combat")]
    public float combatDistance = 5f;
    public int damage = 2;

    [Header("Skill")]
    public GameObject projectileSkill;
    public int projectileDamage = 5;
    public GameObject aoeSkill;
    public int aoeDamage = 4;

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

    public Transform target;
    public float detectionRange = 8f;
    public float loseAggroRange = 12f;

    [NonSerialized]
    public Vector3 currentPatrolPoint;
}
