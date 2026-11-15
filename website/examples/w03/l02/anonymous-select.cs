List<string> users = ["ana", "ben"];

var rows = users.Select(user => new { Name = user, Length = user.Length });

foreach (var row in rows)
{
    Console.WriteLine($"{row.Name}: {row.Length}");
}
