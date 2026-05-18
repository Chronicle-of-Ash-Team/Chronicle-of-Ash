using UnityEngine;

public class FlowerPicker : MonoBehaviour
{
    public BasicStateMachine FSM {  get; private set;  }

    public Vector3 currentFlowerTarget;

    public Rigidbody rigidbody;

    public float moveSpeed = 1f;

    public float pickingDistance = 2f;

    public float chooseFlowerRadius = 10f;

    public float pickFlowerTime = 2f;


    private void Awake()
    {
        FSM = new BasicStateMachine();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        FSM.ChangeState(new ChooseFlowerState(this));
    }

    private void Update()
    {
        FSM.Update();
    }
}
