using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInput playerInputAction;

    public event Action<bool> OnRunPerformed;
    public event Action OnLockOnPerformed;
    public event Action OnDodgePerformed;

    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInput();

        playerInputAction.Enable();
    }

    private void OnEnable()
    {
        playerInputAction.Player.Run.performed += Run_performed;
        playerInputAction.Player.Run.canceled += Run_canceled;
        playerInputAction.Player.LockOn.performed += LockOn_performed;
        playerInputAction.Player.Dodge.performed += Dodge_performed;
    }

    private void Dodge_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDodgePerformed?.Invoke();
    }

    private void LockOn_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnLockOnPerformed?.Invoke();
    }

    private void Run_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnRunPerformed?.Invoke(false);
    }

    private void Run_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnRunPerformed?.Invoke(true);
    }


    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
    //public Vector2 GetLookVectorNormalized()
    //{
    //    Vector2 inputVector = playerInputAction.Player.Look.ReadValue<Vector2>();

    //    inputVector = inputVector.normalized;

    //    return inputVector;
    //}
}
