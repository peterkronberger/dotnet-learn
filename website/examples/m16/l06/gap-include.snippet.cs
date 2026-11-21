var incidents = db.Incidents
    .___(i => i.Events)
    .ToList();
// SQL: LEFT JOIN "Events" AS "e" ON "i"."Id" = "e"."StoredIncidentId"
