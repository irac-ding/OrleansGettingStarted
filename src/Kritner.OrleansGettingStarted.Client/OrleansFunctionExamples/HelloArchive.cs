using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kritner.OrleansGettingStarted.Client.Helpers;
using Kritner.Orleans.GettingStarted.GrainInterfaces;
using Orleans;
using Orleans.Runtime;

namespace Kritner.OrleansGettingStarted.Client.OrleansFunctionExamples
{
    public class HelloArchive : IOrleansFunction
    {
        public string Description => "Demonstrates the most basic Orleans function of 'HelloArchive'.";

        public async Task PerformFunction(IClusterClient clusterClient)
        {
            // example of calling grains from the initialized client
            for (int i = 0; i < 1000000; i++)
            {
                // example of calling IHelloArchive grqain that implements persistence
                var g = clusterClient.GetGrain<IHelloArchive>(0);
                var response = await g.SayHello("Good day!");
                Console.WriteLine($"{response}");

                response = await g.SayHello("Good evening!");
                Console.WriteLine($"{response}");

                var greetings = await g.GetGreetings();
                Console.WriteLine($"\nArchived greetings: {Utils.EnumerableToString(greetings)}");
            }
            ConsoleHelpers.ReturnToMenu();
        }
    }
}
