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
            var orleansConfig = orleansConfigOptions.Value;
            //builder.UseLocalhostClustering();
            //builder.Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback);
            //builder.UseDevelopmentClustering(
            //        IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0])
            //    );
            //builder.Configure<EndpointOptions>(options =>
            //{
            //    // Port to use for Silo-to-Silo
            //    options.SiloPort = orleansConfig.SiloPort;
            //    // Port to use for the gateway
            //    options.GatewayPort = orleansConfig.GatewayPort;
            //    // IP Address to advertise in the cluster
            //    options.AdvertisedIPAddress = IPEndPoint.Parse(orleansConfig.NodeIpAddresses[1]).Address;
            //    // The socket used for silo-to-silo will bind to this endpoint
            //    options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 40000);
            //    // The socket used by the gateway will bind to this endpoint
            //    options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 50000);

            //})
            //.UseLocalhostClustering();
            //.UseDevelopmentClustering(IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0]));

            builder.Configure<EndpointOptions>(options =>
            {
                //这里的IP决定了是本机 还是内网 还是公网
                options.AdvertisedIPAddress = IPEndPoint.Parse(orleansConfig.NodeIpAddresses[1]).Address;
                //监听的端口
                options.SiloPort = orleansConfig.SiloPort;
                //监听的网关端口
                options.GatewayPort = orleansConfig.GatewayPort;
                //监听的silo 远程连接点
                options.GatewayListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.GatewayPort);
                //监听的silo 远程端口连接点
                options.SiloListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.SiloPort);
            })       //监听的主silo 远程连接点 为空则创建一个主silo连接点
            //.UseDevelopmentClustering(IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0]));
            .UseConsulClustering(gatewayOptions =>
             {
                 gatewayOptions.Address = new Uri("http://10.12.20.120:8500/");
             });
                 //if (hostEnvironment.IsDevelopment())
                 //{
                 //    // Configure the first listed node as the "primary node".
                 //    // Note this type of configuration should probably not be used in prod - using HA clustering instead.
                 //    builder.UseDevelopmentClustering(
                 //        IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0])
                 //    );
                 //    builder.ConfigureEndpoints(
                 //        IPEndPoint.Parse(orleansConfig.NodeIpAddresses[1]).Address,
                 //        siloPort: orleansConfig.SiloPort,
                 //        gatewayPort: orleansConfig.GatewayPort
                 //    );
                 //}
                 //else if (hostEnvironment.IsStaging())
                 //{
                 //    // Configure the first listed node as the "primary node".
                 //    // Note this type of configuration should probably not be used in prod - using HA clustering instead.
                 //    builder.UseDevelopmentClustering(
                 //        IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0])
                 //    );
                 //    builder.ConfigureEndpoints(
                 //       IPEndPoint.Parse(orleansConfig.NodeIpAddresses[1]).Address,
                 //       siloPort: orleansConfig.SiloPort,
                 //       gatewayPort: orleansConfig.GatewayPort
                 //   );
                 //}
                 //else if (hostEnvironment.IsProduction())
                 //{
                 //    // Configure the first listed node as the "primary node".
                 //    // Note this type of configuration should probably not be used in prod - using HA clustering instead.
                 //    builder.UseDevelopmentClustering(
                 //       IPEndPoint.Parse(orleansConfig.NodeIpAddresses[0])
                 //    );
                 //    builder.ConfigureEndpoints(
                 //        IPAddress.Parse(orleansConfig.NodeIpAddresses[1]),
                 //        siloPort: orleansConfig.SiloPort,
                 //        gatewayPort: orleansConfig.GatewayPort
                 //    );
                 //}
                 return builder;
        }
    }
}
