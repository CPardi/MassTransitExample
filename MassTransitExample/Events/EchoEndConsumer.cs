using System.Threading.Tasks;
using MassTransit;

namespace MassTransitExample.Events
{
    public class EchoEndConsumer : IConsumer<EchoEnd>
    {
        public Task Consume(ConsumeContext<EchoEnd> context)
        {
            return Task.Run(() =>
            {
                var messageText = context.Message.Text;
//                Console.WriteLine($" -> Echo: {messageText}");
            });
        }
    }
}