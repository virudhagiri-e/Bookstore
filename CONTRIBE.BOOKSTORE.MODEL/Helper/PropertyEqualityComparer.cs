#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name			: PropertyEqualityComparer.cs
//Purpose			: Serves as Equality comparer for custom types property
//Date				: 24-June-2018
//Written By		: Giri
//Modified			: 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "Imports"

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Helper
{
    /// <summary>
    /// PropertyEqualityComparer class
    /// </summary>
    /// <typeparam name="T">type T whom which comparision performed</typeparam>
    public class PropertyEqualityComparer<T> : IEqualityComparer<T>
    {
        #region Private Fields

        private PropertyInfo propertyInfo;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PropertyEqualityComparer.
        /// </summary>
        /// <param name="propertyName">The name of the property on type T to perform the comparison on.</param>
        public PropertyEqualityComparer(string propertyName)
        {
            propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.", propertyName, typeof(T)));
            }
        }

        #endregion

        #region IEqualityComparer<T> Interface Implementation

        public bool Equals(T object1, T object2)
        {
            //get the current value of the comparison property of x and of y
            object object1Value = propertyInfo.GetValue(object1, null);
            object object2Value = propertyInfo.GetValue(object2, null);

            //if the object1Value is null then we consider them equal if and only if object2Value is null
            if (object1Value == null)
            {
                return object2Value == null;
            }

            //use the default comparer for whatever type the comparison property is.
            return object1Value.Equals(object2Value);
        }

        public int GetHashCode(T obj)
        {
            //get the value of the comparison property out of obj
            object propertyValue = propertyInfo.GetValue(obj, null);
            return (propertyValue == null) ? 0 : propertyValue.GetHashCode();
        }

        #endregion
    }
}