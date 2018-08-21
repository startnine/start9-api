using System;

namespace Start9.Api
{
    public interface IReceiverEntry
    {
        Type Type { get; }
        String DisplayName { get; }

        void SendMessage(IMessage message);

    }
}