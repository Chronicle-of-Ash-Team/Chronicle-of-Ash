using UnityEngine;

public interface IWeaponOwner
{
    int GetDamage();
    WeaponBase GetWeaponData();
    Transform GetTransform();
    void OnWeaponHit(IDamageable target, Collider other, WeaponHitBox hitbox);
}
