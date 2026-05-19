using UnityEngine;

public class FlowerPicker : MonoBehaviour, IDamageable
{
    public BasicStateMachine FSM {  get; private set;  }

    public PickFlowerState pickFlowerState;
    public ChooseFlowerState chooseFlowerState;
    public MoveToFlowerState moveToFlowerState;
    public RestState restState;
    public FallState fallState;
    public StandUpState standUpState;
    public DeadState deadState;


    public Vector3 currentFlowerTarget;

    public Rigidbody rigidbody;

    public Animator animator;

    public float moveSpeed = 1f;

    public float pickingDistance = 2f;

    public float chooseFlowerRadius = 10f;

    public float pickFlowerTime = 2f;

    public float restTime = 5f;

    public float fallTime = 1f;

    public float standUpTime = 1f;

    public bool isAlive = true;

    private void Awake()
    {
        FSM = new BasicStateMachine();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        pickFlowerState = new PickFlowerState(this);
        chooseFlowerState = new ChooseFlowerState(this);
        moveToFlowerState = new MoveToFlowerState(this);
        restState = new RestState(this);
        fallState = new FallState(this);
        standUpState = new StandUpState(this);
        deadState = new DeadState(this);
    }

    private void Start()
    {
        FSM.ChangeState(chooseFlowerState);
    }

    private void Update()
    {
        FSM.Update();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (FSM.currentState == fallState || FSM.currentState == deadState || FSM.currentState == standUpState) return;
        var obHit = collision.gameObject.TryGetComponent<IDamageable>(out var damageable);
        if( obHit )
        {
            FSM.ChangeState(fallState);
        }
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public void SetInvincible(bool invincible)
    {
    }

    public void TakeDamage(DamageContext damageContext)
    {
        if (isAlive)
        {
            isAlive = false;
            FSM.ChangeState(deadState);
        }
    }
}
