using System;
using System.Collections.Generic;
using System.Threading;

public class TransititionSequencer
{
    public readonly HierarchicalStateMachine Machine;

    private ISequence sequencer;
    private Action nextPhase;
    private (HierarchicalState from, HierarchicalState to)? pending;
    HierarchicalState lastFrom, lastTo;



    public TransititionSequencer(HierarchicalStateMachine machine)
    {
        Machine = machine;
    }

    public void RequestTransition(HierarchicalState from, HierarchicalState to)
    {
        if (to == null || from == to) return;
        if (sequencer != null) { pending = (from, to); return; }
        BeginTransition(from, to);
    }

    static List<PhaseStep> GatherPhaseSteps(List<HierarchicalState> chain, bool deactive)
    {
        var steps = new List<PhaseStep>();

        for (int i = 0; i < chain.Count; i++)
        {
            var activities = chain[i].Activities;
            for (int j = 0; j < activities.Count; j++)
            {
                var a = activities[j];
                if (deactive)
                {
                    if (a.Mode == ActivityMode.Active)
                    {
                        steps.Add(cancellationToken => a.DeactivateAsync(cancellationToken));
                    }
                }
                else
                {
                    if (a.Mode == ActivityMode.Inactive)
                    {
                        steps.Add(cancellationToken => a.ActivateAsync(cancellationToken));
                    }
                }
            }
        }
        return steps;
    }

    static List<HierarchicalState> StatesToExit(HierarchicalState from, HierarchicalState lca)
    {
        var result = new List<HierarchicalState>();
        for (var s = from; s != null && s != lca; s = s.Parent)
        {
            result.Add(s);
        }
        return result;
    }

    static List<HierarchicalState> StatesToEnter(HierarchicalState to, HierarchicalState lca)
    {
        var result = new Stack<HierarchicalState>();
        for (var s = to; s != null && s != lca; s = s.Parent)
        {
            result.Push(s);
        }
        return new List<HierarchicalState>(result);
    }


    CancellationTokenSource cancellationTokenSource;
    public readonly bool UseSequential = true;
    void BeginTransition(HierarchicalState from, HierarchicalState to)
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();

        cancellationTokenSource =
            new CancellationTokenSource();
        var lca = Lca(from, to);
        var exitChain = StatesToExit(from, lca);
        var enterChain = StatesToEnter(to, lca);

        // Deactivate old branch
        var exitSteps = GatherPhaseSteps(exitChain, deactive: true);
        sequencer = UseSequential ? new SequentialPhase(exitSteps, cancellationTokenSource.Token) : new ParallelPhase(exitSteps, cancellationTokenSource.Token);
        sequencer.Start();

        nextPhase = () =>
        {
            // Change State
            Machine.ChangeState(from, to);
            // Activate new branch
            var enterSteps = GatherPhaseSteps(enterChain, deactive: false);
            sequencer = UseSequential ? new SequentialPhase(enterSteps, cancellationTokenSource.Token) : new ParallelPhase(enterSteps, cancellationTokenSource.Token);
            sequencer.Start();
        };


    }

    void EndTransition()
    {
        sequencer = null;
        if (pending.HasValue)
        {
            var p = pending.Value;
            pending = null;
            BeginTransition(p.from, p.to);
        }
    }

    public void Tick(float deltaTime)
    {
        if (sequencer != null)
        {
            if (sequencer.Update())
            {
                if (nextPhase != null)
                {
                    var np = nextPhase;
                    nextPhase = null;
                    np();
                }
                else
                {
                    EndTransition();
                }
            }
            return;
        }
        Machine.InternalTick(deltaTime);
    }



    // Tìm cha chung gần nhất của hai state a và b
    public static HierarchicalState Lca(HierarchicalState a, HierarchicalState b)
    {
        // Đường đi từ a đến gốc
        var ap = new HashSet<HierarchicalState>();
        for (var s = a; s != null; s = s.Parent)
        {
            ap.Add(s);
        }

        // Đi từ b lên gốc, tìm state đầu tiên xuất hiện trong ap
        for (var s = b; s != null; s = s.Parent)
        {
            if (ap.Contains(s))
            {
                return s;
            }
        }

        return null;
    }
}
