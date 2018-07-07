using System;
using System.Threading.Tasks;

namespace ServiceFabric.PubSubActors.Consumers
{
    public interface IMessageConsumer : IMessageRegister, IMessageUnregister
    {
    }

    public interface IMessageRegister
    {
        /// <summary>
        /// Registers this Actor/Service as a subscriber for messages of type <paramref> <name>T</name>  </paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        Task RegisterMessageAsync<T>(Uri brokerServiceName = null, string listenerName = null) where T : class;

        /// <summary>
        /// Registers this Actor/Service as a subscriber for messages of type <paramref> <name>T</name>  </paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        Task RegisterMessageAsync(Type type, Uri brokerServiceName = null, string listenerName = null);
    }

    public interface IMessageUnregister
    {
        /// <summary>
        /// Unregisters this Actor/Service as a subscriber for messages of type <paramref><name>T</name></paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        Task UnregisterMessageAsync<T>(bool flushQueue = false) where T : class;
        /// <summary>
        /// Unregisters this Actor/Service as a subscriber for messages of type <paramref><name>T</name></paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        Task UnregisterMessageAsync<T>(Uri brokerServiceName = null, bool flushQueue = false) where T : class;

        /// <summary>
        /// Unregisters this Actor/Service as a subscriber for messages of type <paramref><name>T</name></paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        Task UnregisterMessageAsync(Type type, bool flushQueue = false);
        /// <summary>
        /// Unregisters this Actor/Service as a subscriber for messages of type <paramref><name>T</name></paramref> with the <see cref="BrokerService"/>.
        /// </summary>
        /// <returns></returns>
        Task UnregisterMessageAsync(Type type, Uri brokerServiceName = null, bool flushQueue = false);
    }
}
