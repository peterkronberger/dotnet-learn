Access granted = Access.Read | Access.Write;
Console.WriteLine(granted);
Console.WriteLine((int)granted);
Console.WriteLine((granted & Access.Write) != Access.None);
Console.WriteLine((granted & Access.Delete) != Access.None);

[Flags]
enum Access
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
}
