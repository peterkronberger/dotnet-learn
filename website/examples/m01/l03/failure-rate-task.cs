Console.WriteLine(LoginStats.FailureRate(7, 20));

static class LoginStats
{
    // Returns the share of failed logins in percent (0 when there were no attempts).
    public static double FailureRate(int failed, int total)
    {
        return failed / total * 100;
    }
}
