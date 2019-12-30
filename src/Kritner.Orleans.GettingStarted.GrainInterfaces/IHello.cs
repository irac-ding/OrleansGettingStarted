using System.Threading.Tasks;
using Orleans;

namespace Kritner.Orleans.GettingStarted.GrainInterfaces
{
    /// <summary>
    /// Orleans grain communication interface IHello
    /// </summary>
    public interface IHello :IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }
}
