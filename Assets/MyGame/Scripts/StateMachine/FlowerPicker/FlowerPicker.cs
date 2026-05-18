using UnityEngine;

public class FlowerPicker : MonoBehaviour
{
    public BasicStateMachine FSM {  get; private set;  }

    public PickFlowerState pickFlowerState;
    public ChooseFlowerState chooseFlowerState;
    public MoveToFlowerState moveToFlowerState;
    public RestState restState;


    public Vector3 currentFlowerTarget;

    public Rigidbody rigidbody;

    public Animator animator;

    public float moveSpeed = 1f;

    public float pickingDistance = 2f;

    public float chooseFlowerRadius = 10f;

    public float pickFlowerTime = 2f;


    private void Awake()
    {
        FSM = new BasicStateMachine();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();


        pickFlowerState = new PickFlowerState(this);
        chooseFlowerState = new ChooseFlowerState(this);
        moveToFlowerState = new MoveToFlowerState(this);
        restState = new RestState(this);
    }

    private void Start()
    {
        FSM.ChangeState(chooseFlowerState);
    }

    private void Update()
    {
        FSM.Update();
    }
}
