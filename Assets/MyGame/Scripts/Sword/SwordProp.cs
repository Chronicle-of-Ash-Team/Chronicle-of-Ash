using UnityEngine;

public class SwordProp : MonoBehaviour, IWeaponProp
{
    [SerializeField] private GameObject weaponPref;

    public GameObject GetWeaponPref()
    {
        return weaponPref;
    }
}
