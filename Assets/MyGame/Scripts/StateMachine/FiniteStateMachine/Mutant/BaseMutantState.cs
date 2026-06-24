using UnityEngine;

public abstract class BaseMutantState : IFiniteState
{
    protected readonly MutantBrain brain;
    protected readonly Animator animator;

    protected static readonly int Attack1Hash = Animator.StringToHash("Attack1");
    protected static readonly int WalkHash = Animator.StringToHash("Walking");
    protected static readonly int RunHash = Animator.StringToHash("Running");
    protected static readonly int IdleHash = Animator.StringToHash("Idle");
    protected static readonly int DeathHash = Animator.StringToHash("Dying");
    protected static readonly int HitHash = Animator.StringToHash("Hit");

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
