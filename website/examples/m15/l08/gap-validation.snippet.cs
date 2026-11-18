builder.Services
    .AddOptions<DetectionOptions>()
    .Bind(builder.Configuration.GetSection("Detection"))
    .ValidateDataAnnotations()
    .___();
