using System;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using ServiceFabric.PubSubActors.Helpers;

namespace ServiceFabric.PubSubActors.Consumers
{
    public class ActorMessageRegistry : MessageRegistry<ActorContextWrapper>
    {
        public ActorMessageRegistry(ActorBase context) : this(context, new BrokerServiceLocator())
        {
        }
        public ActorMessageRegistry(ActorBase context, IBrokerServiceLocator serviceLocator) : base(new ActorContextWrapper(context), serviceLocator)
        {
        }

        public override async Task RegisterMessageAsync(Type type, Uri brokerServiceName = null, string listenerName = null)
        {
            if (brokerServiceName == null)
            {
                brokerServiceName = await PublisherServiceHelper.DiscoverBrokerServiceNameAsync();
                if (brokerServiceName == null)
                {
                    throw new InvalidOperationException("No brokerServiceName was provided or discovered in the current application.");
                }
            }

            var brokerService = await ServiceLocator.GetBrokerServiceForMessageAsync(type.Name, brokerServiceName);
            await brokerService.RegisterSubscriberAsync(ActorReference.Get(ContextWrapper.Context), type.FullName);
        }

       
        public override async Task UnregisterMessageAsync(Type type, Uri brokerServiceName = null, bool flushQueue = false)
        {
            if (brokerServiceName == null)
            {
                brokerServiceName = await PublisherServiceHelper.DiscoverBrokerServiceNameAsync();
                if (brokerServiceName == null)
                {
                    throw new InvalidOperationException("No brokerServiceName was provided or discovered in the current application.");
                }
            }
            var brokerService = await ServiceLocator.GetBrokerServiceForMessageAsync(type.Name, brokerServiceName);
            await brokerService.UnregisterSubscriberAsync(ActorReference.Get(ContextWrapper.Context), type.FullName, flushQueue);
        }
    }
}