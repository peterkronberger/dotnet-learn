string Describe(Pet pet) => pet switch
{
    Dog dog => $"dog {dog.Name}",
};

record Dog(string Name);
record Cat(string Name);
union Pet(Dog, Cat);
