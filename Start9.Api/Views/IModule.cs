using System.AddIn.Pipeline;
using System.Collections.Generic;

namespace Start9.Api
{
    [AddInBase]
    public interface IModule
    {
        void Initialize(IHost host, IConfiguration loadedConfig);

        IConfiguration Configuration { get; }
        void ShowConfigurator();

        IList<IMessageEntry> MessageContract { get; }
        IList<IReceiverEntry> ReceiverContract { get; }
    }
}