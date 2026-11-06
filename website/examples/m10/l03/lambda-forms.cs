// Expression body, one parameter
Func<int, int> twice = x => x * 2;

// Two parameters need parentheses
Func<int, int, int> add = (x, y) => x + y;

// No parameters: empty parentheses
Func<string> greeting = () => "hello";

// Explicit parameter type
Func<int, bool> isPositive = (int x) => x > 0;

// Block body: needs braces and return
Func<int, string> describe = x =>
{
    string kind = x % 2 == 0 ? "even" : "odd";
    return $"{x} is {kind}";
};

// Action: no return value
Action<string> show = text => Console.WriteLine($"> {text}");

Console.WriteLine(twice(4));
Console.WriteLine(add(3, 4));
Console.WriteLine(greeting());
Console.WriteLine(isPositive(-2));
Console.WriteLine(describe(7));
show("done");
