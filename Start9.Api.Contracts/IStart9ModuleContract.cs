using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Start9.Api.Contracts
{
    [AddInContract]
    public interface IModuleContract : IContract
    {
        Message MessageRecieved<T>(Message<T> message);
        Configuration Configuration { get; }
        Skin Skin { get; set; }
    }
}