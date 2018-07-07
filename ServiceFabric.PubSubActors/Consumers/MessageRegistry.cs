using System;
using System.Threading.Tasks;
using ServiceFabric.PubSubActors.Helpers;
using ServiceFabric.PubSubActors.Interfaces;

namespace ServiceFabric.PubSubActors.Consumers
{
    public abstract class MessageRegistry<TContext> : IMessageRegistry<TContext> where TContext : IContextWrapper
    {
        protected MessageRegistry(TContext context, IBrokerServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
            ContextWrapper = context;
        }

        public IBrokerServiceLocator ServiceLocator { get; }
        public TContext ContextWrapper { get; }

        public abstract Task RegisterMessageAsync(Type type, Uri brokerServiceName = null, string listenerName = null);
        public abstract Task UnregisterMessageAsync(Type type, Uri brokerServiceName = null, bool flushQueue = false);

        public Task RegisterMessageAsync<T>(Uri brokerServiceName = null, string listenerName = null) where T : class
        {
            return RegisterMessageAsync(typeof(T), brokerServiceName, listenerName);
        }
        public Task UnregisterMessageAsync<T>(bool flushQueue = false) where T : class
        {
            return UnregisterMessageAsync<T>(null, flushQueue);
        }
        public Task UnregisterMessageAsync<T>(Uri brokerServiceName = null, bool flushQueue = false) where T : class
        {
            return UnregisterMessageAsync(typeof(T), brokerServiceName, flushQueue);
        }
        public Task UnregisterMessageAsync(Type type, bool flushQueue = false)
        {
            return UnregisterMessageAsync(type, null, flushQueue);
        }

    }

    public interface IMessageRegistry<out TContext> : IMessageRegister, IMessageUnregister where TContext : IContextWrapper
    {
        IBrokerServiceLocator ServiceLocator { get; }
        TContext ContextWrapper { get; }
    }
}
