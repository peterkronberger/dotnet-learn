Basket basket = new Basket();
basket.Items.Add("a");
basket.Items.Add("b");
basket.Items.RemoveAt(0);
Console.WriteLine(basket.Items.Count);
Console.WriteLine(basket.Items[0]);

class Basket
{
    public readonly List<string> Items = [];
}
