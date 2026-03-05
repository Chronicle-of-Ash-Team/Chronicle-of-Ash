using UnityEngine;

public struct DamageContext
{
    public int Damage;
    public GameObject Attacker;
    public Vector3 HitDirection;
    public Vector3 HitPosition;
    public DamageType DamageType;
}
public enum DamageType
{
    Normal,
    Heavy,
    Unblockable
}
