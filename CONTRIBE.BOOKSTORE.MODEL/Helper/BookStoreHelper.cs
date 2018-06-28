#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : BookStoreHelper.cs
//Purpose           : Contains the helper methods for book store
//Date              : 24-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CONTRIBE.BOOKSTORE.MODEL.Entity;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Helper
{
    /// <summary>
    /// BookStoreHelper <see langword="class"/>
    /// </summary>
    public class BookStoreHelper
    {
        #region Inner class

        /// <summary>
        /// Inner class RootBook
        /// </summary>
        class RootBook
        {
            public List<Book> Books { get; set; }
        }

        #endregion Inner class

        #region Public Methods

        /// <summary>
        /// Get all available books
        /// </summary>
        /// <returns>list of books</returns>
        public async Task<List<Book>> GetAllBooks()
        {
            List<Book> getData()
            {
                var bookList = new List<Book>();

                try
                {
                    using (var webClient = new WebClient() { Encoding = Encoding.UTF8 })
                    {
                        var jsonBooks = webClient.DownloadString("https://raw.githubusercontent.com/contribe/contribe/dev/arbetsprov-net/books.json");
                        var rootBook = JsonConvert.DeserializeObject<RootBook>(jsonBooks);
                        bookList.AddRange(rootBook.Books);
                    }
                }
                catch
                {
                    // log the exception
                }

                return bookList;
            }

            return await Task.FromResult(Cache.GetCacheValue(getData, MethodBase.GetCurrentMethod()));
        }

        /// <summary>
        /// To search for book
        /// </summary>
        /// <param name="searchString">search string</param>
        /// <returns>list of books</returns>
        public async Task<List<Book>> SearchBooksAsync(string searchString)
        {
            var allBooks = await GetAllBooks();

            var hasAnd = searchString.SafeToUpper().Contains(" && ");
            var hasOr = searchString.SafeToUpper().Contains(" || ");

            var searchValue = GetFilterValue(searchString, "author");
            var filteredBooks = (from book in allBooks
                                 where !searchValue.IsNullOrEmpty() && book.Author.ToLower().Contains(searchValue)
                                 select book).ToList();

            searchValue = GetFilterValue(searchString, "title");

            if (hasAnd)
            {
                filteredBooks = filteredBooks.Intersect(allBooks.Where(b => !searchValue.IsNullOrEmpty() && b.Title.ToLower().Contains(searchValue))).ToList();
            }
            else
            {
                filteredBooks = filteredBooks.Union(allBooks.Where(b => !searchValue.IsNullOrEmpty() && b.Title.ToLower().Contains(searchValue))).ToList();
            }

            return filteredBooks;
        }

        /// <summary>
        /// To Get book
        /// </summary>
        /// <param name="bookId">book Id</param>
        /// <returns>customer object</returns>
        public async Task<Book> SearchBookById(string bookId)
        {
            var allBooks = await GetAllBooks();
            var book = allBooks.FirstOrDefault(b => b.ID == bookId.SafeTrim());

            return book;
        }

        /// <summary>
        /// To Get customer
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>customer object</returns>
        public Customer GetCustomer(string userId)
        {
            var allCustomers = GetAllCustomers();
            var customer = allCustomers.FirstOrDefault(c => c.ID == userId.SafeToUpper().SafeTrim());

            if (customer == null)
            {
                customer = new Customer(userId);
                allCustomers.Add(customer);
            }

            return customer;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// To get search value for the key
        /// </summary>
        /// <param name="searchString">search string</param>
        /// <param name="filterKey">search key</param>
        /// <returns>filtered value</returns>
        private string GetFilterValue(string searchString, string filterKey)
        {
            // format : title eq 'generic' or author eq 'rich'
            string filterValue = string.Empty;

            try
            {
                var filterKeys = searchString.Replace("'", "").Replace("\"", "").ToLower().Split(new string[] { " && ", " || " }, System.StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (var key in filterKeys)
                {
                    if (key.Contains(filterKey.ToLower()))
                    {
                        var splitVal = key.Split(new string[] { "eq" }, System.StringSplitOptions.RemoveEmptyEntries);

                        if (splitVal.Length == 2)
                        {
                            filterValue = splitVal[1].Trim();
                        }

                        break;
                    }
                }
            }
            catch
            {
                filterValue = string.Empty;
            }

            return filterValue;
        }

        /// <summary>
        /// Get all available customers
        /// </summary>
        /// <returns>list of customers</returns>
        private List<Customer> GetAllCustomers()
        {
            List<Customer> getData()
            {
                var customerList = new List<Customer>()
                {
                    new Customer("dummy")
                };

                return customerList;
            }

            return Cache.GetCacheValue(getData, MethodBase.GetCurrentMethod());
        }

        #endregion Private Methods
    }
}