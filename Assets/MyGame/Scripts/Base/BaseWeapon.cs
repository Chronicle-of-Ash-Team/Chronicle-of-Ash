using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    protected IWeaponOwner owner;
    protected new BaseAnimation animation;

    public virtual void SetOwner(IWeaponOwner owner, BaseAnimation animEvent)
    {
        this.owner = owner;
        this.animation = animEvent;

        Subscribe();
    }

    protected abstract void Subscribe();
}
