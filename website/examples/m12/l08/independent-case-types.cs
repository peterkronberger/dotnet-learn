// The case types stay independent of the union
Pet pet = new Dog("Rex");

Console.WriteLine(typeof(Dog).BaseType);
Console.WriteLine(pet.Value is Dog);
Console.WriteLine(pet.Value is Pet);
Console.WriteLine(pet.Value?.GetType().Name);

record Dog(string Name);
record Cat(string Name);
union Pet(Dog, Cat);
