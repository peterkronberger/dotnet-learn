Access granted = Access.Read | Access.Delete;
Console.WriteLine((int)granted);
Console.WriteLine((granted & Access.Delete) != Access.None);
Console.WriteLine((granted & Access.Write) != Access.None);

enum Access
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
}
