using Kritner.OrleansGettingStarted.Common.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Clustering.Redis;
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
            var orleansConfig = orleansConfigOptions.Value;
            if (hostEnvironment.IsDevelopment())
            {
                builder.UseRedisMembership(opt =>
                 {
                     opt.ConnectionString = orleansConfig.RedisCluster;
                     opt.Database = 0;
                 });
            }
            if (hostEnvironment.IsStaging())
            {
                builder.UseRedisMembership(opt =>
                {
                    opt.ConnectionString = orleansConfig.RedisCluster;
                    opt.Database = 0;
                });
            }
            if (hostEnvironment.IsProduction())
            {
                builder.UseRedisMembership(opt =>
                {
                    opt.ConnectionString = orleansConfig.RedisCluster;
                    opt.Database = 0;
                });
            }

            return builder;
        }
    }
}
