using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.Idempotence.Tests;

[TestFixture]
public class HandleMessageObserverFixture
{
    [Test]
    public async Task Should_be_able_to_ignore_messages_that_have_already_been_handled_async()
    {
        var messageHandlerInvoker = new Mock<IMessageHandlerInvoker>();
        var serializer = new Mock<ISerializer>();
        var idempotenceService = new Mock<IIdempotenceService>();

        idempotenceService.Setup(m => m.ContainsAsync(It.IsAny<TransportMessage>())).Returns(ValueTask.FromResult(true));

        var observer = new HandleMessageObserver(Options.Create(new ServiceBusOptions()), messageHandlerInvoker.Object, serializer.Object, idempotenceService.Object);

        var pipeline = new Pipeline(new Mock<IServiceProvider>().Object)
            .AddObserver(observer);

        pipeline
            .AddStage(".")
            .WithEvent<OnHandleMessage>();

        var transportMessage = new TransportMessage();

        pipeline.State.SetTransportMessage(transportMessage);

        await pipeline.ExecuteAsync();

        idempotenceService.Verify(m => m.ContainsAsync(transportMessage), Times.Once);

        messageHandlerInvoker.VerifyNoOtherCalls();
        serializer.VerifyNoOtherCalls();
        idempotenceService.VerifyNoOtherCalls();
    }
    
    [Test]
    public async Task Should_be_able_to_handle_message_async()
    {
        var messageHandlerInvoker = new Mock<IMessageHandlerInvoker>();
        var serializer = new Mock<ISerializer>();
        var idempotenceService = new Mock<IIdempotenceService>();

        idempotenceService.Setup(m => m.ContainsAsync(It.IsAny<TransportMessage>())).Returns(ValueTask.FromResult(true));

        var observer = new HandleMessageObserver(Options.Create(new ServiceBusOptions()), messageHandlerInvoker.Object, serializer.Object, idempotenceService.Object);

        var pipeline = new Pipeline(new Mock<IServiceProvider>().Object)
            .AddObserver(observer);

        pipeline
            .AddStage(".")
            .WithEvent<OnHandleMessage>();

        var transportMessage = new TransportMessage();

        pipeline.State.SetTransportMessage(transportMessage);

        await pipeline.ExecuteAsync();

        idempotenceService.Verify(m => m.ContainsAsync(transportMessage), Times.Once);

        messageHandlerInvoker.VerifyNoOtherCalls();
        serializer.VerifyNoOtherCalls();
        idempotenceService.VerifyNoOtherCalls();
    }
}