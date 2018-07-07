using System;
using System.Threading.Tasks;
using ServiceFabric.PubSubActors.Helpers;
using ServiceFabric.PubSubActors.Interfaces;

namespace ServiceFabric.PubSubActors.Publishers
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IBrokerServiceLocator _brokerServiceLocator;

        public MessagePublisher():this(new BrokerServiceLocator())
        {
        }

        public MessagePublisher(IBrokerServiceLocator brokerServiceLocator)
        {
            _brokerServiceLocator = brokerServiceLocator ?? new BrokerServiceLocator();
        }

        /// <summary>
        /// Publish a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="brokerServiceName">The name of a SF Service of type <see cref="BrokerService"/>.</param>
        /// <returns></returns>
        public async Task PublishMessageAsync(object message, Uri brokerServiceName = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (brokerServiceName == null)
            {
                brokerServiceName = await _brokerServiceLocator.LocateAsync();
                if (brokerServiceName == null)
                {
                    throw new InvalidOperationException("No brokerServiceName was provided or discovered in the current application.");
                }
            }

            var brokerService = await _brokerServiceLocator.GetBrokerServiceForMessageAsync(message, brokerServiceName);
            var wrapper = MessageWrapper.CreateMessageWrapper(message);
            await brokerService.PublishMessageAsync(wrapper);
        }
    }
}
