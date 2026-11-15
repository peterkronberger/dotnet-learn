List<int> scores = [10, 20, 30];
IEnumerable<int> high = scores.Where(score => score >= 20);
List<int> saved = high.ToList();

scores.Add(40);
Console.WriteLine(high.Count());
Console.WriteLine(saved.Count);
