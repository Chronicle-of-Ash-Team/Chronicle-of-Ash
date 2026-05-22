using UnityEngine;

public class MutantAttackState : BaseMutantState
{
    public MutantAttackState(MutantBrain brain, Animator animator) : base(brain, animator)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ATTACK");
        animator.CrossFade(Attack1Hash, CrossFadeDuration);
        brain.isAttacking = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (brain.target == null)
            return;

        Vector3 direction =
            brain.target.position -
            brain.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        direction.Normalize();

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        brain.transform.rotation =
            Quaternion.Slerp(
                brain.transform.rotation,
                targetRotation,
                brain.rotationSpeed * 0.5f * Time.deltaTime
            );
    }

    public override void OnExit()
    {
        base.OnExit();

        brain.mutantAnimatorHandler.EndAttack();

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
