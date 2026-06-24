using UnityEngine;

public class MutantFleeState : BaseMutantState
{
    private Vector3 fleeDirection;

    private float zigzagTimer;

    public MutantFleeState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("FLEE");

        animator.CrossFade(RunHash, CrossFadeDuration);

        PickNewFleeDirection();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (brain.target == null)
        {
            StopMovement();
            return;
        }

        zigzagTimer -= Time.deltaTime;

        // Đổi hướng zig zag liên tục
        if (zigzagTimer <= 0f)
        {
            PickNewFleeDirection();
        }

        fleeDirection.y = 0f;

        if (fleeDirection.sqrMagnitude <= 0.01f)
        {
            StopMovement();
            return;
        }

        fleeDirection.Normalize();

        // Rotation
        Quaternion targetRotation =
            Quaternion.LookRotation(fleeDirection);

        brain.transform.rotation =
            Quaternion.Slerp(
                brain.transform.rotation,
                targetRotation,
                brain.rotationSpeed * Time.deltaTime
            );

        // Movement
        Vector3 velocity =
            fleeDirection * brain.runSpeed;

        velocity.y =
            brain.rigidbody.linearVelocity.y;

        brain.rigidbody.linearVelocity = velocity;
    }

    private void PickNewFleeDirection()
    {
        zigzagTimer = Random.Range(0.3f, 0.8f);

        // Hướng ngược player
        Vector3 awayDirection =
            brain.transform.position -
            brain.target.position;

        awayDirection.y = 0f;

        awayDirection.Normalize();

        // Zig zag random
        Vector3 randomSideOffset =
            new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            );

        fleeDirection =
            (awayDirection + randomSideOffset * 0.6f)
            .normalized;
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
