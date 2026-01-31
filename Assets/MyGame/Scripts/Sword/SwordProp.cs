using UnityEngine;

public class SwordProp : MonoBehaviour, IWeaponProp
{
    [SerializeField] private WeaponData weaponData;

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }
}
