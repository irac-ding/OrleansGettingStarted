using Kritner.Orleans.GettingStarted.GrainInterfaces;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kritner.Orleans.GettingStarted.Grains
{
    public class HelloArchiveGrain : Grain, IHelloArchive
    {
        private readonly IPersistentState<GreetingArchive> _archive;

        public HelloArchiveGrain([PersistentState("archive", "OrleansMemoryProvider")] IPersistentState<GreetingArchive> archive)
        {
            this._archive = archive;
        }

        public async Task<string> SayHello(string greeting)
        {
            this._archive.State.Greetings.Add(greeting);

            await this._archive.WriteStateAsync();
            Console.WriteLine($"You said: '{greeting}', I say: Hello!");
            return $"You said: '{greeting}', I say: Hello!";
        }

        public Task<IEnumerable<string>> GetGreetings() => Task.FromResult<IEnumerable<string>>(this._archive.State.Greetings);
    }

    public class GreetingArchive
    {
        public List<string> Greetings { get; } = new List<string>();
    }
}
