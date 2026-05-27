

using System.Threading;
using System.Threading.Tasks;

public enum ActivityMode
{
    Inactive,
    Activating,
    Active,
    Deactivating
}

public interface IActivity
{
    ActivityMode Mode { get; }
    Task ActivateAsync(CancellationToken cancellationToken);
    Task DeactivateAsync(CancellationToken cancellationToken);
}
