using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Start9.Api.Contracts
{
	[AddInContract]
	public interface ILibraryManagerContract : IContract
	{
		// Get a IBookInfoContract object
		// from the add-in of the
		// the best selling book.
		IBookInfoContract BestSeller { get; }

		// Pass a collection of books,
		// of type IBookInfoContract
		// to the add-in for processing.
		void ProcessBooks(IListContract<IBookInfoContract> books);

		// This method has has arbitrary
		// uses and shows how you can
		// mix serializable and custom types.
		string Data(string txt);
	}

	// Contains infomration about a book.
	public interface IBookInfoContract : IContract
	{
		string Id { get; }
		string Author { get; }
		string Title { get; }
		string Genre { get; }
		string Price { get; }
		string PublishDate { get; }
		string Description { get; }
	}
}