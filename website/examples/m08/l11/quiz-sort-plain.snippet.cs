List<Plain> items = new()
{
    new Plain { Name = "b", Number = 2 },
    new Plain { Name = "a", Number = 1 },
};
items.Sort();

class Plain
{
    public string Name { get; set; } = "";
    public int Number { get; set; }
}
