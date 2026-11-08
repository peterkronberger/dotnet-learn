interface IStore<T>
{
}

class LogEvent
{
}

class EventStore : IStore<LogEvent>
{
}

class Store<T> : IStore<T>
{
}
