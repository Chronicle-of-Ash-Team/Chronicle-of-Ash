using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseLocomotion : MonoBehaviour, IMove
{
    [SerializeField] protected float walkSpeed = 5f;
    [SerializeField] protected float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;

    protected float currentSpeed = 0f;
    private Vector3 currentMoveDir = Vector3.zero;
    private bool canMove = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void HandleMovement(Vector3 moveDir, bool isRunning)
    {
        if (!canMove)
        {
            StopVelocity();
            return;
        }

        currentMoveDir = moveDir;
        currentMoveDir.y = 0f;

        if (currentMoveDir.sqrMagnitude < 0.01f)
        {
            StopVelocity();
            return;
        }

        currentSpeed = isRunning ? runSpeed : walkSpeed;

        Move();
        Rotate();
    }

    protected void Move()
    {
        Vector3 velocity = currentMoveDir * currentSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    protected void Rotate()
    {
        if (currentMoveDir == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(currentMoveDir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

    protected void StopVelocity()
    {
        currentSpeed = 0f;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    public Vector3 GetCurrentMoveDir()
    {
        return currentMoveDir;
    }
    public void StopMove()
    {
        canMove = false;
    }
    public void ResumeMove()
    {
        canMove = true;
    }

}
