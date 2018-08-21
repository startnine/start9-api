using System;

namespace Start9.Api
{
    public interface IMessageEntry
    {
        Type Type { get; }

        String DisplayName { get; }
    }
}