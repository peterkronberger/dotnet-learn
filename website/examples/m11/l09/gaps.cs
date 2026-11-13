string[] failed = ["bob", "alice", "bob", "dave"];
string[] success = ["alice", "carol", "dave"];

var both = failed.Distinct().Intersect(success);
var onlyFailed = failed.Distinct().Except(success);

Console.WriteLine($"Failed and success: {string.Join(", ", both)}");
Console.WriteLine($"Only failed: {string.Join(", ", onlyFailed)}");
