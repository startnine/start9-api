using System;

namespace Start9.Api
{
    public interface IConfigurationEntry
    {
        Object Object { get; }

        Type Type { get; }
    }
}