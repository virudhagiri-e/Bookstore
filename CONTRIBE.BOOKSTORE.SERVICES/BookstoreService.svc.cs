#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : BookstoreService.cs
//Purpose           : Contains the properties/methods for handling book store services
//Date              : 26-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CONTRIBE.BOOKSTORE.MODEL.Entity;
using CONTRIBE.BOOKSTORE.MODEL.Entity.Interface;
using CONTRIBE.BOOKSTORE.MODEL.Helper;

#endregion

namespace CONTRIBE.BOOKSTORE.SERVICES
{
    /// <summary>
    /// BookstoreService class
    /// </summary>
    public class BookstoreService : IBookstoreService
    {
        /// <summary>
        /// Get books asynchronously
        /// </summary>
        /// <param name="searchString">search string in microsoft standard format</param>
        /// <returns>list of books</returns>
        public async Task<IEnumerable<IBook>> GetBooksAsync(string searchString)
        {
            var books = new List<Book>();

            try
            {
                var booksAsync = await new BookStoreHelper().SearchBooksAsync(searchString);
                books = booksAsync;
            }
            catch (Exception)
            {
                // Log the exception
            }

            return books;
        }

        /// <summary>
        /// Add book to the cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <param name="quantity">quantity</param>
        /// <returns>book cart object</returns>
        public async Task<BookCart> AddBookAsync(string userID, string bookID, int quantity)
        {
            BookCart bookCart = null;

            try
            {
                var bookStore = new BookStoreHelper();
                var customer = bookStore.GetCustomer(userID);
                var book = await bookStore.SearchBookById(bookID);

                if (customer != null && book != null)
                {
                    customer.BookCart.AddBook(new BookCartItem { Book = book, Quantity = quantity });
                    bookCart = customer.BookCart;
                }
            }
            catch
            {
                // Log the exception
            }

            return bookCart ?? new BookCart();
        }

        /// <summary>
        /// Remove book from the cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <returns>book cart object</returns>
        public async Task<BookCart> RemoveBookAsync(string userID, string bookID)
        {
            BookCart bookCart = null;

            try
            {
                var bookStore = new BookStoreHelper();
                var customer = bookStore.GetCustomer(userID);
                var book = await bookStore.SearchBookById(bookID);

                if (customer != null && book != null)
                {
                    customer.BookCart.RemoveBook(new BookCartItem { Book = book });
                    bookCart = customer.BookCart;
                }
            }
            catch
            {
                // Log the exception
            }

            return bookCart ?? new BookCart();
        }

        /// <summary>
        /// Update the quantity in the book cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <param name="quantity">quantity</param>
        /// <returns>book cart object</returns>
        public async Task<BookCart> UpdateQuantityAsync(string userID, string bookID, int quantity)
        {
            BookCart bookCart = null;

            try
            {
                var bookStore = new BookStoreHelper();
                var customer = bookStore.GetCustomer(userID);
                var book = await bookStore.SearchBookById(bookID);

                if (customer != null && book != null)
                {
                    customer.BookCart.UpdateQuantity(new BookCartItem { Book = book }, quantity);
                    bookCart = customer.BookCart;
                }
            }
            catch
            {
                // Log the exception
            }

            return bookCart ?? new BookCart();
        }

        /// <summary>
        /// Create order asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookCart">book cart object</param>
        /// <returns>order details</returns>
        public async Task<OrderDetails> CreateOrderAsync(string userID, BookCart bookCart)
        {
            OrderDetails orderDetails = null;

            try
            {
                var bookStore = new BookStoreHelper();
                var customer = bookStore.GetCustomer(userID);
                customer.BookCart = bookCart;

                if (customer != null && bookCart != null)
                {
                    orderDetails = await customer.PlaceOrder(); ;
                }
            }
            catch
            {
                // Log the exception
            }

            return orderDetails ?? new OrderDetails();
        }

        /// <summary>
        /// Get order details asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <returns>order details</returns>
        public async Task<List<OrderDetails>> GetOrderDetailsAsync(string userID)
        {
            List<OrderDetails> orderDetails = null;

            try
            {
                var bookStore = new BookStoreHelper();
                var customer = bookStore.GetCustomer(userID);

                orderDetails = customer.Orders;
            }
            catch
            {
                // Log the exception
            }

            orderDetails = orderDetails ?? new List<OrderDetails>();

            return await Task.FromResult(orderDetails);
        }
    }
}