using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FaceTargetActivity : Activity
{
    private readonly Transform owner;
    private readonly Transform target;

    private readonly float rotateSpeed;
    private readonly float angleThreshold;

    public FaceTargetActivity(
        Transform owner,
        Transform target,
        float rotateSpeed = 720f,
        float angleThreshold = 3f)
    {
        this.owner = owner;
        this.target = target;
        this.rotateSpeed = rotateSpeed;
        this.angleThreshold = angleThreshold;
    }

    public override async Task ActivateAsync(
        CancellationToken cancellationToken)
    {
        await base.ActivateAsync(
            cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            Vector3 dir =
                target.position -
                owner.position;

            dir.y = 0f;

            if (dir.sqrMagnitude < 0.001f)
                break;

            Quaternion targetRot =
                Quaternion.LookRotation(dir);

            owner.rotation =
                Quaternion.RotateTowards(
                    owner.rotation,
                    targetRot,
                    rotateSpeed * Time.deltaTime);

            float angle =
                Quaternion.Angle(
                    owner.rotation,
                    targetRot);

            if (angle <= angleThreshold)
                break;

            await Task.Yield();
        }

        await DeactivateAsync(
            cancellationToken);
    }
}