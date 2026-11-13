string[] words = ["pear", "fig", "banana", "kiwi", "apple", "plum"];

var query =
    from word in words
    let length = word.Length
    where length == 4
    orderby word descending
    select $"{word} ({length})";

foreach (string line in query)
{
    Console.WriteLine(line);
}
