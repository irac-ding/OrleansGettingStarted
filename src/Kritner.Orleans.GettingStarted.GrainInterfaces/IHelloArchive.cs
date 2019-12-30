using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Kritner.Orleans.GettingStarted.GrainInterfaces
{
    /// <summary>
    /// Orleans grain communication interface that will save all greetings
    /// </summary>
    public interface IHelloArchive : IGrainWithIntegerKey, IGrainInterfaceMarker
    {
        Task<string> SayHello(string greeting);

        Task<IEnumerable<string>> GetGreetings();
    }
}
