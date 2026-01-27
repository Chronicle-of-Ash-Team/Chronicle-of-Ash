using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;


    public event Action OnDodge;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnDodge?.Invoke();
        }
    }
}
