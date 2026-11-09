A x = new B();
Console.WriteLine(x.Name());

class A
{
    public virtual string Name() => "A";
}

class B : A
{
    public new string Name() => "B";
}
