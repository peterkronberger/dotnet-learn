int x = 1;
int y = x;
y++;
Console.WriteLine(x); // 1, because y was only a copy

var p1 = new Person { Name = "Ada" };
var p2 = p1;
p2.Name = "Grace";
Console.WriteLine(p1.Name); // Grace, because p1 and p2 are the same object

class Person
{
    public string Name { get; set; } = "";
}
