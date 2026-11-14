// A union holds one value of one of its case types
Pet first = new Dog("Rex");
Pet second = new Cat("Tom");

Console.WriteLine(Describe(first));
Console.WriteLine(Describe(second));
Console.WriteLine(first.Value);

// No discard arm: the switch covers both case types
string Describe(Pet pet) => pet switch
{
    Dog dog => $"dog {dog.Name}",
    Cat cat => $"cat {cat.Name}",
};

record Dog(string Name);
record Cat(string Name);
union Pet(Dog, Cat);
