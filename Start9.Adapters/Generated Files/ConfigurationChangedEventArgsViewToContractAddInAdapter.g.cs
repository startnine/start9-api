//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Start9.Api.AddInSideAdapters
{
    
    public class ConfigurationChangedEventArgsViewToContractAddInAdapter : System.AddIn.Pipeline.ContractBase, Start9.Api.Contracts.IConfigurationChangedEventArgsContract
    {
        private Start9.Api.AConfigurationChangedEventArgs _view;
        public ConfigurationChangedEventArgsViewToContractAddInAdapter(Start9.Api.AConfigurationChangedEventArgs view)
        {
            _view = view;
        }
        public Start9.Api.Contracts.IConfigurationEntryContract Old
        {
            get
            {
                return Start9.Api.AddInSideAdapters.IConfigurationEntryAddInAdapter.ViewToContractAdapter(_view.Old);
            }
        }
        public Start9.Api.Contracts.IConfigurationEntryContract New
        {
            get
            {
                return Start9.Api.AddInSideAdapters.IConfigurationEntryAddInAdapter.ViewToContractAdapter(_view.New);
            }
        }
        internal Start9.Api.AConfigurationChangedEventArgs GetSourceView()
        {
            return _view;
        }
    }
}

