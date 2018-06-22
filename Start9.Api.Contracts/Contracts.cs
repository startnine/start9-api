using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections;

namespace Start9.Api.Contracts
{
    [AddInContract]
    public interface IModuleContract : IContract
    {
        IMessageContract SendMessage(IMessageContract message);
        IConfigurationContract Configuration { get; }
        void HostReceived(IHostContract host);
    }

    public interface IMessageContract : IContract
    {
        String Text { get; }
        Object Object { get; }
    }

    public interface IConfigurationContract : IContract
    {
        IDictionary Entries { get; }
    }

    public interface IHostContract : IContract
    {
        void SendGlobalMessage(IMessageContract message);
        IListContract<IModuleContract> GetModules();
        IConfigurationContract GetConfiguration(IModuleContract module);
    }
}
    