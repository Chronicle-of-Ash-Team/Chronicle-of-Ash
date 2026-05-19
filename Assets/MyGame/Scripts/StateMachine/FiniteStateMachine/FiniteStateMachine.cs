using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FiniteStateMachine
{
    private StateNode current;
    private Dictionary<Type, StateNode> nodes = new();
    private HashSet<ITransition> anyTransitions = new();

    public void OnUpdate()
    {
        var transition = GetTransition();
        if(transition != null)
        {
            ChangeState(transition.To);
        }

        current.State?.OnUpdate();
    }


    public void OnFixesUpdate()
    {
        current.State?.OnFixedUpdate();
    }

    public void SetState(IState state)
    {
        current = nodes[state.GetType()];
        current.State?.OnEnter();
    }

    private void ChangeState(IFiniteState state)
    {
        if(state == current.State)
            return;

        var previousState = current.State;
        var nextState = nodes[state.GetType()].State;

        previousState?.OnExit();
        nextState?.OnEnter();
        current = nodes[state.GetType()];
    }

    private ITransition GetTransition()
    {
        foreach(var transition in anyTransitions)
        {
            if (transition.Codition.Evaluate())
            {
                return transition;
            }
        }

        foreach(var transition in current.Transitions)
        {
            if (transition.Codition.Evaluate())
            {
                return transition;
            }
        }

        return null;
    }

    public void AddTransition(IFiniteState from, IFiniteState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IFiniteState to, IPredicate condition)
    {
        anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    private StateNode GetOrAddNode(IFiniteState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());
        
        if(node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }
        return node;
    }
}
