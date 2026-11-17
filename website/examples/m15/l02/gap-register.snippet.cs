ServiceCollection services = new();
// 1. Register: which type is created for the interface
services.___<IAlertSender, ConsoleSender>();
// 2. Build the container
using ServiceProvider provider = services.___();
// 3. Resolve: the container creates the object
IAlertSender sender = provider.GetRequiredService<IAlertSender>();
