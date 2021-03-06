﻿using Kritner.OrleansGettingStarted.Common.Config;
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
            if (hostEnvironment.IsDevelopment())
            {
                builder.Configure<EndpointOptions>(options =>
                {
                    //这里的IP决定了是本机 还是内网 还是公网
                    options.AdvertisedIPAddress = IPAddress.Parse(orleansConfig.AdvertisedIPAddress);
                    //监听的端口
                    options.SiloPort = orleansConfig.SiloPort;
                    //监听的网关端口
                    options.GatewayPort = orleansConfig.GatewayPort;
                    //监听的silo 远程连接点
                    options.GatewayListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.GatewayPort);
                    //监听的silo 远程端口连接点
                    options.SiloListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.SiloPort);
                })       //监听的主silo 远程连接点 为空则创建一个主silo连接点
             .UseConsulClustering(gatewayOptions =>
             {
                 gatewayOptions.Address = new Uri(orleansConfig.ConsulCluster);
             });
            }
            else if (hostEnvironment.IsStaging())
            {
                builder.Configure<EndpointOptions>(options =>
                {
                    //这里的IP决定了是本机 还是内网 还是公网
                    options.AdvertisedIPAddress = IPAddress.Parse(orleansConfig.AdvertisedIPAddress);
                    //监听的端口
                    options.SiloPort = orleansConfig.SiloPort;
                    //监听的网关端口
                    options.GatewayPort = orleansConfig.GatewayPort;
                    //监听的silo 远程连接点
                    options.GatewayListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.GatewayPort);
                    //监听的silo 远程端口连接点
                    options.SiloListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.SiloPort);
                })       //监听的主silo 远程连接点 为空则创建一个主silo连接点
             .UseConsulClustering(gatewayOptions =>
             {
                 gatewayOptions.Address = new Uri(orleansConfig.ConsulCluster);
             });
            }
            else if (hostEnvironment.IsProduction())
            {
                builder.Configure<EndpointOptions>(options =>
                {
                    //这里的IP决定了是本机 还是内网 还是公网
                    options.AdvertisedIPAddress = IPAddress.Parse(orleansConfig.AdvertisedIPAddress);
                    //监听的端口
                    options.SiloPort = orleansConfig.SiloPort;
                    //监听的网关端口
                    options.GatewayPort = orleansConfig.GatewayPort;
                    //监听的silo 远程连接点
                    options.GatewayListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.GatewayPort);
                    //监听的silo 远程端口连接点
                    options.SiloListeningEndpoint = new IPEndPoint(options.AdvertisedIPAddress, options.SiloPort);
                })       //监听的主silo 远程连接点 为空则创建一个主silo连接点
            .UseConsulClustering(gatewayOptions =>
            {
                gatewayOptions.Address = new Uri(orleansConfig.ConsulCluster);
            });
            }
            return builder;
        }
    }
}
