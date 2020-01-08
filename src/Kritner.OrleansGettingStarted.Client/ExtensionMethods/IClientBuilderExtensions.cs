using Kritner.OrleansGettingStarted.Common.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Providers.MongoDB.Configuration;
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
               builder.UseMongoDBClient(orleansConfig.MongoCluster)
               .UseMongoDBClustering(options =>
               {
                   options.DatabaseName = "OrleansGettingStartedMongo";
                   options.Strategy = MongoDBMembershipStrategy.SingleDocument;
               });
            }
            if (hostEnvironment.IsStaging())
            {
              builder.UseMongoDBClient(orleansConfig.MongoCluster)
              .UseMongoDBClustering(options =>
              {
                  options.DatabaseName = "OrleansGettingStartedMongo";
                  options.Strategy = MongoDBMembershipStrategy.SingleDocument;
              });
            }
            if (hostEnvironment.IsProduction())
            {
              builder.UseMongoDBClient(orleansConfig.MongoCluster)
              .UseMongoDBClustering(options =>
              {
                  options.DatabaseName = "OrleansGettingStartedMongo";
                  options.Strategy = MongoDBMembershipStrategy.SingleDocument;
              });
            }

            return builder;
        }
    }
}
