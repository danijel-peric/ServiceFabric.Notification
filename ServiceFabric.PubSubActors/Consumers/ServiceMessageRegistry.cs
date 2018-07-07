using System;
using System.Fabric;
using System.Threading.Tasks;
using ServiceFabric.PubSubActors.Helpers;
using ServiceFabric.PubSubActors.Interfaces;

namespace ServiceFabric.PubSubActors.Consumers
{
    public class ServiceMessageRegistry : MessageRegistry<ServiceContextWrapper>
    {
        private readonly ServicePartitionInformation partition;

        public ServiceMessageRegistry(ServiceContext context, ServicePartitionInformation partition) : this(context, partition, new BrokerServiceLocator())
        {
        }
        public ServiceMessageRegistry(ServiceContext context, ServicePartitionInformation partition, IBrokerServiceLocator serviceLocator) : base(new ServiceContextWrapper(context), serviceLocator)
        {
            this.partition = partition;
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
            var serviceReference = CreateServiceReference(ContextWrapper.Context, partition, listenerName);
            await brokerService.RegisterServiceSubscriberAsync(serviceReference, type.FullName);
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
            var serviceReference = CreateServiceReference(ContextWrapper.Context, partition);
            await brokerService.UnregisterServiceSubscriberAsync(serviceReference, type.FullName, flushQueue);
        }

        private ServiceReference CreateServiceReference(ServiceContext context, ServicePartitionInformation info, string listenerName = null)
        {
            var serviceReference = new ServiceReference
            {
                ApplicationName = context.CodePackageActivationContext.ApplicationName,
                PartitionKind = info.Kind,
                ServiceUri = context.ServiceName,
                PartitionGuid = context.PartitionId,
                ListenerName = listenerName
            };

            if (info is Int64RangePartitionInformation longInfo)
            {
                serviceReference.PartitionKey = longInfo.LowKey;
            }
            else if (info is NamedPartitionInformation stringInfo)
            {
                serviceReference.PartitionName = stringInfo.Name;
            }

            return serviceReference;
        }
    }
}