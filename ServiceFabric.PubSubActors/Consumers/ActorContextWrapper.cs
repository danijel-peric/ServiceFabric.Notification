using Microsoft.ServiceFabric.Actors.Runtime;
using ServiceFabric.PubSubActors.Interfaces;

namespace ServiceFabric.PubSubActors.Consumers
{
    public class ActorContextWrapper : IContextWrapper
    {
        public ActorBase Context { get; }

        public ActorContextWrapper(ActorBase context)
        {
            Context = context;
        }
    }
}