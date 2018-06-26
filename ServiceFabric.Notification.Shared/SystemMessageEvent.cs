using System;
using System.Runtime.Serialization;

namespace ServiceFabric.Notification.Shared
{
    [DataContract]
    public class SystemMessageEvent
    {
        [DataMember]
        public string Node { get; set; }
        [DataMember]
        public string Service { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
    }
}
