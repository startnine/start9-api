using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections;

namespace Start9.Api.Contracts
{
    [AddInContract]
    public interface IModuleContract : IContract
    {
        IConfigurationContract Configuration { get; }
        IMessageContractContract MessageContract { get; }
        IReceiverContractContract ReceiverContract { get; }

        void Initialize(IHostContract host);

        void MessageReceivedEventAdd(IMessageEventHandlerContract handler);
        void MessageReceivedEventRemove(IMessageEventHandlerContract handler);
    }

    public interface IMessageContractContract : IContract
    {
        IListContract<IMessageEntryContract> Entries { get; }
    }

    public interface IMessageEntryContract : IContract
    {
        Type MessageObjectType { get; }
        String FriendlyName { get; }

        void MessageSentEventAdd(IMessageEventHandlerContract handler);
        void MessageSentEventRemove(IMessageEventHandlerContract handler);
    }


    public interface IMessageContract : IContract
    {
        Object Object { get; }
        IMessageEntryContract MessageEntry { get; }
    }

    public interface IReceiverContractContract : IContract
    {
        IListContract<IMessageEntryContract> Entries { get; }
    }


    public interface IReceiverEntryContract : IContract
    {
        String FriendlyName { get; }
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

    public interface IConfigurationContract : IContract
    {
        IListContract<IConfigurationEntryContract> Entries { get; }
    }

    public interface IConfigurationEntryContract : IContract
    {
        Object Object { get; }
        String FriendlyName { get; }
    }

    public interface IHostContract : IContract
    {
        void SendMessage(IMessageContract message);
        void SaveConfiguration(IModuleContract module);
        IListContract<IModuleContract> GetModules();
        IConfigurationContract GetGlobalConfiguration();
    }
}
    