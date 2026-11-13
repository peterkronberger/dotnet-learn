string[] users = ["dave", "alice", "bob", "amy", "carol"];

var queryResult =
    from user in users
    where user.Length > 3
    orderby user
    select user.ToUpper();

var methodResult = users
    .Where(user => user.Length > 3)
    .OrderBy(user => user)
    .Select(user => user.ToUpper());

Console.WriteLine($"Query syntax:  {string.Join(", ", queryResult)}");
Console.WriteLine($"Method syntax: {string.Join(", ", methodResult)}");
