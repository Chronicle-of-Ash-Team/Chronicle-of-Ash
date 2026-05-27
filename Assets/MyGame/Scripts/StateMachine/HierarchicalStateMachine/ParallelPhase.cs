using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class ParallelPhase : ISequence
{
    readonly List<PhaseStep> steps;
    readonly CancellationToken cancellationToken;

    List<Task> tasks;

    public bool IsDone { get; private set; }

    public ParallelPhase(List<PhaseStep> steps, CancellationToken cancellationToken)
    {
        this.steps = steps;
        this.cancellationToken = cancellationToken;
    }

    public void Start()
    {
        if (steps == null || steps.Count == 0)
        {
            IsDone = true;
            return;
        }

        tasks = new List<Task>(steps.Count);

        for (int i = 0; i < steps.Count; i++)
        {
            tasks.Add(steps[i](cancellationToken));
        }
    }

    public bool Update()
    {
        if (IsDone) return true;

        IsDone = tasks == null || tasks.TrueForAll(t => t.IsCompleted);

        return IsDone;
    }
}
