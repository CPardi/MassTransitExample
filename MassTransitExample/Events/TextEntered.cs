using System;

namespace MassTransitExample.Events
{
    public class TextEntered : IHasText
    {
        public Guid Id { get; }

        public string Text { get; }

        public TextEntered(Guid id, string text)
        {
            Text = text;
            Id = id;
        }
    }
}