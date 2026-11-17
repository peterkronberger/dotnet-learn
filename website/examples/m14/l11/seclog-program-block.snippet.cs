// Check if the incident of bob is still active: the clock is a parameter
DateTimeOffset lastSeen = bobIncident.LastFailure!.Value;
TimeSpan activeWindow = TimeSpan.FromMinutes(10);
IncidentActivity soon = new(
    new FixedTimeProvider(lastSeen.AddMinutes(3)), activeWindow);
IncidentActivity later = new(
    new FixedTimeProvider(lastSeen.AddMinutes(11)), activeWindow);
Console.WriteLine($"Active after 3 minutes: {soon.IsActive(bobIncident)}");
Console.WriteLine($"Active after 11 minutes: {later.IsActive(bobIncident)}");
