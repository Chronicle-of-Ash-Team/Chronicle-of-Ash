using UnityEngine;

public abstract class BaseMutantState : IFiniteState
{
    protected readonly MutantBrain brain;
    protected readonly Animator animator;

    protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    protected static readonly int AttackHash = Animator.StringToHash("Attack");
    protected static readonly int WalkHash = Animator.StringToHash("Walking");
    protected static readonly int IdleHash = Animator.StringToHash("Idle");

    protected const float CrossFadeDuration = 0.1f;

    public BaseMutantState(MutantBrain brain, Animator animator)
    {
        this.brain = brain;
        this.animator = animator;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnUpdate()
    {
    }
}
