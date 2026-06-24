using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Collider col;
    private HashSet<IDamageable> hittedTargets = new();



    private void OnTriggerEnter(Collider other)
    {
        if (!col.enabled) return;

        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            if (hittedTargets.Contains(target)) return;
            if (other.transform == transform) return;

            hittedTargets.Add(target);
            //owner.OnWeaponHit(target, other, this);
        }
    }
}
