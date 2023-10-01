using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController2 : MonoBehaviour,IPlayer
{

    private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        cancellationTokenSource = new CancellationTokenSource();
    }

    public async void SetTargetPosition(Vector3 targetPosition)
    {
        StopTasks();
        var cancellationToken = cancellationTokenSource.Token;

        try
        {
            await LookAtAsync(targetPosition, 1f, cancellationToken);

            // Check if the operation was canceled before proceeding
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await MoveAsync(targetPosition, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            // Handle cancellation if needed
            Debug.Log("Task was canceled.");
        }
    }

    public async Task LookAtAsync(Vector3 targetPosition, float rotationSpeed, CancellationToken cancellationToken)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Ignore the Y-component for rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float elapsedTime = 0f;

        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, elapsedTime);

            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested(); // Re-throw the cancellation exception
            }

            await Task.Yield();
        }

        // Ensure the final rotation exactly faces the target
        transform.rotation = targetRotation;
    }

    private async Task MoveAsync(Vector3 targetPosition, CancellationToken cancellationToken)
    {
        // Move the character to the target
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 1);

            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested(); // Re-throw the cancellation exception
            }

            await Task.Yield();
        }
    }

    public void StopTasks()
    {
        if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose(); // Dispose of the CancellationTokenSource
            cancellationTokenSource = new CancellationTokenSource(); // Create a new CancellationTokenSource
        }
    }
}


