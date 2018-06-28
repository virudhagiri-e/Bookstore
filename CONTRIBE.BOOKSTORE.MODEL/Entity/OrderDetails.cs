#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : OrderDetails.cs
//Purpose           : Contains the properties/methods for handling Order details business object
//Date              : 24-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CONTRIBE.BOOKSTORE.MODEL.Entity.Interface;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Entity
{
    /// <summary>
    /// OrderDetails <see langword="class"/>
    /// </summary>
    public class OrderDetails
    {
        #region Properties

        /// <summary>
        /// Gets or sets purchased book list
        /// </summary>
        public List<BookCartItem> PurchasedBooks { get; set; }

        /// <summary>
        /// Gets or sets not inStock books
        /// </summary>
        public List<BookCartItem> NotInStockBooks { get; set; }

        /// <summary>
        /// Gets or sets total price
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets total quantity
        /// </summary>
        public int TotalQuantity { get; set; }

        #endregion Properties
    }
}