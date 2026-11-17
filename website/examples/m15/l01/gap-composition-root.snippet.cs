// 1. Create the dependency
IAlertSender sender = new ConsoleSender();
// 2. Hand it to the class through the constructor
AlertService service = ___(___);
// 3. Use the class
service.Raise("bob", 5);
