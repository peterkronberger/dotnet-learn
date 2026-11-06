// Two or more parameters named _ are discards: their values are ignored
Func<string, string, int, string> label = (_, _, count) => $"{count} items";

// A callback that must accept two arguments but needs neither
Action<object?, string> ignoreFirst = (_, text) => Console.WriteLine(text);

Console.WriteLine(label("a", "b", 3));
ignoreFirst(null, "second argument");
