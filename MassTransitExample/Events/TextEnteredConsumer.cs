using System.Threading.Tasks;
using MassTransit;

namespace MassTransitExample.Events
{
    public class TextEnteredConsumer : IConsumer<TextEntered>
    {
        public async Task Consume(ConsumeContext<TextEntered> context)
        {
            var id = context.Message.Id;
            var messageText = context.Message.Text;
            await context.Publish(new TextEchoStart(id, messageText));
//            Console.Write($"Entered: {messageText}");
            await context.Publish(new EchoEnd(id, messageText));
        }
    }
}