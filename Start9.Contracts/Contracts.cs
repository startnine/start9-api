using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Start9.Api.Contracts
{
    #region Modules

    [AddInContract]
    public interface IModuleContract : IContract
    {
        IConfigurationContract Configuration { get; }
        IListContract<IMessageEntryContract> MessageContract { get; }
        IListContract<IReceiverEntryContract> ReceiverContract { get; }
        void ShowConfigurator();
        void Initialize(IHostContract host, IConfigurationContract loadedConfig);
    }

    #endregion

    #region Messages and Receivers

    public interface IMessageEntryContract : IContract
    {
        Type Type { get; }
        String DisplayName { get; }
    }

    public interface IMessageContract : IContract
    {
        Object Object { get; }
        IMessageEntryContract MessageEntry { get; }
    }

    public interface IReceiverEntryContract : IContract
    {
        Type Type { get; }
        String DisplayName { get; }
        void SendMessage(IMessageContract message);
    }

    #endregion

    #region Configuration

    public interface IConfigurationContract : IContract
    {
        void ConfigurationChangedEventAdd(IConfigurationChangedEventHandlerContract handler);
        void ConfigurationChangedEventRemove(IConfigurationChangedEventHandlerContract handler);
    }

    public interface IConfigurationChangedEventHandlerContract : IContract
    {
        void Handler(IConfigurationChangedEventArgsContract args);
    }

    public interface IConfigurationChangedEventArgsContract : IContract
    {
        IConfigurationEntryContract Old { get; }
        IConfigurationEntryContract New { get; }
    }

    public interface IConfigurationEntryContract : IContract
    {
        Object Object { get; }
        Type Type { get; }
    }

    #endregion

    public interface IHostContract : IContract
    {
        void PushMessage(IMessageContract message);
        IListContract<IConfigurationEntryContract> GetConfigurationEntries(IConfigurationContract configuration);
        IConfigurationContract GetGlobalConfiguration();
        IListContract<IModuleContract> GetInstancesOfExecutingModule();
    }
}
