#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : BookCartTest.cs
//Purpose           : Contains the test methods for book cart
//Date              : 25-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using CONTRIBE.BOOKSTORE.MODEL.Entity;
using CONTRIBE.BOOKSTORE.MODEL.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace CONTRIBE.BOOKSTORE.UNITTEST
{
    [TestClass]
    public class BookCartTest
    {
        [TestMethod]
        public async Task AddRemoveUpdateClearBook()
        {
            var bookAsync1 = await new BookStoreHelper().SearchBooksAsync("title eq 'Mastering едц'");
            var bookAsync2 = await new BookStoreHelper().SearchBooksAsync("author eq 'Cunning Bastard'");
            var bookAsync3 = await new BookStoreHelper().SearchBooksAsync("title eq 'Generic Title'");

            var book1 = bookAsync1.First();
            var book2 = bookAsync2.First();
            var book3 = bookAsync3.First();

            var bookCartItem1 = new BookCartItem { Book = book1, Quantity = 5 };
            var bookCartItem2 = new BookCartItem { Book = book2, Quantity = 2 };
            var bookCartItem3 = new BookCartItem { Book = book3, Quantity = 3 };

            // Create book cart
            var bookCart = new BookCart();
            Assert.AreEqual(0, bookCart.BookCartItems.Count);
            Assert.AreEqual(0, bookCart.TotalQuantity);
            Assert.AreEqual(0, bookCart.TotalPrice);

            // Add 1st book cart item to the cart
            bookCart.AddBook(bookCartItem1);
            Assert.AreEqual(1, bookCart.BookCartItems.Count);
            Assert.AreEqual(5, bookCart.TotalQuantity);
            Assert.AreEqual(3810, bookCart.TotalPrice);

            // Add 2nd book cart item to the cart
            bookCart.AddBook(bookCartItem2);
            Assert.AreEqual(2, bookCart.BookCartItems.Count);
            Assert.AreEqual(7, bookCart.TotalQuantity);
            Assert.AreEqual(5808, bookCart.TotalPrice);

            // Add 3rd book cart item to the cart
            bookCart.AddBook(bookCartItem3);
            Assert.AreEqual(3, bookCart.BookCartItems.Count);
            Assert.AreEqual(10, bookCart.TotalQuantity);
            Assert.AreEqual(Convert.ToDecimal(6364.5), bookCart.TotalPrice);

            // Remove 2nd book cart item from the cart
            bookCart.RemoveBook(bookCartItem2);
            Assert.AreEqual(2, bookCart.BookCartItems.Count);
            Assert.AreEqual(8, bookCart.TotalQuantity);
            Assert.AreEqual(Convert.ToDecimal(4366.5), bookCart.TotalPrice);

            // Update quantity of 2nd book cart item
            bookCart.UpdateQuantity(bookCartItem2, 4);
            Assert.AreEqual(2, bookCart.BookCartItems.Count);
            Assert.AreEqual(8, bookCart.TotalQuantity);
            Assert.AreEqual(Convert.ToDecimal(4366.5), bookCart.TotalPrice);

            // Update quantity of 1st book cart item
            bookCart.UpdateQuantity(bookCartItem1, 1);
            Assert.AreEqual(2, bookCart.BookCartItems.Count);
            Assert.AreEqual(4, bookCart.TotalQuantity);
            Assert.AreEqual(Convert.ToDecimal(1318.5), bookCart.TotalPrice);

            // Empty the cart
            bookCart.Clear();
            Assert.AreEqual(0, bookCart.BookCartItems.Count);
            Assert.AreEqual(0, bookCart.TotalQuantity);
            Assert.AreEqual(0, bookCart.TotalPrice);
        }
    }
}
