using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    protected IWeaponOwner owner;

    public virtual void Init(IWeaponOwner owner)
    {
        this.owner = owner;
    }

    protected abstract void Subscribe();
}
