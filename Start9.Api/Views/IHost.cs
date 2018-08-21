using System.Collections.Generic;

namespace Start9.Api
{    
    public interface IHost
    {
        void PushMessage(IMessage message);

        IList<IConfigurationEntry> GetConfigurationEntries(IConfiguration configuration);
        IConfiguration GetGlobalConfiguration();

        IList<IModule> GetInstancesOfExecutingModule();
    }
}