public static IQueryable<StoredIncident> Search(
    IQueryable<StoredIncident> incidents, string? user)
{
    var query = incidents.AsNoTracking();
    if (user is not null)
    {
        query = query.Where(/* gap 1 */);
    }

    return query./* gap 2 */;
}
