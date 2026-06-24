using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    private Collider col;
    private HashSet<IDamageable> hittedTargets = new();
    private IWeaponOwner owner;

    private GameObject trailEffect;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        trailEffect = transform.GetComponentInChildren<ParticleSystem>()?.gameObject;
        DisableHitbox();
    }

    public void Init(IWeaponOwner owner)
    {
        this.owner = owner;
    }

    public void EnableHitbox()
    {
        hittedTargets.Clear();
        if (trailEffect != null)
        {
            trailEffect.SetActive(true);
        }

        col.enabled = true;
    }

    public void DisableHitbox()
    {
        if (trailEffect != null)
        {
            trailEffect.SetActive(false);
        }
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
