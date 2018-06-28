#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : BookStoreTest.cs
//Purpose           : Contains the test methods for book store
//Date              : 25-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using CONTRIBE.BOOKSTORE.MODEL.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

#endregion

namespace CONTRIBE.BOOKSTORE.UNITTEST
{
    [TestClass]
    public class BookStoreTest
    {
        [TestMethod]
        public async Task GetAllBooks()
        {
            var bookStoreHelper = new BookStoreHelper();
            var books = await bookStoreHelper.GetAllBooks();
            Assert.IsTrue(books.IsNotEmpty());

            // Check we have same books object returned for multiple method invocation
            var books1 = await bookStoreHelper.GetAllBooks();
            Assert.AreEqual(books, books1);
        }

        [TestMethod]
        public async Task SearchBooks()
        {
            var bookStoreHelper = new BookStoreHelper();
            // Search book with title/author
            var books = await bookStoreHelper.SearchBooksAsync("title eq 'generic' || author eq 'rIch'");
            Assert.AreEqual(4, books.Count);

            // Search book with title/author
            books = await bookStoreHelper.SearchBooksAsync("title eq 'gen' || author eq 'aut'");
            Assert.AreEqual(2, books.Count);

            // Search book with title and author
            books = await bookStoreHelper.SearchBooksAsync("title eq 'genEric' && author eq 'rich'");
            Assert.AreEqual(0, books.Count);

            // Search book with title and author
            books = await bookStoreHelper.SearchBooksAsync("title eq 'Desired' && author eq 'rich'");
            Assert.AreEqual(1, books.Count);

            // Search book with title 'generic'
            books = await bookStoreHelper.SearchBooksAsync("title eq 'generic'");
            Assert.AreEqual(2, books.Count);

            // Search book with title 'test'
            books = await bookStoreHelper.SearchBooksAsync("title eq 'test'");
            Assert.AreEqual(0, books.Count);

            // Search book with author 'First'
            books = await bookStoreHelper.SearchBooksAsync("author eq 'First'");
            Assert.AreEqual(1, books.Count);

            // Search book with blank
            books = await bookStoreHelper.SearchBooksAsync("");
            Assert.AreEqual(0, books.Count);

            // Search book with null
            books = await bookStoreHelper.SearchBooksAsync(null);
            Assert.AreEqual(0, books.Count);
        }

        [TestMethod]
        public void GetCustomer()
        {
            var bookStoreHelper = new BookStoreHelper();
            var customer1 = bookStoreHelper.GetCustomer("dummy");
            Assert.AreEqual("DUMMY", customer1.ID);

            var customer2 = bookStoreHelper.GetCustomer("anonymous");
            Assert.AreEqual("ANONYMOUS", customer2.ID);

            // Check we have only one customer object for the given userId
            var customer3 = bookStoreHelper.GetCustomer("Dummy");
            Assert.AreSame(customer1, customer3);
            Assert.AreEqual("DUMMY", customer3.ID);

            // Check we have only one customer object for the given userId
            var customer4 = bookStoreHelper.GetCustomer("anonymouS");
            Assert.AreSame(customer2, customer4);
            Assert.AreEqual("ANONYMOUS", customer4.ID);
        }
    }
}
