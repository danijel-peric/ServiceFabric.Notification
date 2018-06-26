using System;
using System.Fabric;
using ServiceFabric.PubSubActors;

namespace ServiceFabric.Notification.Broker
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Broker : BrokerServiceUnordered
    {
        public Broker(StatefulServiceContext context)
            : base(context)
        {
            ServiceEventSourceMessageCallback = (message) => ServiceEventSource.Current.ServiceMessage(context, message);

            Period = TimeSpan.FromMilliseconds(1);
            DueTime = TimeSpan.FromSeconds(1);

            MaxDequeuesInOneIteration = 1000;
        }
    }
}
