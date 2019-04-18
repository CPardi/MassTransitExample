using System;
using System.Threading;
using Automatonymous;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.Saga;
using MassTransitExample.Events;
using MassTransitExample.Saga;
using Microsoft.EntityFrameworkCore;

namespace MassTransitExample
{
    public static class Program
    {
        public static void Main()
        {
            MassTransitStateMachine<SagaInstance> stateMachine = new EchoStateMachine();
            var sagaRepository = CreateSagaRepository();

            var bus = CreateBusWithOutbox(stateMachine, sagaRepository);

            bus.StartAsync();

            for (int i = 0; i < 100; ++i)
            {
                bus.Publish(new TextEntered(Guid.NewGuid(), i.ToString()));
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }

            Console.ReadLine();
            bus.Stop();
        }

        private static ISagaRepository<SagaInstance> CreateSagaRepository()
        {
            var contextFactory = new ContextFactory();

            using (var context = contextFactory.CreateDbContext(Array.Empty<string>()))
            {
                context.Database.EnsureCreated();
            }

            DbContext SagaDbContextFactory() => contextFactory.CreateDbContext(Array.Empty<string>());
            return new EntityFrameworkSagaRepository<SagaInstance>(SagaDbContextFactory);
        }

        private static IBusControl CreateBus(SagaStateMachine<SagaInstance> stateMachine, ISagaRepository<SagaInstance> sagaRepository)
        {
            return Bus.Factory.CreateUsingInMemory(sbc =>
            {
                sbc.ReceiveEndpoint("saga_queue", ep => ep.StateMachineSaga(stateMachine, sagaRepository));
                sbc.ReceiveEndpoint("entered_queue", ep => ep.Consumer<TextEnteredConsumer>());
                sbc.ReceiveEndpoint("echo_queue", ep => ep.Consumer<EchoEndConsumer>());
            });
        }

        private static IBusControl CreateBusWithOutbox(SagaStateMachine<SagaInstance> stateMachine, ISagaRepository<SagaInstance> sagaRepository)
        {
            return Bus.Factory.CreateUsingInMemory(sbc =>
            {
                sbc.ReceiveEndpoint("saga_queue", ep =>
                {
                    ep.UseInMemoryOutbox();
                    ep.StateMachineSaga(stateMachine, sagaRepository);
                });

                sbc.ReceiveEndpoint("entered_queue", ep => ep.Consumer<TextEnteredConsumer>());
                sbc.ReceiveEndpoint("echo_queue", ep => ep.Consumer<EchoEndConsumer>());
            });
        }
    }
}