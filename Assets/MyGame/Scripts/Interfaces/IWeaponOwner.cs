using UnityEngine;

public interface IWeaponOwner
{
    int GetDamage();
    Transform GetTransform();
    void OnWeaponHit(IDamageable target, Collider other, WeaponHitBox hitbox);
}
