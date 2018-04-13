using System;
using Start9.Api;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Start9.Api.Contracts
{
    [AddInContract]
    public interface IStart9ModuleContract : IContract
    {
        void MessageReceived<TObject, TReceiver>(Message<TObject, TReceiver> message);
        Configuration Configuration { get; }
    }
}
