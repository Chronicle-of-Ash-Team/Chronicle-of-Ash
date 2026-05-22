using UnityEngine;

public class MutantPatrolState : BaseMutantState
{
    private Vector3 targetPos;

    public MutantPatrolState(
        MutantBrain brain,
        Animator animator
    ) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("PATROL");

        animator.CrossFade(WalkHash, CrossFadeDuration);

        PickNewTarget();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Vector3 direction =
            (targetPos - brain.transform.position);

        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance < 0.5f)
        {
            PickNewTarget();
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

        velocity.y = brain.rigidbody.linearVelocity.y;

        brain.rigidbody.linearVelocity = velocity;

        brain.currentStamina -= Time.deltaTime * 2f;
    }

    public override void OnExit()
    {
        base.OnExit();

        Vector3 velocity = brain.rigidbody.linearVelocity;
        velocity.x = 0f;
        velocity.z = 0f;

        brain.rigidbody.linearVelocity = velocity;
    }

    private void PickNewTarget()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle =
                Random.insideUnitCircle *
                brain.patrolRange;

            Vector3 randomPos = new Vector3(
                brain.patrolPos.x + randomCircle.x,
                brain.transform.position.y,
                brain.patrolPos.z + randomCircle.y
            );

            if (Vector3.Distance(
                brain.transform.position,
                randomPos) > 2f)
            {
                targetPos = randomPos;
                return;
            }
        }

        targetPos = brain.patrolPos;
    }
}