class Reading
{
    public double Celsius { get; set; }
}

static class ReadingExtensions
{
    extension(Reading reading)
    {
        public string Label { get; set; }
    }
}
