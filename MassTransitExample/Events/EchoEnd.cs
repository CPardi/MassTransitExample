using System;

namespace MassTransitExample.Events
{
    public class EchoEnd : IHasText
    {
        public Guid Id { get; }
        public string Text { get; }

        public EchoEnd(Guid id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}