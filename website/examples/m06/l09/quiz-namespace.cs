using Tools.Counting;

Counter counter = new Counter();
counter.Add();
counter.Add();
Console.WriteLine(typeof(Counter).FullName);
Console.WriteLine(counter.Value);

namespace Tools.Counting
{
    class Counter
    {
        public int Value { get; private set; }

        public void Add()
        {
            Value++;
        }
    }
}
