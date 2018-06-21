using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections;

namespace Start9.Api
{
    [AddInContract]
    public interface IModuleContract : IContract
    {
        IMessageContract SendMessage(IMessageContract message);
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
}
