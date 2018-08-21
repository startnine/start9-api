using System;

namespace Start9.Api
{
    public interface IConfiguration
    {
        event EventHandler<AConfigurationChangedEventArgs> ConfigurationChanged;
    }
}