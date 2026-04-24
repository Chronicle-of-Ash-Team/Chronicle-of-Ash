using System;
using UnityEngine;

public class BaseBoss : BaseLocomotion, IDamageable, ILockable
{
    [SerializeField] private Transform lockOnPos;
    [SerializeField] protected Transform target;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth;
    [SerializeField] protected bool isAlive = true;
    [SerializeField] private bool isInvicible = false;

    [Header("Action Settings")]
    [SerializeField] private Hit_Event hitEvent;

    protected BaseAnimation baseAnimation;

    public Action OnBossDie;

    protected override void Awake()
    {
        base.Awake();
        baseAnimation = GetComponentInChildren<BaseAnimation>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(DamageContext damageContext)
    {
        hitEvent.Raise(damageContext);
        currentHealth -= damageContext.Damage;

        var checkAttacker = damageContext.Attacker.GetComponent<IDamageable>();
        if (checkAttacker != null)
        {
            target = damageContext.Attacker.transform;
        }

        if (currentHealth <= 0)
        {
            isAlive = false;
            OnBossDie?.Invoke();
            baseAnimation.PlayThisAnimation("Die", 0f);
        }
    }

    protected void FaceTarget(Transform target)
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

    protected Vector3 GetMoveDirToTarget(Transform self, Transform target)
    {
        Vector3 dir = target.position - self.position;
        dir.y = 0f;
        return dir.normalized;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public void SetInvincible(bool invincible)
    {
        isInvicible = invincible;
    }

    public Transform GetLockOnTransform()
    {
        return lockOnPos;
    }
}
