using System;

namespace Start9.Api
{
    public class Message<T> : IMessage
    {
        public Message(T o, IMessageEntry<T> entry)
        {
            Object = o;
            MessageEntry = entry;
        }

        public T Object { get; }

        Object IMessage.Object => Object;

        public IMessageEntry MessageEntry { get; }
    }

    public class MessageEntry<T> : IMessageEntry<T>
    {
        public MessageEntry(String name)
        {
            DisplayName = name;
        }

        public Type Type => typeof(T);

        public String DisplayName { get; }
    }

    public interface IMessageEntry<in T> : IMessageEntry
    {
        // nothing
    }

    public class ReceiverEntry<T> : IReceiverEntry
    {
        public ReceiverEntry(String name)
        {
            DisplayName = name;
        }

        public Type Type => typeof(T);
        public String DisplayName { get; }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void SendMessage(IMessage message) => MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message));
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(IMessage message)
        {
            Message = message;
        }

        public IMessage Message { get; }
    }

    public class ConfigurationChangedEventArgsugh : AConfigurationChangedEventArgs
    {
        public ConfigurationChangedEventArgsugh(IConfigurationEntry old, IConfigurationEntry @new)
        {
            Old = old;
            New = @new;
        }

        public override IConfigurationEntry Old { get; }

        public override IConfigurationEntry New { get; }
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ConfigurationEntryAttribute : Attribute
    {
        public ConfigurationEntryAttribute(String displayName, String description)
        {
            DisplayName = displayName;
            Description = description;
        }

        public String DisplayName { get; }
        public String Description { get; }

    }
}