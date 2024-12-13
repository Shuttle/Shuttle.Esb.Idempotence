namespace Shuttle.Esb.Idempotence;

public interface IIdempotenceService
{
    Task RegisterAsync(TransportMessage transportMessage);
    Task HandledAsync(TransportMessage transportMessage);
    ValueTask<bool> ContainsAsync(TransportMessage transportMessage);
}