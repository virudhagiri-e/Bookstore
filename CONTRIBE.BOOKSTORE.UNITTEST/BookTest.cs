#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : BookTest.cs
//Purpose           : Contains the test methods for book
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
    public class BookTest
    {
        [TestMethod]
        public async Task UpdateInStock()
        {
            // Clear the cache
            Cache.Clear();

            // Search books
            var books = await new BookStoreHelper().SearchBooksAsync("title eq 'Mastering едц'");
            Assert.AreEqual(1, books.Count);
            Assert.AreEqual(15, books[0].InStock);

            // Update In-Stock
            books[0].UpdateInStock(20);
            Assert.AreEqual(20, books[0].InStock);

            // Search books again and verify InStock
            books = await new BookStoreHelper().SearchBooksAsync("title eq 'Mastering едц'");
            Assert.AreEqual(1, books.Count);
            Assert.AreEqual(20, books[0].InStock);

            // Update In-Stock with negative values
            books[0].UpdateInStock(-20);
            Assert.AreEqual(20, books[0].InStock);
        }
    }
}
