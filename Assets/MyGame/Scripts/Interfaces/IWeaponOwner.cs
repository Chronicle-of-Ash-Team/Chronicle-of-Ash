using UnityEngine;

public interface IWeaponOwner
{
    int GetDamage();
    WeaponBase GetWeaponData();
    Transform GetTransform();
}
