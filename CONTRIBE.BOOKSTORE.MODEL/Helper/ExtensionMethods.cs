#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name         : ExtensionMethods.cs
//Purpose           : Contains the extension methods
//Date              : 24-06-2016
//Written By        : Giri
//Modified          : 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Helper
{
    /// <summary>
    /// ExtensionMethods static class
    /// </summary>
    public static class ExtensionMethods
    {
        #region Public Methods

        /// <summary>
        /// Emulate .ForEach on collection
        /// </summary>
        /// <typeparam name="T">list type</typeparam>
        /// <param name="collection">list</param>
        /// <param name="action">action</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var tObject in collection)
            {
                action(tObject);
            }
        }

        /// <summary>
        /// Finds whether a collection source has particular value
        /// </summary>
        /// <typeparam name="TSource">list type</typeparam>
        /// <param name="source">source</param>
        /// <param name="predicate">predicate</param>
        /// <returns>boolean</returns>
        public static bool Contains<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                return false;
            }

            return source.FirstOrDefault(predicate) != null;
        }

        /// <summary>
        /// Determines whether an IEnumerable is empty or not.
        /// 
        /// In many places we access IEnumerable.Count() for the sole purpose of seeing if there are any
        /// items in the IEnumerable.  We aren't even interested in the total count.  That's potentially very expensive
        /// and this should improve the performance in those instances.
        /// </summary>
        /// <typeparam name="TSource">Type contained in source</typeparam>
        /// <param name="source">IEnumerable of type TSource</param>
        /// <returns>True if null or empty, otherwise false</returns>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return true;
            }

            foreach (TSource item in source)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether an IEnumerable is empty or not.
        /// 
        /// In many places we access IEnumerable.Count() for the sole purpose of seeing if there are any
        /// items in the IEnumerable.  We aren't even interested in the total count.  That's potentially very expensive
        /// and this should improve the performance in those instances.
        /// </summary>
        /// <typeparam name="TSource">Type contained in source</typeparam>
        /// <param name="source">IEnumerable of type TSource</param>
        /// <param name="condition">Predicate used to calculate result set</param>
        /// <returns>True if null or empty, otherwise false</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> source, Predicate<T> condition)
        {
            if (source == null)
            {
                return true;
            }

            var matches = from s in source
                          where condition(s)
                          select s;

            foreach (T item in matches)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether an IEnumerable is not empty or not.
        /// 
        /// In many places we access IEnumerable.Count() for the sole purpose of seeing if there are any
        /// items in the IEnumerable.  We aren't even interested in the total count.  That's potentially very expensive
        /// and this should improve the performance in those instances.
        /// </summary>
        /// <typeparam name="TSource">Type contained in source</typeparam>
        /// <param name="source">IEnumerable of type TSource</param>
        /// <returns>True if not null and not empty, otherwise false</returns>
        public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return !source.IsEmpty();
        }

        /// <summary>
        /// Checks and returns whether the input value is null or empty
        /// </summary>
        /// <param name="value">input string</param>
        /// <returns>boolean</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value.SafeTrim());
        }

        /// <summary>
        /// Trims the input string
        /// </summary>
        /// <param name="value">input string</param>
        /// <returns>trimmed string</returns>
        public static string SafeTrim(this string value)
        {
            string tempValue = string.Empty;
            if (value != null)
            {
                tempValue = value.Trim();
            }
            return tempValue;
        }

        /// <summary>
        /// returns the input string in upper case
        /// </summary>
        /// <param name="value">input string</param>
        /// <returns>trimmed string</returns>
        public static string SafeToUpper(this string value)
        {
            string tempValue = string.Empty;
            if (value != null)
            {
                tempValue = value.ToUpper();
            }
            return tempValue;
        }

        /// <summary>
        /// Serializes given object for debugging purpose
        /// </summary>
        /// <param name="objectToBeSerialized">object to be serialized</param>
        public static void WriteXml(this object objectToBeSerialized)
        {
            try
            {
                using (var writer = new System.IO.StreamWriter(string.Format(@"C:\Test\{0}.xml", objectToBeSerialized.GetType().Name), true))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objectToBeSerialized.GetType());
                    serializer.Serialize(writer, objectToBeSerialized);
                }
            }
            catch { }
        }

        /// <summary>
        /// Serializes this object
        /// </summary>
        /// <param name="objectToBeSerialized">object to be serialized</param>
        /// <param name="indentation">indentation boolean</param>
        /// <returns>serialized input object's string representation</returns>
        public static string GetSerializedObject(this object objectToBeSerialized, bool indentation)
        {
            try
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(objectToBeSerialized.GetType());

                using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
                {
                    System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings
                    {
                        Encoding = new UTF8Encoding(false),
                        Indent = indentation,
                        OmitXmlDeclaration = true
                    };

                    using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                    {
                        System.Xml.Serialization.XmlSerializerNamespaces nameSpace = new System.Xml.Serialization.XmlSerializerNamespaces();
                        nameSpace.Add("", "");

                        serializer.Serialize(xmlWriter, objectToBeSerialized, nameSpace);
                    }
                    return textWriter.ToString();
                }
            }
            catch { }

            return "";
        }

        #endregion
    }
}