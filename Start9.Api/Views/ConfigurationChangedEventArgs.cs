using System;

namespace Start9.Api
{    
    public abstract class AConfigurationChangedEventArgs : EventArgs
    {
        public abstract IConfigurationEntry Old { get; }
        public abstract IConfigurationEntry New { get; }
    }
}