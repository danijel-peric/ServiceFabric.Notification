using System.Fabric;
using ServiceFabric.PubSubActors.Interfaces;

namespace ServiceFabric.PubSubActors.Consumers
{
    public class ServiceContextWrapper : IContextWrapper
    {
        public ServiceContext Context { get; }

        public ServiceContextWrapper(ServiceContext context)
        {
            Context = context;
        }
    }
}