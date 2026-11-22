// A small helper without state (transient): a new object is cheap
sealed class FailureMatcher
{
    public bool IsFailure(string line) =>
        line.Contains("login-failed", StringComparison.Ordinal);
}
