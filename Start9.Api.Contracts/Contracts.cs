using System;
using System.AddIn.Contract;
using System.AddIn.Pipeline;
using System.Collections;

namespace Start9.Api.Contracts
{

    /// <summary>
    /// The contract defining the add-in.
    /// </summary>
    [AddInContract]
    public interface IModuleContract : IContract
    {
        /// <summary>
        /// Sends a message to the module.
        /// /// </summary>
        /// <param name="message">The message to send to the module</param>
        /// <returns>A message sent as a response by the module.</returns>
        IMessageContract SendMessage(IMessageContract message);

        /// <summary>
        /// Gets the module's configuration.
        /// </summary>
        /// <value>An object representing the module's configuration.</value>
        /// <remarks>
        /// This is the <see cref="IConfigurationContract"/>-matching object representing the current configuration of the module. To notify the add-in host of changes to the configuration, invoke <see cref="IHostContract.GetConfiguration(IModuleContract)"/>. Usually, implementations of the contract implement the <see cref="System.ComponentModel.INotifyPropertyChanged"/> interface and invoke this method when <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> is raised.
        /// </remarks>
        /// <seealso cref="IConfigurationContract"/>
        IConfigurationContract Configuration { get; }

        /// <summary>
        /// This method supports the add-in pipeline infrastructure and is not intended to be used directly from your code. Fires when the add-in host sends a host object to the module.
        /// </summary>
        /// <param name="host">The host object sent by the add-in host.</param>
        /// <remarks>
        /// Upon module activation, the add-in host will send an object matching the <see cref="IHostContract"/> contract to represent functions and data that the module can invoke on the add-in host. Currently, this method is only invoked directly after the activation and may have its functionality changed into a different form in the future. Modules have the freedom to act on the invocation of this method however they would like, however, the most common implementation of this method is to assign the host to a globally-accessible variable.
        /// </remarks>
        /// <seealso cref="IHostContract"/>
        [Obsolete("This method supports the add-in pipeline infrastructure and is not intended to be used directly from your code.")]
        void HostReceived(IHostContract host);
    }
    /// <summary>
    /// The contract defining messages.
    /// </summary>
    public interface IMessageContract : IContract
    {
        /// <summary>
        /// Gets a textual message attached to the object.
        /// </summary>
        /// <value>A string of text attached to the object of the message.</value>
        String Text { get; }

        /// <summary>
        /// Gets the object of the message.
        /// </summary>
        /// <value>
        /// The object attached to the message.
        /// </value>
        /// <remarks>
        /// The type of this object must be serializable. If it is not, an exception will be thrown as it crosses the isolation boundary.
        /// </remarks>
        Object Object { get; }
    }

    /// <summary>
    /// The contract defining configuration.
    /// </summary>
    /// <remarks>
    /// Implementers are intended to add configuration options as properties on the class, implement <see cref="System.ComponentModel.INotifyPropertyChanged"/> and call <see cref="IHostContract.GetConfiguration(IModuleContract)"/>, and use reflection to loop over all properties and return a dictionary of the properties' names and their values.
    /// <example>
    /// <code>
    ///     public class ModuleConfiguration : IConfiguration, INotifyPropertyChanged
    ///     {
    ///         public ModuleConfiguration(IHost host)
    ///         {
    ///             PropertyChanged += (sender, e) => host.GetConfiguration(ModuleAddIn.Instance); //ModuleAddIn.Instance should be replaced with your own code to get access to the add-in object.
    ///         }
    ///         
    ///         public IDictionary Entries => GetType().GetProperties().ToDictionary(k => k.Name, v => v.GetValue(this));
    /// 
    ///         TaskbarGroupingMode _groupmode;
    ///         
    ///         public TaskbarGroupingMode TaskbarGroupingMode
    ///         {
    ///             get => _groupmode;
    ///             set 
    ///             {
    ///                 _groupmode = value;
    ///                 PropertyChanged(nameof(TaskbarGroupingMode));
    ///             }
    ///         }
    ///     }
    /// </code>
    /// </example>
    /// </remarks>
    public interface IConfigurationContract : IContract
    {
        /// <summary>
        /// Gets the configuration options.
        /// </summary>
        /// <value>
        /// A dictionary with the names of options as keys and their values as values.
        /// </value>
        /// <remarks>
        /// See <see cref="IConfigurationContract"/>'s remarks for more information.
        /// </remarks>
        IDictionary Entries { get; }
    }

    /// <summary>
    /// The contract defining the add-in host's capabilities served to the add-in.
    /// </summary>
    /// <remarks>
    /// The implementation of this contract is passed to the module upon activation through the <see cref="IModuleContract.HostReceived(IHostContract)"/> method.
    /// </remarks>
    public interface IHostContract : IContract
    {
        /// <summary>
        /// Sends a message to all modules.
        /// </summary>
        /// <param name="message">The message to send to all modules.</param>
        void SendGlobalMessage(IMessageContract message);

        /// <summary>
        /// Gets a list of all installed modules.
        /// </summary>
        /// <returns>A contract representing the list of modules.</returns>
        IListContract<IModuleContract> GetModules();

        /// <summary>
        /// Notifies the host to update the stored configuration for the module.
        /// </summary>
        /// <param name="module">The module that is requesting the configuration to be updated.</param>
        /// <returns>The return value is currently unused.</returns>
        /// <remarks>This method was originally intended for a different purpose. The name and return value will be eventually updated to reflect its true use.</remarks>
        IConfigurationContract GetConfiguration(IModuleContract module);
    }
}
    