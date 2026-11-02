string user = "bob";
int failed = 6;
// Step 1: add a first section for "bob" with 5+ failures: "lock bob"
switch (user)
{
    case "bob":
        Console.WriteLine($"watch bob ({failed} failures)");
        break;
    default:
        Console.WriteLine("ignore");
        break;
}
