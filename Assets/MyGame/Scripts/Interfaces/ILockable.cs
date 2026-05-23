using UnityEngine;

public interface ILockable
{
    public Transform GetLockOnTransform();
    public bool GetIsAlive();
}
