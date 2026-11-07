string[] morning = ["ana", "ben"];
string[] evening = ["cem", "dia"];

List<string> all = [..morning, ..evening];
all.Add("eva");

Console.WriteLine(all.Count);
Console.WriteLine(morning.Length);
Console.WriteLine(string.Join(",", all));
