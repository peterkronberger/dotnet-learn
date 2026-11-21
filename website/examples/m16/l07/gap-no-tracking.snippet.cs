// A report only reads: the context does not need to track the rows
var rows = db.Incidents
    .___()
    .OrderBy(i => i.Id)
    .ToList();
