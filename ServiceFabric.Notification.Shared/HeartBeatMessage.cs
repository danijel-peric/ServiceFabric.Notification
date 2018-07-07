using System;

namespace ServiceFabric.Notification
{
    public class HeartBeatMessage
    {
        public HeartBeatMessage()
        {
            ServerTime = DateTime.UtcNow;
        }

        public DateTime ServerTime { get; set; }

        public static HeartBeatMessage Instance => new HeartBeatMessage();
    }
}