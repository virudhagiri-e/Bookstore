#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : Book.cs
//Purpose           : Contains the properties/methods for handling book business object
//Date              : 24-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

using CONTRIBE.BOOKSTORE.MODEL.Entity.Interface;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Entity
{
    /// <summary>
    /// Book <see langword="class"/>
    /// </summary>
    public class Book : IBook
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Book()
        {
            ID = Guid.NewGuid().ToString("N");
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets ID of the book
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets title of the book
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets author of the book
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets price of the book
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets inStock quantity of book
        /// </summary>
        public int InStock { get; set; }

        #endregion Properties

        #region public Methods

        /// <summary>
        /// To update inStock quantity of book
        /// </summary>
        /// <param name="quantity">inStock quantity</param>
        public void UpdateInStock(int quantity)
        {
            lock (this)
            {
                if (quantity >= 0)
                {
                    InStock = quantity;
                }
            }
        }

        #endregion public Methods
    }
}