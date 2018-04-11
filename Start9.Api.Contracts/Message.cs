using System;

namespace Start9.Api
{
	[Serializable]
	public struct Message<TObject, TReceiver>
	{
        public Message(TReceiver receiver, TObject o = default, String message = default)
        {
            Receiver = receiver;
            Object = o;
            Text = message;
        }

        public TObject Object { get; }
        public String Text { get; }
        public TReceiver Receiver { get; }

        public void Send()
        {
            // Reminder to convert this to use shapes when those come out. Right now it'll throw an exception, which is bad because generics are supposed to remove exceptions from the programming model
            typeof(TReceiver).GetMethod("MessageRevieved").MakeGenericMethod(new[] { typeof(TObject), typeof(TReceiver) }).Invoke(Receiver, new Object[] { this });
        }
    }
}