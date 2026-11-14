// The default value of a union holds no case value
// The compiler warns about the second switch (the lesson shows this)
#pragma warning disable CS8655
Pet none = default;
Console.WriteLine(none.Value is null);

string withNullArm = none switch
{
    Dog dog => dog.Name,
    Cat cat => cat.Name,
    null => "no pet",
};
Console.WriteLine(withNullArm);

try
{
    string withoutNullArm = none switch
    {
        Dog dog => dog.Name,
        Cat cat => cat.Name,
    };
    Console.WriteLine(withoutNullArm);
}
catch (Exception ex)
{
    Console.WriteLine(ex.GetType().Name);
}

record Dog(string Name);
record Cat(string Name);
union Pet(Dog, Cat);
