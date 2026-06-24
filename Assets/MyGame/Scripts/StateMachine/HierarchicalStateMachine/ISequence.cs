using System.Threading;
using System.Threading.Tasks;

public delegate Task PhaseStep(CancellationToken cancellationToken);
public interface ISequence
{
    bool IsDone { get; }
    void Start();
    bool Update();
}

