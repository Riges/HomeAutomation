using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HomeAutomation.Netatmo.Query.IoC;
using HomeAutomation.Netatmo.Query.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netatmo;
using NodaTime;

namespace HomeAutomation.Netatmo.Query
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            var Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);
            containerBuilder.RegisterInstance(Configuration).As<IConfiguration>();
            containerBuilder.Register(c => SystemClock.Instance).As<IClock>();
            containerBuilder.RegisterModule<QueryModule>();

            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            var app = new App(
                serviceProvider.GetService<IClock>(),
                serviceProvider.GetService<IClient>(),
                serviceProvider.GetService<IWeatherService>(),
                Configuration.GetSection("StationId").Value);

            await app.Run(Configuration.GetSection("Netatmo:Username").Value, Configuration.GetSection("Netatmo:Password").Value);
            
            container.Dispose();
        }
    }
}