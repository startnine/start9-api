using System;
using System.Threading.Tasks;

namespace Start9.Api.Contracts
{
    [Serializable]
    public class Message
    {
        public String Text { get; }
    }

    [Serializable]
    public class Message<T> : Message
    {
        public T Object { get; }
    }
}