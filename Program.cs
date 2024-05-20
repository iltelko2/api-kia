using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IConfigurationRoot Configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton(s =>
        {
            var connStr = Configuration["CosmosDBConnection"];
            if (string.IsNullOrEmpty(connStr))
            {
                throw new InvalidOperationException("check cosmosdb string");
            }

            var client = new CosmosClientBuilder(connStr).Build();

            return client;
        });
    })
    .Build();

host.Run();
