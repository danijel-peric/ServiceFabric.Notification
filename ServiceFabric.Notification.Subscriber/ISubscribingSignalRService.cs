using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using ServiceFabric.PubSubActors.SubscriberServices;

[assembly: FabricTransportServiceRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]

namespace ServiceFabric.Notification.Subscriber
{
    public interface ISubscribingSignalRService : ISubscriberService
    {
        Task RegisterAsync();

        Task UnregisterAsync();
    }
}
