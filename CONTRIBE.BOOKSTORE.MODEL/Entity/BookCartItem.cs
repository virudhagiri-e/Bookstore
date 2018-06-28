#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : BookCartItem.cs
//Purpose           : Contains the properties/methods for handling book cart item business object
//Date              : 24-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"

using CONTRIBE.BOOKSTORE.MODEL.Entity.Interface;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Entity
{
    /// <summary>
    /// BookCartItem <see langword="class"/>
    /// </summary>
    public class BookCartItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets book object
        /// </summary>
        public Book Book { get; set; }

        /// <summary>
        /// Gets or sets quantity of book
        /// </summary>
        public int Quantity { get; set; }

        #endregion Properties
    }
}