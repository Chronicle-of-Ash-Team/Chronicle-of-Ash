using UnityEngine;

public interface IMove
{
    public Vector3 GetCurrentMoveDir();
    public void ResumeMove();
    public void StopMove();
}
