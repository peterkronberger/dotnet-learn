namespace SecLog.Model;

/// <summary>Extension members that ask an incident a question.</summary>
static class IncidentExtensions
{
    extension(Incident incident)
    {
        /// <summary>An incident is severe from 5 failed logins.</summary>
        public bool IsSevere => incident.FailureCount >= 5;
    }

    extension(Incident)
    {
        /// <summary>The header line of an incident table.</summary>
        public static string TableHeader => "Number | User | Failures | Severe";
    }
}
