using System.Fabric;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.PubSubActors.Consumers;
using ServiceFabric.PubSubActors.Helpers;

namespace ServiceFabric.PubSubActors
{
    public static class MessageConsumerExtensions
    {
        public static IMessageConsumer CreateConsumer(this StatelessService service, ServicePartitionInformation partition)
        {
            return service.Context.CreateConsumer(partition);
        }

        public static IMessageConsumer CreateConsumer(this StatelessService service, ServicePartitionInformation partition, IBrokerServiceLocator serviceLocator)
        {
            return service.Context.CreateConsumer(partition, serviceLocator);
        }

        public static IMessageConsumer CreateConsumer(this StatefulService service, ServicePartitionInformation partition)
        {
            return service.Context.CreateConsumer(partition);
        }

        public static IMessageConsumer CreateConsumer(this StatefulService service, ServicePartitionInformation partition, IBrokerServiceLocator serviceLocator)
        {
            return service.Context.CreateConsumer(partition, serviceLocator);
        }

        public static IMessageConsumer CreateConsumer(this ServiceContext context, ServicePartitionInformation partition)
        {
            return new MessageConsumer(new ServiceMessageRegistry(context, partition));
        }

        public static IMessageConsumer CreateConsumer(this ServiceContext context, ServicePartitionInformation partition, IBrokerServiceLocator serviceLocator)
        {
            return new MessageConsumer(new ServiceMessageRegistry(context, partition, serviceLocator));
        }

        public static IMessageConsumer CreateConsumer(this ActorBase actor)
        {
            return new MessageConsumer(new ActorMessageRegistry(actor));
        }

        public static IMessageConsumer CreateConsumer(this ActorBase actor, IBrokerServiceLocator serviceLocator)
        {
            return new MessageConsumer(new ActorMessageRegistry(actor, serviceLocator));
        }
    }
}
