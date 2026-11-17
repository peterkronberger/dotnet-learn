/// <summary>Counts the failed logins, one short wait per line.</summary>
/// <param name="token">Cancels the count.</param>
/// <returns>The number of lines with a failed login.</returns>
/// <exception cref="OperationCanceledException">The token was canceled.</exception>
public async Task<int> CountFailuresAsync(CancellationToken token)
{
    int count = 0;
    foreach (string line in _lines)
    {
        await Task.Delay(1, token);
        if (line.Contains("login-failed"))
        {
            count++;
        }
    }
    return count;
}
