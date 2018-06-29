using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Start9.Api.Contracts
{
    #region Modules

    [AddInContract]
    public interface IModuleContract : IContract
    {
        IConfigurationContract Configuration { get; set; }
        IMessageContractContract MessageContract { get; }
        IReceiverContractContract ReceiverContract { get; }
        void Initialize(IHostContract host);
    }

    #endregion

    #region Messages

    public interface IMessageContractContract : IContract
    {
        IListContract<IMessageEntryContract> Entries { get; }
    }

    public interface IMessageEntryContract : IContract
    {
        Type Type { get; }
        String FriendlyName { get; }
    }

    public interface IMessageContract : IContract
    {
        Object Object { get; }
        IMessageEntryContract MessageEntry { get; }
    }

    #endregion
    
    #region Receivers

    public interface IReceiverContractContract : IContract
    {
        IListContract<IReceiverEntryContract> Entries { get; }
    }

    public interface IReceiverEntryContract : IContract
    {
        Type Type { get; }
        String FriendlyName { get; }
        void SendMessage(IMessageContract mesage);
        void MessageReceivedEventAdd(IMessageEventHandlerContract handler);
        void MessageReceivedEventRemove(IMessageEventHandlerContract handler);
    }

    public interface IMessageEventHandlerContract : IContract
    {
        void Handler(IMessageReceivedEventArgsContract args);
    }

    public interface IMessageReceivedEventArgsContract : IContract
    {
        IMessageContract Message { get; }
    }

    #endregion

    #region Configuration

    public interface IConfigurationContract : IContract
    {
        IListContract<IConfigurationEntryContract> Entries { get; }
    }

    public interface IConfigurationEntryContract : IContract
    {
        Object Object { get; }
        String FriendlyName { get; }
    }

    #endregion

    public interface IHostContract : IContract
    {
        void SendMessage(IMessageContract message);
        void SaveConfiguration(IModuleContract module);
        IListContract<IModuleContract> GetModules();
        IConfigurationContract GetGlobalConfiguration();
    }

    public interface IModuleAssemblyContract : IContract
    {
        String Name { get; }
        String Description { get; }
        String Publisher { get; }
        Version Version { get; }
        IListContract<IModuleInstanceContract> Instances { get; }
        IConfigurationContract SavedConfiguration { get; }
        IConfigurationContract CurrentConfiguration { get; }
        IMessageContractContract MessageContract { get; }
        IReceiverContractContract ReceiverContract { get; }
        void Kill(IModuleInstanceContract instance);
        void KillAll();
        void Activate(Boolean initialize);
    }

    public interface IModuleInstanceContract : IContract
    {
        IModuleContract Instance { get; }
        Int32 ProcessId { get; }
    }
}
