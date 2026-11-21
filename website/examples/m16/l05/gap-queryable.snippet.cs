___<StoredIncident> incidents = db.Incidents;
var severe = incidents
    .Where(i => i.FailureCount >= 5)
    .ToList();
// SQL: WHERE "i"."FailureCount" >= 5
