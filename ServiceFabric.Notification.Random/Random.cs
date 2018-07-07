using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.PubSubActors.Helpers;
using ServiceFabric.PubSubActors.Publishers;
using ServiceFabric.RandomGenerator;

namespace ServiceFabric.Notification
{
    internal sealed class Random : StatelessService
    {
        private readonly IRandomGenerator randomGenerator;
        private readonly IMessagePublisher messagePublisher;

        public Random(StatelessServiceContext context)
            : base(context)
        {
            randomGenerator = new RandomGenerator.RandomGenerator(s => ServiceEventSource.Current.ServiceMessage(Context, s));

            messagePublisher = new MessagePublisher();
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            Stopwatch watch = Stopwatch.StartNew();

            ServiceEventSource.Current.ServiceMessage(Context, "Generating 1000 random messages");

            await randomGenerator.Fill(1000);

            ServiceEventSource.Current.ServiceMessage(Context, "Generating random messages completed in " + watch.Elapsed);

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await PublishRandom();

                await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
            }
        }

        private async Task PublishRandom()
        {
            try
            {
                string randomMessage = await randomGenerator.Generate();

                SystemMessageEvent m = new SystemMessageEvent
                {
                    Created = DateTime.UtcNow,
                    Node = Context.NodeContext.NodeName,
                    Service = Context.ServiceName.ToString().Split("/").Last(),
                    Message = randomMessage
                };

                ServiceEventSource.Current.ServiceMessage(Context, $"Publishing message: {nameof(SystemMessageEvent)}");

                await messagePublisher.PublishMessageAsync(m);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceMessage(Context, e.Message);
            }
        }
    }
}
