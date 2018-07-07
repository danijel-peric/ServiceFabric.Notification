using System;

namespace ServiceFabric.Notification
{
    public class GlobalHost
    {
        public static GlobalHost Instance { get; }

        static GlobalHost()
        {
            Instance = new GlobalHost();
        }

        public void Set(IServiceProvider provider)
        {
            if(provider != null)
                ServiceProvider = provider;
        }

        public IServiceProvider ServiceProvider { get; private set; }
    }
}
