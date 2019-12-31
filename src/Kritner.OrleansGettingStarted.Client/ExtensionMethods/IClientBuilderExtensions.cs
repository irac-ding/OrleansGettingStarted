using Kritner.OrleansGettingStarted.Common.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Kritner.OrleansGettingStarted.Client.ExtensionMethods
{
    public static class IClientBuilderExtensions
    {
        /// <summary>
        /// Configures clustering for the Orleans Client based on
        /// the Orleans environment.
        /// </summary>
        /// <param name="builder">The client builder.</param>
        /// <param name="orleansConfigOptions">The Orleans configuration options.</param>
        /// <param name="environmentName">The environment.</param>
        public static IClientBuilder ConfigureClustering(
            this IClientBuilder builder, 
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

           // builder.UseLocalhostClustering();
            if (hostEnvironment.IsDevelopment())
            {
                var orleansConfig = orleansConfigOptions.Value;
                List<IPEndPoint> nodes = new List<IPEndPoint>();
                foreach (var node in orleansConfig.NodeIpAddresses)
                {
                    nodes.Add(IPEndPoint.Parse(node));
                }
                builder.UseStaticClustering(nodes.ToArray());
            }
            if (hostEnvironment.IsStaging())
            {
                var orleansConfig = orleansConfigOptions.Value;
                List<IPEndPoint> nodes = new List<IPEndPoint>();
                foreach (var node in orleansConfig.NodeIpAddresses)
                {
                    nodes.Add(IPEndPoint.Parse(node));
                }
                builder.UseStaticClustering(nodes.ToArray());
            }
            if (hostEnvironment.IsProduction())
            {
                var orleansConfig = orleansConfigOptions.Value;
                List<IPEndPoint> nodes = new List<IPEndPoint>();
                foreach (var node in orleansConfig.NodeIpAddresses)
                {
                    nodes.Add(IPEndPoint.Parse(node));
                }
                builder.UseStaticClustering(nodes.ToArray());
            }

            return builder;
        }
    }
}
