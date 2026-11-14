// Before C# 15: an abstract base record and a discard arm
Pet first = new Dog("Rex");
Pet second = new Cat("Tom");

Console.WriteLine(Describe(first));
Console.WriteLine(Describe(second));

string Describe(Pet pet) => pet switch
{
    Dog dog => $"dog {dog.Name}",
    Cat cat => $"cat {cat.Name}",
    _ => "unknown pet",
};

abstract record Pet;
record Dog(string Name) : Pet;
record Cat(string Name) : Pet;
