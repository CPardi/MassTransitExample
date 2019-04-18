using Automatonymous;
using MassTransitExample.Events;

namespace MassTransitExample.Saga
{
    internal class EchoStateMachine : MassTransitStateMachine<SagaInstance>
    {
        public State Saying { get; private set; }
        public State Echoing { get; private set; }
        public State Complete { get; private set; }

        public Event<TextEntered> TextEnteredEvent { get; set; }
        public Event<TextEchoStart> TextEchoStartEvent { get; set; }
        public Event<EchoEnd> TextEchoEndEvent { get; set; }

        public EchoStateMachine()
        {
            InstanceState(instance => instance.CurrentState);

            Event(() => TextEnteredEvent, cfg => cfg.CorrelateById(x => x.Message.Id).SelectId(x => x.Message.Id));
            Event(() => TextEchoStartEvent, cfg => cfg.CorrelateById(x => x.Message.Id).SelectId(x => x.Message.Id));
            Event(() => TextEchoEndEvent, cfg => cfg.CorrelateById(x => x.Message.Id));

            Initially(
                When(TextEnteredEvent).Then(AssignText).TransitionTo(Saying),
                When(TextEchoStartEvent).Then(AssignText).TransitionTo(Echoing),
                When(TextEchoEndEvent).Then(AssignText).TransitionTo(Complete));

            During(Saying, Ignore(TextEnteredEvent),
                When(TextEchoStartEvent).Then(AssignText).TransitionTo(Echoing),
                When(TextEchoEndEvent).Then(AssignText).TransitionTo(Complete));

            During(Echoing, Ignore(TextEnteredEvent),
                Ignore(TextEchoStartEvent),
                When(TextEchoEndEvent).Then(AssignText).TransitionTo(Complete));

            During(Complete, Ignore(TextEnteredEvent),
                Ignore(TextEchoStartEvent),
                Ignore(TextEchoEndEvent));
        }

        private static void AssignText(BehaviorContext<SagaInstance, IHasText> context)
        {
            context.Instance.Text = context.Data.Text;
        }
    }
}