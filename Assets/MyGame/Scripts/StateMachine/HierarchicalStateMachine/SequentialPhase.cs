using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class SequentialPhase : ISequence
{
    readonly List<PhaseStep> steps;
    readonly CancellationToken cancellationToken;
    int index = -1;
    Task currentTask;

    public bool IsDone { get; private set; }

    public SequentialPhase(List<PhaseStep> steps, CancellationToken cancellationToken)
    {
        this.steps = steps;
        this.cancellationToken = cancellationToken;
    }

    public void Start() => Next();

    public bool Update()
    {
        if (IsDone) return true;
        if (currentTask != null || currentTask.IsCompleted) Next();
        return IsDone;
    }

    private void Next()
    {
        index++;
        if (index >= steps.Count)
        {
            IsDone = true;
            return;
        }
        currentTask = steps[index](cancellationToken);
    }

}