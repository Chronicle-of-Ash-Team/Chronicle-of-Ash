using System.Threading;
using System.Threading.Tasks;

public abstract class Activity : IActivity
{
    public ActivityMode Mode { get; protected set; } = ActivityMode.Inactive;

    public virtual async Task ActivateAsync(CancellationToken cancellationToken)
    {
        if (Mode != ActivityMode.Inactive) return;

        Mode = ActivityMode.Activating;

        await Task.CompletedTask;

        Mode = ActivityMode.Active;
    }

    public virtual async Task DeactivateAsync(CancellationToken cancellationToken)
    {
        if (Mode != ActivityMode.Active) return;

        Mode = ActivityMode.Deactivating;

        await Task.CompletedTask;

        Mode = ActivityMode.Inactive;
    }
}
