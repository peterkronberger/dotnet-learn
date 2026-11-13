string[] words = ["tea", "coffee", "milk"];

var query = from word in words let size = word.Length where size > 3 select size;
Console.WriteLine(string.Join(", ", query));
