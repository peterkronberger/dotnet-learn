string[] words = ["apple", "banana", "avocado", "blueberry", "cherry"];

var query =
    from word in words
    group word by word[0] into letterGroup
    orderby letterGroup.Key
    select new { Letter = letterGroup.Key, Count = letterGroup.Count() };

foreach (var row in query)
{
    Console.WriteLine($"{row.Letter}: {row.Count}");
}
