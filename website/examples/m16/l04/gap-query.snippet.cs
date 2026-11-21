var query = db.Incidents
    .Where(i => i.FailureCount >= 5)
    .___(i => i.FailureCount)
    .Select(i => i.User);
// SQL: ORDER BY "i"."FailureCount" DESC
