using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.Notification.Hubs;
using ServiceFabric.PubSubActors;
using ServiceFabric.PubSubActors.Consumers;
using ServiceFabric.PubSubActors.Helpers;
using ServiceFabric.PubSubActors.Interfaces;
using ServiceFabric.PubSubActors.SubscriberServices;

namespace ServiceFabric.Notification
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class SignalRHost : StatelessService, ISubscribingSignalRService
    {
        private IMessageConsumer messageConsumer;
        private readonly Dictionary<string, Type> messageTypes;

        public SignalRHost(StatelessServiceContext context)
            : base(context)
        {
            messageTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                {typeof(SystemClockEvent).FullName, typeof(SystemClockEvent)},
                {typeof(SystemMessageEvent).FullName, typeof(SystemMessageEvent)}
            };

        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            var listeners = new List<ServiceInstanceListener>();

            listeners.AddRange(this.CreateServiceRemotingInstanceListeners());

            listeners.Add(new ServiceInstanceListener(serviceContext => new KestrelCommunicationListener(serviceContext, "KestrelServiceEndpoint", (url, listener) =>
            {
                ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                return new WebHostBuilder().UseKestrel()
                    .ConfigureServices(services => { services.AddSingleton(serviceContext); })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                    .UseUrls(url)
                    .Build();
            }), "SignalRKestrelCommunicationListener"));

            return listeners;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Process(HeartBeatMessage.Instance);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        protected override async Task OnOpenAsync(CancellationToken cancellationToken)
        {
            messageConsumer = this.CreateConsumer(Partition.PartitionInfo);

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var messageType in messageTypes.Values)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    ServiceEventSource.Current.ServiceMessage(Context, "Service registration for message {0}", messageType.FullName);

                    await TryRegisterAsync(messageType, cancellationToken);
                }
                catch (Exception e)
                {
                    ServiceEventSource.Current.ServiceMessage(Context, "Error {0} while registering message type {1}", e.Message, messageType.FullName);
                }
            }
        }

        protected override async Task OnCloseAsync(CancellationToken cancellationToken)
        {
            await UnregisterAsync();
        }

        protected override void OnAbort()
        {
            AsyncHelper.RunSync(UnregisterAsync);
        }

        private async Task TryRegisterAsync(Type messageType, CancellationToken cancellationToken)
        {
            int retries = 0;
            const int maxRetries = 10;
            Thread.Yield();
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    ServiceEventSource.Current.ServiceMessage(this.Context, "Sending Register message {0} to broker service {1}",  messageType, ServiceNaming.BrokerServiceAddress);

                    await messageConsumer.RegisterMessageAsync(messageType);

                    ServiceEventSource.Current.ServiceMessage(Context, $"Registered Service:'{nameof(SignalRHost)}' Instance:'{Context.InstanceId}' as Subscriber.");
                    break;
                }
                catch (Exception ex)
                {
                    if (retries++ < maxRetries)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(6), cancellationToken);
                        continue;
                    }
                    ServiceEventSource.Current.ServiceMessage(Context, $"Failed to register Service:'{nameof(SignalRHost)}' Instance:'{Context.InstanceId}' as Subscriber. Error:'{ex}'");
                    break;
                }
            }
        }

        public async Task ReceiveMessageAsync(MessageWrapper message)
        {
            ServiceEventSource.Current.ServiceMessage(Context, "Received message of type: " + message?.MessageType);

            if (messageTypes.TryGetValue(message.MessageType, out Type type))
            {
                await Process(this.Deserialize(message, type));
            }
            else
            {
                ServiceEventSource.Current.ServiceMessage(Context, $"Failed to find message type {message.MessageType} in subscribed messages");
                return;
            }

            await Task.CompletedTask;
        }

        public async Task Process(object payload)
        {
            switch (payload)
            {
                case SystemClockEvent clockItem:
                    await GlobalHost.Instance.ServiceProvider.GetRequiredService<IHubContext<NotificationsHub>>().Clients.All.SendAsync("notifySystemClock", clockItem);
                    break;
                case SystemMessageEvent messageItem:
                    await GlobalHost.Instance.ServiceProvider.GetRequiredService<IHubContext<NotificationsHub>>().Clients.All.SendAsync("notifySystemMessage", messageItem);
                    break;
                case HeartBeatMessage heartBeatItem:
                    await GlobalHost.Instance.ServiceProvider.GetRequiredService<IHubContext<NotificationsHub>>().Clients.All.SendAsync("Heartbeat", heartBeatItem);
                    break;
                default:
                    return;
            }
        }

        public async Task RegisterAsync()
        {
            foreach (var messageType in messageTypes.Values)
            {
                ServiceEventSource.Current.ServiceMessage(this.Context, "Sending Register message {0} to broker service {1}", messageType, ServiceNaming.BrokerServiceAddress);

                await messageConsumer.RegisterMessageAsync(messageType);
            }
        }

        public async Task UnregisterAsync()
        {
            foreach (var messageType in messageTypes.Values)
            {
                await messageConsumer.UnregisterMessageAsync(messageType, true);
            }
        }
    }
}

