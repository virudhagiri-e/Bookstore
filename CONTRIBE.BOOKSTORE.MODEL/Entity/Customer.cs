#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : Customer.cs
//Purpose           : Contains the properties/methods for handling customer business object
//Date              : 24-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CONTRIBE.BOOKSTORE.MODEL.Helper;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Entity
{
    /// <summary>
    /// Customer <see langword="class"/>
    /// </summary>
    public class Customer
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="Id">user Id</param>
        public Customer(string Id)
        {
            ID = Id.SafeToUpper().SafeTrim();
            BookCart = new BookCart();
            Orders = new List<OrderDetails>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets ID of the customer
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets BookCart
        /// </summary>
        public BookCart BookCart { get; set; }

        /// <summary>
        /// Gets or sets orders
        /// </summary>
        public List<OrderDetails> Orders { get; set; }

        #endregion Properties

        #region public Methods

        /// <summary>
        /// To create the order
        /// </summary>
        /// <returns>order details</returns>
        public async Task<OrderDetails> PlaceOrder()
        {
            if (BookCart == null || BookCart.BookCartItems.IsEmpty())
            {
                throw new DataException("Cart is empty");
            }

            var orderDetails = new OrderDetails()
            {
                PurchasedBooks = new List<BookCartItem>(),
                NotInStockBooks = new List<BookCartItem>()
            };

            // Get available book list
            var availableBooks = await new BookStoreHelper().GetAllBooks();
            availableBooks = availableBooks.Where(b => b.InStock > 0).ToList();

            // Loop through the cart items which is having quantity greater than '0'
            BookCart.BookCartItems.Where(item => item.Quantity > 0).ForEach(item =>
            {
                // check for the stock
                var availableBook = availableBooks.FirstOrDefault(b => b.ID == item.Book.ID && b.InStock > 0);
                if (availableBook != null)
                {
                    if (availableBook.InStock < item.Quantity)
                    {
                        orderDetails.NotInStockBooks.Add(new BookCartItem { Book = item.Book, Quantity = item.Quantity - availableBook.InStock });
                        item.Quantity = availableBook.InStock;
                    }

                    orderDetails.PurchasedBooks.Add(item);
                    availableBook.UpdateInStock(availableBook.InStock - item.Quantity);
                }
                else
                {
                    orderDetails.NotInStockBooks.Add(item);
                }
            });

            orderDetails.TotalPrice = (from item in orderDetails.PurchasedBooks
                                       select item.Quantity * item.Book.Price).Sum();

            orderDetails.TotalQuantity = (from item in orderDetails.PurchasedBooks
                                          select item.Quantity).Sum();

            // clear the cart
            BookCart.Clear();

            Orders.Add(orderDetails);

            return orderDetails;
        }

        #endregion
    }
}