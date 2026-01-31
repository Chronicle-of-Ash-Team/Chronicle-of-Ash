using UnityEngine;

public abstract class BaseCombatAction : MonoBehaviour
{
    [SerializeField] protected int staminaCost;
    //protected PlayerStamina stamina;
    protected PlayerAnimation playerAnimation;
    protected Rigidbody rb;
    protected IMove playerLocomotion;

    public bool IsRunning { get; protected set; }

    protected virtual void Awake()
    {
        //stamina = GetComponent<PlayerStamina>();
        playerLocomotion = GetComponent<IMove>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        rb = GetComponent<Rigidbody>();
    }

    public bool CanExecute()
    {
        //return !IsRunning && stamina.Current >= staminaCost;
        return !IsRunning;
    }

    public void TryExecute()
    {
        if (!CanExecute()) return;

        //stamina.Consume(staminaCost);
        //IsRunning = true;
        Execute();
    }

    protected abstract void Execute();
    public abstract void OnFinish();
}
