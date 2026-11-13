string[] words = ["apple", "banana", "avocado", "blueberry", "cherry"];

var query =
    from word in words
    group word by word[0] into letterGroup
    orderby letterGroup.Key
    select new { Letter = letterGroup.Key, Count = letterGroup.Count() };

var methods = words
    .GroupBy(word => word[0])
    .OrderBy(letterGroup => letterGroup.Key)
    .Select(letterGroup => new
    {
        Letter = letterGroup.Key,
        Count = letterGroup.Count()
    });

Console.WriteLine($"Same rows: {query.SequenceEqual(methods)}");
