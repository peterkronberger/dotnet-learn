List<string> hosts = ["web-1", "db-1"];

// 1. Define the query: nothing runs yet
IEnumerable<string> webHosts = hosts.Where(h => h.StartsWith("web"));

// 2. Change the source after the definition
hosts.Add("web-2");

// 3. Run the query now and keep a copy
List<string> copy = webHosts.ToList();

// 4. Change the source again and compare
hosts.Add("web-3");
Console.WriteLine($"Query: {webHosts.Count()}");
Console.WriteLine($"Copy: {copy.Count}");
