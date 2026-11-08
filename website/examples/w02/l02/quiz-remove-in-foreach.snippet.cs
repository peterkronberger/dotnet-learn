List<string> all = ["ana", "ben", "cem"];

foreach (string name in all)
{
    if (name == "ben")
    {
        all.Remove(name);
    }
}
