namespace Checks;

public class FailureCounter
{
    public async Task<int> CountAsync(
        IEnumerable<string> lines, CancellationToken token)
    {
        int count = 0;
        foreach (string line in lines)
        {
            await Task.Delay(1, token);
            if (line.Contains("login-failed"))
            {
                count++;
            }
        }
        return count;
    }
}
