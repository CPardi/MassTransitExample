using System;

namespace MassTransitExample.Events
{
    public class TextEchoStart : IHasText
    {
        public Guid Id { get; }
        public string Text { get; }

        public TextEchoStart(Guid id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}