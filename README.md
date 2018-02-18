# Start9 API
This is the home to the Start9 API, a way for module and application developers to integrate their application with Start9. Developers will be able to use this to create open, univeral modules for Start9 that support interoperability with Windows and other modules and open-ended customization with little restrictions, while still being secure. Application developers will be able to send metadata about their applications for module developers to take advantage of.

## Contributing and Conduct
This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information, see the [Contributor Covenant code of conduct](https://www.contributor-covenant.org/).

In addition, Please follow the [contributing guidelines](https://github.com/StartNine/Start9.Host/blob/master/CONTRIBUTING.md) for all Start9 projects.


## Dependencies and Frameworks
- .NET Framework 4.0
	- Official builds of Start9.Api are built with .NET Framework 4.0. However, you may want to retarget it to be able to access newer introduced in later versions of .NET. However, you may want to recompile any dependants to use the same framework version.
- WPF related assemblies (PresentationFramework, etc.)
	- These assemblies make Start9 incompatible with .NET Core and other non-Microsoft .NET implementations. 
- System.Windows.Forms
	- This is used for abstractions like Screen and Mouse.
-UIAutomationTypes and UIAutomationClient
	- Currently, these are used for global window closed and opened hooks. The uses may expand in the future.
- System.AddIn
	- This represents the Microsoft Addin Framework assembly. We use this in Start9 for the contracts part of the addin pipeline.
- Fluent.Ribbon
	- ControlsEx is a dependency of this.

Other, smaller dependencies can be seen in the [csproj file for the project](https://github.com/StartNine/Start9.Host/blob/master/Start9/Start9.csproj#L36). 

---
Interested? Join our Discord: [![Discord](https://img.shields.io/discord/321793250602254336.svg?style=flat-square&colorB=7289DA)](https://discord.gg/6cpvxBS)
