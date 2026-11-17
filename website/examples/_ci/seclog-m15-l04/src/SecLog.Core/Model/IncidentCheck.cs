namespace SecLog.Model;

/// <summary>A method that decides whether an incident matches.</summary>
/// <param name="incident">The incident to check.</param>
/// <returns><c>true</c> if the incident matches the check.</returns>
public delegate bool IncidentCheck(Incident incident);
