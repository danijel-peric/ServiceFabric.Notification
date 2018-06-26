using System.Fabric;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ServiceFabric.Notification.Subscriber.Hubs;
using StackExchange.Redis;

namespace ServiceFabric.Notification.Subscriber
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var context = services.BuildServiceProvider(true).GetRequiredService<StatelessServiceContext>();

            var redisConnection = new SignalRScaleoutConfiguration(context);

            services.AddCors();
        
            if (redisConnection.UseScaleout)
                services.AddSignalR().AddRedis(options => options.ConnectionFactory = writer => ConnectionFactory(redisConnection.RedisConnectionString, writer));
            else
                services.AddSignalR();
        }

        private async Task<IConnectionMultiplexer> ConnectionFactory(string connection, TextWriter writer)
        {
            var conn = await ConnectionMultiplexer.ConnectAsync(connection, writer);

            return conn;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(policy => {

                policy.AllowAnyHeader();

                policy.AllowAnyMethod();

                policy.AllowAnyOrigin();

            });

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSignalR(route =>
            {
                route.MapHub<NotificationsHub>("/notifications");
            });

            GlobalHost.Instance.Set(app.ApplicationServices);
        }
    }
}
