using System;
using System.Threading.Tasks;
using ServiceFabric.PubSubActors.Interfaces;

namespace ServiceFabric.PubSubActors.Consumers
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly IMessageRegistry<IContextWrapper> _messageRegistry;

        public MessageConsumer(IMessageRegistry<IContextWrapper> registry)
        {
            _messageRegistry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        /// <summary>
        /// Registers this Actor/Service as a subscriber for messages of type <paramref> <name>T</name>  </paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        public Task RegisterMessageAsync<T>(Uri brokerServiceName = null, string listenerName = null) where T : class
        {
            return _messageRegistry.RegisterMessageAsync<T>(brokerServiceName, listenerName);
        }

        /// <summary>
        /// Registers this Actor/Service as a subscriber for messages of type <paramref> <name>T</name>  </paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        public Task RegisterMessageAsync(Type type, Uri brokerServiceName = null, string listenerName = null)
        {
            return _messageRegistry.RegisterMessageAsync(type, brokerServiceName, listenerName);
        }

        /// <summary>
        /// Registers this Actor/Service as a subscriber for messages of type <paramref> <name>T</name>  </paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        public Task UnregisterMessageAsync<T>(bool flushQueue = false) where T : class
        {
            return _messageRegistry.UnregisterMessageAsync<T>(flushQueue);
        }

        /// <summary>
        /// Unregisters this Actor/Service as a subscriber for messages of type <paramref><name>T</name></paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        public Task UnregisterMessageAsync<T>(Uri brokerServiceName = null, bool flushQueue = false) where T : class
        {
            return _messageRegistry.UnregisterMessageAsync<T>(brokerServiceName, flushQueue);
        }

        /// <summary>
        /// Unregisters this Actor/Service as a subscriber for messages of type <paramref><name>T</name></paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        public Task UnregisterMessageAsync(Type type, bool flushQueue = false)
        {
            return _messageRegistry.UnregisterMessageAsync(type, flushQueue);
        }

        /// <summary>
        /// Unregisters this Actor/Service as a subscriber for messages of type <paramref><name>T</name></paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        public Task UnregisterMessageAsync(Type type, Uri brokerServiceName = null, bool flushQueue = false)
        {
            return _messageRegistry.UnregisterMessageAsync(type, brokerServiceName, flushQueue);
        }
    }
}

