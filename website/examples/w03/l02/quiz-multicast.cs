Func<int> score = () => 10;
score += () => 20;
score += () => 30;

Console.WriteLine(score());
