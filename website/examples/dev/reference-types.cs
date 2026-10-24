var numberA = 5;
var numberB = numberA;
numberB = 7;

var listA = new List<int> { 1, 2, 3 };
var listB = listA;
listB.Add(4);

var userA = new User { Name = "Ada" };
var userB = userA;
userB.Name = "Bob";

Console.WriteLine($"{numberA} {numberB}");
Console.WriteLine($"{listA.Count} {listB.Count}");
Console.WriteLine($"{userA.Name} {userB.Name}");

class User
{
    public string Name { get; set; } = "";
}
