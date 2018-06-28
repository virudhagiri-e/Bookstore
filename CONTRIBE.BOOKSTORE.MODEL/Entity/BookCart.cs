#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : BookCart.cs
//Purpose           : Contains the properties/methods for handling book cart business object
//Date              : 24-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using CONTRIBE.BOOKSTORE.MODEL.Entity.Interface;
using CONTRIBE.BOOKSTORE.MODEL.Helper;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Entity
{
    /// <summary>
    /// BookCart <see langword="class"/>
    /// </summary>
    public class BookCart
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BookCart()
        {
            BookCartItems = new List<BookCartItem>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets list of bookcart item
        /// </summary>
        public List<BookCartItem> BookCartItems { get; set; }

        /// <summary>
        /// Gets or sets total price
        /// </summary>
        public decimal TotalPrice
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets total quantity
        /// </summary>
        public int TotalQuantity
        {
            get; set;
        }

        #endregion Properties

        #region public Methods

        /// <summary>
        /// To add book to the cart
        /// </summary>
        /// <param name="bookCartItem">book cart item</param>
        public void AddBook(BookCartItem bookCartItem)
        {
            if (bookCartItem != null && bookCartItem.Book != null)
            {
                var existingBookCartItem = BookCartItems.FirstOrDefault(b => b.Book != null && b.Book.ID == bookCartItem.Book.ID);
                if (existingBookCartItem == null)
                {
                    BookCartItems.Add(bookCartItem);
                }
                else
                {
                    existingBookCartItem.Quantity += bookCartItem.Quantity;
                }

                UpdateTotalPrice();
            }
        }

        /// <summary>
        /// To remove book from the cart
        /// </summary>
        /// <param name="bookCartItem">book cart item</param>
        public void RemoveBook(BookCartItem bookCartItem)
        {
            if (bookCartItem != null && bookCartItem.Book != null)
            {
                var existingBookCartItem = BookCartItems.FirstOrDefault(b => b.Book != null && b.Book.ID == bookCartItem.Book.ID);
                if (existingBookCartItem != null)
                {
                    BookCartItems.Remove(existingBookCartItem);
                }

                UpdateTotalPrice();
            }
        }

        /// <summary>
        /// To Update the book cart item quantity
        /// </summary>
        /// <param name="bookCartItem">book cart item</param>
        /// <param name="quantity">number of quantity</param>
        public void UpdateQuantity(BookCartItem bookCartItem, int quantity)
        {
            if (bookCartItem != null && bookCartItem.Book != null)
            {
                var existingBookCartItem = BookCartItems.FirstOrDefault(b => b.Book != null && b.Book.ID == bookCartItem.Book.ID);
                if (existingBookCartItem != null)
                {
                    existingBookCartItem.Quantity = quantity;
                }

                UpdateTotalPrice();
            }
        }

        /// <summary>
        /// To empty the cart
        /// </summary>
        public void Clear()
        {
            BookCartItems.Clear();

            UpdateTotalPrice();
        }

        #endregion public Methods

        #region Private Methods

        /// <summary>
        /// To update total quantity and price
        /// </summary>
        private void UpdateTotalPrice()
        {
            TotalPrice = (from item in BookCartItems
                          select item.Quantity * item.Book.Price).Sum();

            TotalQuantity = (from item in BookCartItems
                             select item.Quantity).Sum();
        }

        #endregion
    }
}