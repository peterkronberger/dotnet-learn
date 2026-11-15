List<string> users = ["ana", "ben"];
List<object> items = users;
IEnumerable<object> view = users;
Console.WriteLine(view.Count());
