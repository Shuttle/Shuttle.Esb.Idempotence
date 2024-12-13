# Idempotence

```
PM> Install-Package Shuttle.Esb.Idempotence
```

Contains an `IIdempotenceService` interface that should be implemented to provide idempotence support for the service bus.

## Configuration

```c#
services.AddIdempotence();
services.AddImplementedIdempotence();
```