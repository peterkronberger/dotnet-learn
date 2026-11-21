using System.Linq.Expressions;

var bob = new Incident { User = "bob", FailureCount = 5 };

// The same lambda text, converted in two ways
Func<Incident, bool> asDelegate = i => i.FailureCount >= 5;
Expression<Func<Incident, bool>> asTree = i => i.FailureCount >= 5;

// A delegate is code: you can only call it
Console.WriteLine($"Delegate result: {asDelegate(bob)}");

// An expression tree is data: you can read its parts
Console.WriteLine($"Tree text: {asTree}");
Console.WriteLine($"Parameter: {asTree.Parameters[0].Name}");
Console.WriteLine($"Body: {asTree.Body}");
Console.WriteLine($"Kind of body: {asTree.Body.NodeType}");

// Compile turns the data into a delegate, then you can call it
Func<Incident, bool> compiled = asTree.Compile();
Console.WriteLine($"Compiled result: {compiled(bob)}");

class Incident
{
    public string User { get; set; } = "";
    public int FailureCount { get; set; }
}
