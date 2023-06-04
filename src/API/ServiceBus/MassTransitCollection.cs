using ELibrary_BorrowingService.Consumers;
using ELibrary_BorrowingService.ServiceBus;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_BorrowingService.RabbitMq
{
    public static class MassTransitCollection
    {
        private const string SubscriptionEndpoint = "BorrowingService";
        public static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMessagePublisher, MessagePublisher>();

            services.AddMassTransit(x =>
            {
                // add consumers
                x.AddConsumer<BookAvailabilityChangedConsumer>();
                x.AddConsumer<BookAvailabilityChangedBrConsumer>();

                x.AddConsumer<BookCreatedConsumer>();
                x.AddConsumer<BookCreatedBrConsumer>();

                x.AddConsumer<BookRemovedConsumer>();
                x.AddConsumer<BookRemovedBrConsumer>();

                x.AddConsumer<UserBlockedConsumer>();
                x.AddConsumer<UserBlockedBrConsumer>();

                x.AddConsumer<UserCreatedConsumer>();
                x.AddConsumer<UserCreatedBrConsumer>();

                x.AddConsumer<UserDeletedConsumer>();
                x.AddConsumer<UserDeletedBrConsumer>();

                x.AddConsumer<UserUnblockedConsumer>();
                x.AddConsumer<UserUnblockedBrConsumer>();


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
                        // bookavailabilitychanged
                        EndpointConvention.Map<BookAvailabilityChangedBk>(new Uri($"queue:{nameof(BookAvailabilityChangedBk)}"));
                        cfg.Message<BookAvailabilityChangedBk>(cfgTopology => cfgTopology.SetEntityName(nameof(BookAvailabilityChangedBk)));
                        
                        // overtimereturn
                        EndpointConvention.Map<OvertimeReturnU>(new Uri($"queue:{nameof(OvertimeReturnU)}"));
                        cfg.Message<OvertimeReturnU>(cfgTopology => cfgTopology.SetEntityName(nameof(OvertimeReturnU)));

                        /// Consumers configuration ///
                        cfg.ReceiveEndpoint("bookavailabilitychangedbr", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<BookAvailabilityChangedBrConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("bookcreatedbr", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<BookCreatedBrConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("bookremovedbr", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<BookRemovedBrConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("userblockedbr", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserBlockedBrConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("usercreatedbr", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserCreatedBrConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("userdeletedbr", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserDeletedBrConsumer>(context);

                        });
                        cfg.ReceiveEndpoint("userunblockedbr", e =>
                        {
                            e.ConfigureConsumeTopology = false;     // configuration for ASB Basic Tier - queues only
                            e.PublishFaults = false;
                            e.ConfigureConsumer<UserBlockedBrConsumer>(context);

                        });

                    });
                }

            });

            return services;
        }
    }
}
