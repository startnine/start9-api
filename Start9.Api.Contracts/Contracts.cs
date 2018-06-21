using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections;

namespace Start9.Api.Contracts
{
    [AddInContract]
    public interface IModuleContract : IContract
    {
        Message SendMessage<T>(Message message);
        IConfiguration Configuration { get; }
    }

    public class Message
    {
        public String Text;
    }

    public class Message<T>
    {
        public T Object { get; }
    }

    public interface IConfiguration
    {
        Dictionary<String, Object> GetConfigurationStrings();
    }

    public interface ISkin : IConfiguration
    {
        IDictionary Resources { get; }
    }
}
