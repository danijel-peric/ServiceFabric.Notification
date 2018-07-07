using System;
using System.Runtime.Serialization;

namespace ServiceFabric.Notification
{
    [DataContract]
    public class SystemClockEvent
    {
        [DataMember]
        public DateTime Clock { get; set; }

        public static SystemClockEvent Instance
        {
            get
            {
                return new SystemClockEvent() { Clock = DateTime.UtcNow };
            }
        }
    }
}
