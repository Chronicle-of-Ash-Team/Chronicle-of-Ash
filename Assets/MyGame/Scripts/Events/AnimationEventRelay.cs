using System;
using System.Threading.Tasks;
using UnityEngine;


public class AnimationEventRelay : MonoBehaviour
{
    public event Action<string> EventRaised;

    public void RaiseEvent(string eventName)
    {
        EventRaised?.Invoke(eventName);
    }

    public Task WaitEvent(string eventName)
    {
        var tcs = new TaskCompletionSource<bool>();

        void Handler(string evt)
        {
            if (evt != eventName) return;

            EventRaised -= Handler;
            tcs.TrySetResult(true);
        }

        EventRaised += Handler;

        return tcs.Task;
    }
}
