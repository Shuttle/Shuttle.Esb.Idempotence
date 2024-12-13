using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.Idempotence;

public class HandleMessageObserver : IHandleMessageObserver
{
    private readonly IIdempotenceService _idempotenceService;
    private readonly Esb.HandleMessageObserver _handleMessageObserver;

    public HandleMessageObserver(IOptions<ServiceBusOptions> serviceBusOptions, IMessageHandlerInvoker messageHandlerInvoker, ISerializer serializer, IIdempotenceService idempotenceService)
    {
        _handleMessageObserver = new(serviceBusOptions, messageHandlerInvoker, serializer);
        _idempotenceService = Guard.AgainstNull(idempotenceService);
    }

#pragma warning disable CS0067
    public event EventHandler<MessageNotHandledEventArgs>? MessageNotHandled;
    public event EventHandler<HandlerExceptionEventArgs>? HandlerException;
#pragma warning restore CS0067

    public async Task ExecuteAsync(IPipelineContext<OnHandleMessage> pipelineContext)
    {
        var state = Guard.AgainstNull(pipelineContext).Pipeline.State;
        var transportMessage = Guard.AgainstNull(state.GetTransportMessage());

        if (await _idempotenceService.ContainsAsync(transportMessage))
        {
            return;
        }

        await _idempotenceService.RegisterAsync(transportMessage);
        await _handleMessageObserver.ExecuteAsync(pipelineContext);
        await _idempotenceService.HandledAsync(transportMessage);
    }
}