Animal animal = new("Rex");
Animal dog = new Dog("Rex");

Console.WriteLine(animal == new Animal("Rex"));
Console.WriteLine(animal.Equals(dog));
Console.WriteLine(dog);

record Animal(string Name);

sealed record Dog(string Name) : Animal(Name);
