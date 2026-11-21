using SecLog.Model;

namespace SecLog.Tests.Doubles;

// Stub: answers every incident with a fixed value
public sealed class StubRule : IDetectionRule
{
    private readonly bool result;

    public StubRule(bool result)
    {
        this.result = result;
    }

    public string Name => "stub";

    public bool IsMatch(Incident incident)
    {
        return result;
    }
}
