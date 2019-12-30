using Kritner.OrleansGettingStarted.Common.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;

namespace Kritner.OrleansGettingStarted.SiloHost.ExtensionMethods
{
    public static class ISiloHostBuilderExtensions
    {
        /// <summary>
        /// Configures clustering for the Orleans Silo Host based on
        /// the Orleans environment.
        /// </summary>
        /// <param name="builder">The silo host builder.</param>
        /// <param name="orleansConfigOptions">The Orleans configuration options.</param>
        /// <param name="environmentName">The environment.</param>
        public static ISiloHostBuilder ConfigureClustering(
            this ISiloHostBuilder builder,
            IOptions<OrleansConfig> orleansConfigOptions,
            IHostEnvironment hostEnvironment
        )
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (orleansConfigOptions.Value == default(OrleansConfig))
            {
                throw new ArgumentException(nameof(orleansConfigOptions));
            }

            builder.UseLocalhostClustering();
            builder.Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback);
            if (hostEnvironment.IsDevelopment())
            {
                var orleansConfig = orleansConfigOptions.Value;
                // Configure the first listed node as the "primary node".
                // Note this type of configuration should probably not be used in prod - using HA clustering instead.
                builder.UseDevelopmentClustering(
                    IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0])
                );
                builder.ConfigureEndpoints(
                    siloPort: orleansConfig.SiloPort,
                    gatewayPort: orleansConfig.GatewayPort
                );
            }
            else if (hostEnvironment.IsStaging())
            {
                var orleansConfig = orleansConfigOptions.Value;
                // Configure the first listed node as the "primary node".
                // Note this type of configuration should probably not be used in prod - using HA clustering instead.
                builder.UseDevelopmentClustering(
                    IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0])
                );
                builder.ConfigureEndpoints(
                    siloPort: orleansConfig.SiloPort,
                    gatewayPort: orleansConfig.GatewayPort
                );
            }
           else if (hostEnvironment.IsProduction())
            {
                var orleansConfig = orleansConfigOptions.Value;
                // Configure the first listed node as the "primary node".
                // Note this type of configuration should probably not be used in prod - using HA clustering instead.
                builder.UseDevelopmentClustering(
                   IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0])
                );
                builder.ConfigureEndpoints(
                    IPAddress.Parse(orleansConfig.NodeIpAddresses[1]),
                    siloPort: orleansConfig.SiloPort,
                    gatewayPort: orleansConfig.GatewayPort
                );
            }

            return builder;
        }
    }
}
