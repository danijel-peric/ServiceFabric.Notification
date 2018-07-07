using System;
using System.Threading.Tasks;

namespace ServiceFabric.PubSubActors.Publishers
{
    public interface IMessagePublisher
    { 
        /// <summary>
        /// Publish a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="brokerServiceName">The name of a SF Service of type <see cref="BrokerService"/>.</param>
        /// <returns></returns>
        Task PublishMessageAsync(object message, Uri brokerServiceName = null);
    }
}
