using ELibrary_BorrowingService.Consumers;
using ELibrary_UserService.RabbitMq.Messages;
using MassTransit;
using RabbitMqMessages;

namespace ELibrary_BorrowingService.RabbitMq
{
    public static class MassTransitCollection
    {
        private const string SubscriptionEndpoint = "BorrowingService";
        public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                // add consumers
                x.AddConsumer<UserCreatedConsumer>();
                x.AddConsumer<BookAvailabilityChangedConsumer>();


                if (configuration["Flags:UserRabbitMq"] == "1")   //todo change to preprocessor directive #if
                {
                    RabbitMqOptions rabbitMqOptions = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();
                    x.UsingRabbitMq((hostContext, cfg) =>
                    {
                        cfg.Host(rabbitMqOptions.Uri, "/", c =>
                        {
                            c.Username(rabbitMqOptions.UserName);
                            c.Password(rabbitMqOptions.Password);
                        });

                        // Consumers Configuration
                        cfg.ConfigureEndpoints(hostContext);
                    });
                }
                else
                {
                    // Azure Basic Tier - only 1-1 queues
                    x.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(configuration["AzureServiceBusConnectionString"]);

                        /// Publisher configuration ///


                        /// Consumers configuration ///
                        // usercreated
                        cfg.ReceiveEndpoint("usercreated", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserCreatedConsumer>(context);

                        });

                        // bookavailabilitychanged
                        cfg.ReceiveEndpoint("bookavailabilitychanged", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<BookAvailabilityChangedConsumer>(context);

                        });
                    });
                }

            });

            return services;
        }
    }
}
