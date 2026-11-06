Box label = new("login-failed");
Console.WriteLine(label.Value);
// Task 2: create a Box<int> with the value 7 and print its value

// Task 1: make Box generic, so that Value has the type of the type argument
class Box
{
    public string Value { get; }

    public Box(string value)
    {
        Value = value;
    }
}
