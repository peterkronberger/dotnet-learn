IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .___()
    .AddCommandLine(args)
    .Build();
