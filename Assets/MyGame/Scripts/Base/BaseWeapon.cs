using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    protected IWeaponOwner owner;
    protected PlayerAnimation animation;

    public virtual void SetOwner(IWeaponOwner owner, PlayerAnimation animEvent)
    {
        this.owner = owner;
        this.animation = animEvent;

        Subscribe();
    }

    protected abstract void Subscribe();
}
