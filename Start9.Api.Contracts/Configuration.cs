using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Start9.Api.Contracts
{
    [Serializable] 
    public abstract class Configuration
    {
        public static Configuration Default { get; } = new ConfigurationBase();
    }

    [Serializable]
    sealed class ConfigurationBase : Configuration { }
}
