﻿using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.Idempotence.Tests;

internal class IdempotenceMessageHandlerInvoker : IMessageHandlerInvoker
{
    private readonly IdempotenceCounter _counter = new();
    private readonly Type _type = typeof(IdempotenceCommand);

    public int ProcessedCount => _counter.ProcessedCount;

    public async ValueTask<bool> InvokeAsync(IPipelineContext<OnHandleMessage> pipelineContext)
    {
        var state = Guard.AgainstNull(pipelineContext).Pipeline.State;
        
        if (Guard.AgainstNull(state.GetMessage()).GetType() != _type)
        {
            throw new("Can only handle type of 'IdempotenceCommand'.");
        }

        _counter.Processed();

        return await ValueTask.FromResult(true).ConfigureAwait(false);
    }
}