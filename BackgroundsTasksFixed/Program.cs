using BackgroundsTasksFixed;
using Lucene.Net.Support;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddLogging();
                    var configuration = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                        .Build();

                    var appSettings = new BackgroundsTasksFixed.AppSettings();
                    configuration.GetSection("AppSettings").Bind(appSettings);

                    var limit = appSettings.MaxInstances;
                    // Add 5 instances of the Worker
                    for (int i = 1; i <= limit; i++)
                    {
                        // Create a local variable inside the loop to capture the correct value
                        int instanceNumber = i;

                        // Use the AddScoped method to ensure each instance gets a unique instanceNumber
                        services.AddSingleton<IHostedService>(provider =>
                        {
                            var logger = provider.GetRequiredService<ILogger<Worker>>();
                            return new Worker(logger, instanceNumber);
                        });
                    }
                })
            .Build();

        await host.RunAsync();
    }
}
