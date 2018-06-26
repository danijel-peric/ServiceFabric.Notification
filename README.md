# ServiceFabric.Notification
Usage example of ServiceFabric Pub/Sub using ServiceFabric.PubSubActors https://github.com/loekd/ServiceFabric.PubSubActors

Just simple demonstration of using https://github.com/loekd/ServiceFabric.PubSubActors

2 Stateless Publisher
1 Broker
1 Stateless Subscriber (ASP.NET core with singalR 2) to display realtime clock which is published by ServiceFabric.Notification.Clock and
random messages published by ServiceFabric.Notification.Random every 200ms

not that if you publish ServiceFabric.Notification.Subscriber more then one instance, you need to use SignalR Scaleout with redist, defined in /PackageRoot/Config/Settings.xml

note that ServiceFabric.PubSubActors and ServiceFabric.PubSubActors.Interfaces are converted to net standard, because of Stateless services which are built in net core
and broker service is using v2 remoting

url of subscriber service on local dev machine is http://localhost:8349/

![alt text](https://github.com/danijel-peric/ServiceFabric.Notification/blob/master/index.jpg)
