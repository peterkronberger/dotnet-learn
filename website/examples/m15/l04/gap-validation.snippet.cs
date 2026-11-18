ServiceProviderOptions options = new()
{
    ___ = true,
    ValidateOnBuild = true,
};
using ServiceProvider provider = services.BuildServiceProvider(options);
