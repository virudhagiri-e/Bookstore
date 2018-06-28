#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : IBook.cs
//Purpose           : Contains the properties/methods definition for book business object
//Date              : 24-June-2018
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "References"


#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Entity.Interface
{
    /// <summary>
    /// IBook <see langword="interface"/>
    /// </summary>
    public interface IBook
    {
        #region Properties

        /// <summary>
        /// Gets title of the book
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets author of the book
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets price of the book
        /// </summary>
        decimal Price { get; }

        #endregion Properties
    }
}
