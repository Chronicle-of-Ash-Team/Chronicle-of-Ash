using UnityEngine;

public class SwordProp : MonoBehaviour, IWeaponProp
{
    [SerializeField] private WeaponBase weaponData;

    public WeaponBase GetWeaponData()
    {
        return weaponData;
    }
}
