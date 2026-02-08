using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    private Collider col;
    private HashSet<IDamageable> hittedTargets = new();
    private IWeaponOwner owner;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        DisableHitbox();
    }

    public void Init(IWeaponOwner owner)
    {
        this.owner = owner;
    }

    public void EnableHitbox()
    {
        hittedTargets.Clear();
        col.enabled = true;
    }

    public void DisableHitbox()
    {
        col.enabled = false;
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
            owner.OnWeaponHit(target, other, this);
        }
    }
}
