#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : CustomerTest.cs
//Purpose           : Contains the test methods for customer
//Date              : 26-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using CONTRIBE.BOOKSTORE.MODEL.Entity;
using CONTRIBE.BOOKSTORE.MODEL.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace CONTRIBE.BOOKSTORE.UNITTEST
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public async Task PlaceOrder()
        {
            // Clear the cache
            Cache.Clear();

            var bookStoreHelper = new BookStoreHelper();
            var bookAsync1 = await bookStoreHelper.SearchBooksAsync("title eq 'Mastering едц'");
            var bookAsync2 = await bookStoreHelper.SearchBooksAsync("author eq 'Cunning Bastard'");
            var bookAsync3 = await bookStoreHelper.SearchBooksAsync("title eq 'Generic Title'");
            var bookAsync4 = await bookStoreHelper.SearchBooksAsync("title eq 'How To Spend Money'");

            var book1 = bookAsync1.First();
            var book2 = bookAsync2.First();
            var book3 = bookAsync3.First();
            var book4 = bookAsync4.First();

            var bookCartItem1 = new BookCartItem { Book = book1, Quantity = 5 };
            var bookCartItem2 = new BookCartItem { Book = book2, Quantity = 2 };
            var bookCartItem3 = new BookCartItem { Book = book3, Quantity = 3 };
            var bookCartItem4 = new BookCartItem { Book = book4, Quantity = 2 };

            // Add books to the cart
            var bookCart = new BookCart();
            bookCart.AddBook(bookCartItem1);
            bookCart.AddBook(bookCartItem2);
            bookCart.AddBook(bookCartItem3);
            bookCart.AddBook(bookCartItem4);

            // Test empty cart
            var customer = bookStoreHelper.GetCustomer("dummy");
            await Assert.ThrowsExceptionAsync<DataException>(() => customer.PlaceOrder());

            // Create customer order (combination of In-stock and not In-stock)
            customer = bookStoreHelper.GetCustomer("dummy");
            customer.BookCart = bookCart;
            var orderDetails = await customer.PlaceOrder();
            Assert.AreEqual(4, orderDetails.PurchasedBooks.Count);
            Assert.AreEqual(1, orderDetails.NotInStockBooks.Count);
            Assert.AreEqual(11, orderDetails.TotalQuantity);
            Assert.AreEqual(Convert.ToDecimal(1006364.5), orderDetails.TotalPrice);
            Assert.AreEqual(0, customer.BookCart.BookCartItems.Count);
            Assert.IsTrue(customer.Orders.IsNotEmpty());
            // Check In-Stock
            Assert.AreEqual(10, book1.InStock);
            Assert.AreEqual(18, book2.InStock);
            Assert.AreEqual(2, book3.InStock);
            Assert.AreEqual(0, book4.InStock);

            // Add books to the cart
            bookCart.AddBook(bookCartItem4);

            // Create customer order (not In-stock)
            orderDetails = await customer.PlaceOrder();
            Assert.AreEqual(0, orderDetails.PurchasedBooks.Count);
            Assert.AreEqual(1, orderDetails.NotInStockBooks.Count);
            Assert.AreEqual(0, orderDetails.TotalQuantity);
            Assert.AreEqual(Convert.ToDecimal(0), orderDetails.TotalPrice);
        }
    }
}
