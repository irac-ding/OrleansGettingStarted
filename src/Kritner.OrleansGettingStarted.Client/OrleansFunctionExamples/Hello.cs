using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kritner.OrleansGettingStarted.Client.Helpers;
using Kritner.Orleans.GettingStarted.GrainInterfaces;
using Orleans;

namespace Kritner.OrleansGettingStarted.Client.OrleansFunctionExamples
{
    public class Hello : IOrleansFunction
    {
        public string Description => "Demonstrates the most basic Orleans function of 'Hello'.";

        public async Task PerformFunction(IClusterClient clusterClient)
        {
            // example of calling grains from the initialized client
            for (int i = 0; i < 1000000; i++)
            {
                var friend = clusterClient.GetGrain<IHello>(i);
                var response = await friend.SayHello("Good morning, my friend!");
                Console.WriteLine($"{response}");
            }
            ConsoleHelpers.ReturnToMenu();
        }
    }
}
