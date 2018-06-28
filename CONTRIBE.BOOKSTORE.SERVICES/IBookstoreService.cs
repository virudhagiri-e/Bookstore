#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : IBookstoreService.cs
//Purpose           : Contains the operation contract interface definition
//Date              : 26-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

using CONTRIBE.BOOKSTORE.MODEL.Entity;
using CONTRIBE.BOOKSTORE.MODEL.Entity.Interface;

#endregion

namespace CONTRIBE.BOOKSTORE.SERVICES
{
    /// <summary>
    /// IBookstoreService interface
    /// </summary>
    [ServiceKnownType(typeof(Book))]
    [ServiceContract]
    public interface IBookstoreService
    {
        /// <summary>
        /// Get books asynchronously
        /// </summary>
        /// <param name="searchString">search string in microsoft standard format</param>
        /// <returns>list of books</returns>
        [OperationContract]
        Task<IEnumerable<IBook>> GetBooksAsync(string searchString);

        /// <summary>
        /// Add book to the cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <param name="quantity">quantity</param>
        /// <returns>book cart object</returns>
        [OperationContract]
        Task<BookCart> AddBookAsync(string userID, string bookID, int quantity);

        /// <summary>
        /// Remove book from the cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <returns>book cart object</returns>
        [OperationContract]
        Task<BookCart> RemoveBookAsync(string userID, string bookID);

        /// <summary>
        /// Update the quantity in the book cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <param name="quantity">quantity</param>
        /// <returns>book cart object</returns>
        [OperationContract]
        Task<BookCart> UpdateQuantityAsync(string userID, string bookID, int quantity);

        /// <summary>
        /// Create order asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookCart">book cart object</param>
        /// <returns>order details</returns>
        [OperationContract]
        Task<OrderDetails> CreateOrderAsync(string userID, BookCart bookCart);

        /// <summary>
        /// Get order details asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <returns>order details</returns>
        [OperationContract]
        Task<List<OrderDetails>> GetOrderDetailsAsync(string UserID);
    }
}
