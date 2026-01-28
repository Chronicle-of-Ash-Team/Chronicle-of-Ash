using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public WeaponType weaponType;
    public Sprite icon;

    [Header("Stats")]
    public float damage;
    public float attackSpeed;
    public float staminaCost;

    [Header("Animation")]
    public RuntimeAnimatorController animatorOverride;

    [Header("Prefab")]
    public GameObject weaponPrefab;
}

public enum WeaponType
{
    OneHandSword,
    TwoHandSword,
    Spear,
}
