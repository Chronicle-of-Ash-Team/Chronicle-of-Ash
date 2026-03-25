using System;
using UnityEngine;

public abstract class EventBase<T> : ScriptableObject
{
    private Action<T> OnEventRaised;

    public void Raise(T value)
    {
        OnEventRaised?.Invoke(value);
    }
}
