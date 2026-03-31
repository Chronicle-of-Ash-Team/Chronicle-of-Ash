using UnityEngine;

public class WeaponProp : MonoBehaviour, IWeaponProp
{
    [SerializeField] private GameObject weaponPref;

    public GameObject GetWeaponPref()
    {
        return weaponPref;
    }
}
