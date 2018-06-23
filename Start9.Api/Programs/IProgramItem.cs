using System.Drawing;

namespace Start9.Api.Programs
{
    /// <summary>
    /// Represents a program.
    /// </summary>
	public interface IProgramItem
	{
        /// <summary>
        /// Gets the name of the program.
        /// </summary>
        System.String Name { get; }

        /// <summary>
        /// Gets the icon of the program.
        /// </summary>
        /// <value>
        /// An <see cref="Icon"/> representing the program's icon that can scale to different sizes.
        /// </value>
		Icon Icon { get; }

        /// <summary>
        /// Opens a new instance of a program.
        /// </summary>
		void Open();
	}
}