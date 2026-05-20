using UnityEngine;

public class MutantChaseState : BaseMutantState
{
    public MutantChaseState(
        MutantBrain brain,
        Animator animator
    ) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("CHASE");

        animator.CrossFade(RunHash, CrossFadeDuration);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (brain.target == null)
        {
            return;
        }

        Vector3 direction =
            brain.target.position -
            brain.transform.position;

        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance <= brain.attackRange)
        {
            StopMovement();
            return;
        }

        direction.Normalize();

        // Rotation
        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        brain.transform.rotation =
            Quaternion.Slerp(
                brain.transform.rotation,
                targetRotation,
                brain.rotationSpeed * Time.deltaTime
            );

        // Rigidbody movement
        Vector3 velocity =
            direction * brain.moveSpeed;

        velocity.y =
            brain.rigidbody.linearVelocity.y;

        brain.rigidbody.linearVelocity = velocity;
    }

    public override void OnExit()
    {
        base.OnExit();

        StopMovement();
    }

    private void StopMovement()
    {
        Vector3 velocity =
            brain.rigidbody.linearVelocity;

        velocity.x = 0f;
        velocity.z = 0f;

        brain.rigidbody.linearVelocity = velocity;
    }
}