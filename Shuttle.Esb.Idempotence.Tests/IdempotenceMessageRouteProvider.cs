namespace Shuttle.Esb.Idempotence.Tests;

public class IdempotenceMessageRouteProvider : IMessageRouteProvider
{
    public async Task AddAsync(IMessageRoute messageRoute)
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }

    public async Task<IEnumerable<string>> GetRouteUrisAsync(string messageType)
    {
        return await Task.FromResult(GetRouteUris(messageType)).ConfigureAwait(false);
    }

    public IEnumerable<string> GetRouteUris(string messageType)
    {
        return new List<string> { "transient-queue://./idempotence-inbox-work" };
    }

    public IEnumerable<IMessageRoute> MessageRoutes => [];
}