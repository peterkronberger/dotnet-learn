using SecLog.Model;

// Failure counts of the canonical sample data
(string User, int Failures)[] counts =
[
    ("bob", 5), ("alice", 1), ("dave", 1), ("carol", 0),
];
foreach ((string user, int failures) in counts)
{
    Console.WriteLine($"{user}: {SeverityRating.For(failures)}");
}

namespace SecLog.Model
{
    /// <summary>How serious a number of failures is.</summary>
    public enum Severity
    {
        None,
        Low,
        Medium,
        High,
    }

    /// <summary>Rates how serious a number of failed logins is.</summary>
    public static class SeverityRating
    {
        /// <summary>The severity for a number of failed logins.</summary>
        /// <exception cref="ArgumentOutOfRangeException">The number is negative.</exception>
        public static Severity For(int failureCount)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(failureCount);

            return failureCount switch
            {
                0 => Severity.None,
                < 3 => Severity.Low,
                < 5 => Severity.Medium,
                _ => Severity.High,
            };
        }
    }
}
