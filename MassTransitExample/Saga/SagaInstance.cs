using System;
using Automatonymous;

namespace MassTransitExample.Saga
{
    public class SagaInstance : SagaStateMachineInstance
    {
        public SagaInstance(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public string CurrentState { get; set; }
        public string Text { get; set; }
        public Guid CorrelationId { get; set; }
    }
}