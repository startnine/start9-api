using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Start9.Api.Contracts
{
    [Serializable] 
    public abstract class Configuration
    {
        public static Configuration Default { get; } = new ConfigurationBase();
    }

    class ConfigurationBase : Configuration { }
}
