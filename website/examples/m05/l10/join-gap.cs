string[] parts = "user=bob".Split('=');
string[] names = [parts[0], "dave"];

Console.WriteLine(string.Join(",", names));
