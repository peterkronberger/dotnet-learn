Light light = new Green();
Console.WriteLine(Next(light));

Light Next(Light current) => current switch
{
    Red => new Green(),
    Green => new Red(),
};

closed record Light;
sealed record Red : Light;
sealed record Green : Light;
