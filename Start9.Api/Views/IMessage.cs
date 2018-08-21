using System;

namespace Start9.Api
{
    public interface IMessage
    {
        Object Object { get; }

        IMessageEntry MessageEntry { get; }
    }
}