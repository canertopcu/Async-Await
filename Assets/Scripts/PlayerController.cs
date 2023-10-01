using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour,IPlayer
{
    private CancellationTokenSource cancellationTokenSource;
     
    public  float movementSpeed = 5.0f;

    private void Awake()
    {
        cancellationTokenSource = new CancellationTokenSource();
    }

    private async Task RotateAndMoveAsync(Vector3 targetPosition, float rotationSpeed, CancellationToken cancellationToken)
    {
        // Define a minimum distance to maintain between characters
        float minDistance = 0.5f;

        // Calculate the direction to the target position
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

        float rotationDuration = 0f; 

        while (rotationDuration < 1.0f)
        {
            rotationDuration += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationDuration);

            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.Yield(); 
        }

        transform.rotation = targetRotation; // Ensure final rotation

        while (Vector3.Distance(transform.position, targetPosition) > minDistance)
        {
            // Calculate the next position based on the movement direction
            Vector3 nextPosition = transform.position + moveDirection * movementSpeed * Time.deltaTime;

             
            // Update the character's position
            transform.position = nextPosition;

            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            await Task.Yield(); 
        }
    }

    public async void SetTargetPosition(Vector3 targetPosition)
    {
        StopTasks();
        var cancellationToken = cancellationTokenSource.Token;

        try
        {
            await RotateAndMoveAsync(targetPosition, rotationSpeed: 5f, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            // Handle cancellation if needed
        }
    }

    public void StopTasks()
    {
        if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
        }
    }
}