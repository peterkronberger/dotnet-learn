// Pair 1: where and select
var query1 = from n in numbers where n > 2 select n * 10;
var methods1 = numbers.Where(n => n > 2).Select(n => n * 10);

// Pair 2: orderby with descending
var query2 = from n in numbers orderby n descending select n;
var methods2 = numbers.OrderByDescending(n => n);

// Pair 3: group by with a continuation (into)
var query3 =
    from word in words
    group word by word[0] into letterGroup
    select letterGroup.Key;
var methods3 = words
    .GroupBy(word => word[0])
    .Select(letterGroup => letterGroup.Key);
