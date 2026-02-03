using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : BaseWeapon
{
    private Collider col;
    private HashSet<IDamageable> hittedTargets = new();

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        DisableHitbox();
    }

    private void EnableHitbox()
    {
        hittedTargets.Clear();
        col.enabled = true;
    }

    private void DisableHitbox()
    {
        col.enabled = false;
    }


    protected override void Subscribe()
    {
        animation.OnAttackStart += Animation_OnAttackStart;
        animation.OnAttackEnd += Animation_OnAttackEnd;
    }

    private void OnDestroy()
    {
        animation.OnAttackStart -= Animation_OnAttackStart;
        animation.OnAttackEnd -= Animation_OnAttackEnd;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!col.enabled || owner == null) return;

        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            if (hittedTargets.Contains(target)) return;
            if (other.transform == owner.GetTransform()) return;

            hittedTargets.Add(target);
            target.TakeDamage(owner.GetDamage(), owner.GetTransform().gameObject);
        }
    }

    private void Animation_OnAttackEnd()
    {
        DisableHitbox();
    }

    private void Animation_OnAttackStart()
    {
        EnableHitbox();
    }
}
