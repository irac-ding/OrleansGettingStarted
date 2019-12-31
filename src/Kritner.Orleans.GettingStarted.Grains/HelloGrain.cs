using System;
using System.Threading.Tasks;
using Kritner.Orleans.GettingStarted.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Kritner.Orleans.GettingStarted.Grains
{
    /// <summary>
    /// Orleans grain implementation class HelloGrain.
    /// </summary>
    public class HelloGrain : Grain, IHello
    {
        private readonly ILogger logger;

        public HelloGrain(ILogger<HelloGrain> logger)
        {
            this.logger = logger;
        }  

        Task<string> IHello.SayHello(string greeting)
        {
            logger.LogInformation($"SayHello message received: greeting = '{greeting}'");
            Console.WriteLine($"You said: '{greeting}', I say: Hello!");

            return Task.FromResult($"You said: '{greeting}', I say: Hello!");
        }
    }
}
