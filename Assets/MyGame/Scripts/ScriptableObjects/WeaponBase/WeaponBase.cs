using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public WeaponType weaponType;
    public Sprite icon;

    [Header("Stats")]
    public int damage = 1;
    public int staminaCost = 1;

    [Header("Animation")]
    public RuntimeAnimatorController animatorOverride;
    public float speed = 1;

    [Header("Prefab")]
    public GameObject weaponPrefab;

    //[Header("Skill")]

    public abstract void Execute(WeaponSkillContext context);
}

public enum WeaponType
{
    OneHandSword,
    TwoHandSword,
    Spear,
}

public class WeaponSkillContext
{
    public Transform caster;
    public Transform target;
    public float damage;
}

