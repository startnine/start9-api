//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Start9.Api.Contracts;

namespace Start9.Api.AddInSideAdapters
{
    
    public class IReceiverEntryViewToContractAddInAdapter : System.AddIn.Pipeline.ContractBase, Start9.Api.Contracts.IReceiverEntryContract
    {
        private Start9.Api.IReceiverEntry _view;
        public IReceiverEntryViewToContractAddInAdapter(Start9.Api.IReceiverEntry view)
        {
            _view = view;
        }
        public System.Type Type
        {
            get
            {
                return _view.Type;
            }
        }
        public string DisplayName
        {
            get
            {
                return _view.DisplayName;
            }
        }
        public void SendMessage(IMessageContract message)
        {
            _view.SendMessage(IMessageAddInAdapter.ContractToViewAdapter(message));
        }
        internal Start9.Api.IReceiverEntry GetSourceView()
        {
            return _view;
        }
    }
}
