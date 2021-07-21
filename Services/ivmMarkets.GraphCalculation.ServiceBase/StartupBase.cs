using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StackExchange.Redis;

using ivmMarkets.GraphCalculation.BuildingBlocks.EventBus;
using ivmMarkets.GraphCalculation.BuildingBlocks.EventBus.Abstractions;
using ivmMarkets.GraphCalculation.BuildingBlocks.EventBusRabbitMQ;
using ivmMarkets.GraphCalculation.BuildingBlocks.Events;
using ivmMarkets.GraphCalculation.BuildingBlocks.Repository;
using ivmMarkets.GraphCalculation.BuildingBlocks.ServiceClients;

namespace ivmMarkets.GraphCalculation.ServiceBase
{
    public class StartupBase
    {
        protected virtual bool NeedValueChangedSubscription => false;

        public StartupBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .Services
                .AddOptions();

            ConfigureSettings(services);

            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var settings = GetSettings(sp);
                var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var settings = GetSettings(sp);
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = Configuration["SubscriptionClientName"];
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            if (NeedValueChangedSubscription)                
                services.AddSingleton<ValueChangedIntegrationEventHandler>();
            
            services.AddSingleton<IParametersRepository, ParametersRepository>();
            services.AddHttpClient<IParametersService, ParametersService>();
            services.AddHttpClient<ICalculationNodeService, CalculationNodeService>();

            AddCustomServices(services);

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        protected virtual void ConfigureSettings(IServiceCollection services)
        {
            services.Configure<AppSettingsBase>(Configuration);
        }

        protected virtual AppSettingsBase GetSettings(IServiceProvider sp)
        {
            return sp.GetRequiredService<IOptions<AppSettingsBase>>().Value;
        }

        protected virtual void AddCustomServices(IServiceCollection services)
        {

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (NeedValueChangedSubscription)
                ConfigureEventBus(app);
        }

        protected void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ValueChangedIntegrationEvent, ValueChangedIntegrationEventHandler>();
        }
    }
}
