#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : HomeController.cs
//Purpose           : Contains the action methods for handling user requests
//Date              : 27-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using CONTRIBE.BOOKSTORE.BookstoreService;

#endregion

namespace CONTRIBE.BOOKSTORE.Controllers
{
    /// <summary>
    /// HomeController class
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Default action
        /// </summary>
        /// <returns>html view</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Search books asynchronously
        /// </summary>
        /// <param name="searchString">search string in microsoft standard format</param>
        /// <returns>books json object</returns>
        public async Task<ActionResult> SearchBookAsync(string searchString)
        {
            try
            {
                var client = new BookstoreServiceClient();
                var result = await client.GetBooksAsync(searchString);
                return Json(result);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Add book to the cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <returns>book cart json object</returns>
        public async Task<ActionResult> AddBookAsync(string userID, string bookID)
        {
            try
            {
                var client = new BookstoreServiceClient();
                var result = await client.AddBookAsync(userID, bookID, 1);
                return Json(result);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update the quantity in the book cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <param name="quantity">quantity</param>
        /// <returns>book cart json object</returns>
        public async Task<ActionResult> UpdateQuantityAsync(string userID, string bookID, int quantity)
        {
            try
            {
                var client = new BookstoreServiceClient();
                var result = await client.UpdateQuantityAsync(userID, bookID, quantity);
                return Json(result);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Remove book from the cart asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookID">unique book Id</param>
        /// <returns>book cart json object</returns>
        public async Task<ActionResult> RemoveBookAsync(string userID, string bookID)
        {
            try
            {
                var client = new BookstoreServiceClient();
                var result = await client.RemoveBookAsync(userID, bookID);
                return Json(result);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Create order asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <param name="bookCart">book cart object</param>
        /// <returns>json order details object</returns>
        public async Task<ActionResult> CreateOrderAsync(string userID, BookCart bookCart)
        {
            try
            {
                var client = new BookstoreServiceClient();
                var result = await client.CreateOrderAsync(userID, bookCart);
                return Json(result);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get order details asynchronously
        /// </summary>
        /// <param name="userID">unique user Id</param>
        /// <returns>json order details object</returns>
        public async Task<ActionResult> GetOrderDetailsAsync(string userID)
        {
            try
            {
                var client = new BookstoreServiceClient();
                var result = await client.GetOrderDetailsAsync(userID);
                return Json(result);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}