using Mango.Services.RewardAPI.Messaging;

namespace Mango.Services.RewardAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer serviceBusConsumer { get; set; }
        public static IApplicationBuilder UserAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            serviceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStop()
        {
            serviceBusConsumer.Stop();
        }

        private static void OnStart()
        {
            serviceBusConsumer.Start();
        }
    }
}