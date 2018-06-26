using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.Notification.Shared;
using ServiceFabric.PubSubActors.Helpers;

namespace ServiceFabric.Notification.Clock
{
    internal sealed class Clock : StatelessService
    {
        private readonly IPublisherServiceHelper publisherService;

        public Clock(StatelessServiceContext context)
            : base(context)
        {
            publisherService = new PublisherServiceHelper();
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await NotifySystemClock(SystemClockEvent.Instance);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        public async Task NotifySystemClock(SystemClockEvent clock)
        {
            ServiceEventSource.Current.ServiceMessage(Context, $"Publishing message: {nameof(SystemClockEvent)}");

            await publisherService.PublishMessageAsync(this, clock);
        }
    }
}
