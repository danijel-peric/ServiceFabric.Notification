using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ServiceFabric.Notification.Hubs
{
    public class NotificationsHub : Hub
    {
        public Task HeartBeatTock()
        {
            return Task.CompletedTask;
        }
    }
}
