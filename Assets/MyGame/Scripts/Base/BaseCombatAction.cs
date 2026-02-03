using UnityEngine;

public abstract class BaseCombatAction : MonoBehaviour
{
    [SerializeField] protected int staminaCost;
    //protected PlayerStamina stamina;
    protected BaseAnimation baseAnimation;
    protected IActionHandler actionHandler;
    protected IMove locomotion;
    protected Rigidbody rb;

    public bool IsRunning { get; protected set; }

    protected virtual void Awake()
    {
        //stamina = GetComponent<PlayerStamina>();
        locomotion = GetComponent<IMove>();
        actionHandler = GetComponent<IActionHandler>();
        baseAnimation = GetComponentInChildren<BaseAnimation>();
        rb = GetComponent<Rigidbody>();
    }

    public bool CanExecute()
    {
        //return !IsRunning && stamina.Current >= staminaCost;
        return !IsRunning;
    }

    public virtual void TryExecute()
    {
        if (!CanExecute()) return;

        //stamina.Consume(staminaCost);
        //IsRunning = true;
        Execute();
    }

    protected abstract void Execute();
    public abstract void OnFinish();
}
