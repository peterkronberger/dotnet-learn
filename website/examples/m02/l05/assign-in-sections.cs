int level = 2;
string name;
switch (level)
{
    case 1:
        name = "low";
        break;
    case 2:
        name = "medium";
        break;
    default:
        name = "unknown";
        break;
}
Console.WriteLine(name);
