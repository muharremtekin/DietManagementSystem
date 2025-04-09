namespace DietManagementSystem.WebApi.Extensions;

public static class RetryHelper
{
    public static async Task ExecuteWithRetriesAsync(
        Func<Task> action,
        int maxRetries,
        int delayMs,
        Action<Exception, int>? onError = null)
    {
        for (int i = 1; i <= maxRetries; i++)
        {
            try
            {
                await action();
                return;
            }
            catch (Exception ex) when (i < maxRetries)
            {
                onError?.Invoke(ex, i);
                await Task.Delay(delayMs);
            }
        }
        await action();
    }
}