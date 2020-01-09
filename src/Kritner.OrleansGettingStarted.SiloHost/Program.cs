using Kritner.OrleansGettingStarted.Common;
using Kritner.OrleansGettingStarted.Common.Config;
using Kritner.OrleansGettingStarted.Common.Helpers;
using Kritner.Orleans.GettingStarted.Grains;
using Kritner.OrleansGettingStarted.SiloHost.ExtensionMethods;
using Kritner.OrleansGettingStarted.SiloHost.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Statistics;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Kritner.OrleansGettingStarted.SiloHost
{
    public class Program
    {
        private static Startup Startup;
        private static IServiceProvider ServiceProvider;

        public static int Main(string[] args)
        {
            Startup = ConsoleAppConfigurator.BootstrapApp();
            var serviceCollection = new ServiceCollection();
            Startup.ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .ConfigureClustering(
                    ServiceProvider.GetService<IOptions<OrleansConfig>>(),
                    Startup.HostingEnvironment
                )
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "HelloWorldApp";
                })
                .AddMemoryGrainStorage(Constants.OrleansMemoryProvider)
                //.ConfigureApplicationParts(parts =>
                //{
                //    parts.AddApplicationPart(typeof(IGrainMarker).Assembly).WithReferences();
                //})
                .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
                .ConfigureServices(
                DependencyInjectionHelper.IocContainerRegistration
                )
#if Linux
                .UseLinuxEnvironmentStatistics()
#elif Windows
                .UsePerfCounterEnvironmentStatistics()
#endif
                .UseDashboard(options =>
                {
                    options.Username = "admin";
                    options.Password = "admin";
                    options.Host = "*";
                    options.Port = 8080;
                    options.HostSelf = true;
                    options.CounterUpdateIntervalMs = 1000;
                })
                .UseInMemoryReminderService()
                .ConfigureServices(services =>
                {
                    services.Configure<ConsoleLifetimeOptions>(options =>
                    {
                        options.SuppressStatusMessages = true;
                    });
                })
                .ConfigureLogging(logging => logging.AddConsole());

          
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Console.WriteLine("Running on Linux!");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Console.WriteLine("Running on macOS!");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Console.WriteLine("Running on Windows!");

#if Linux
        Console.WriteLine("Built on Linux 1!"); 
#elif OSX
        Console.WriteLine("Built on macOS 1!"); 
#elif Windows
            Console.WriteLine("Built in Windows 1!");
#endif
            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
