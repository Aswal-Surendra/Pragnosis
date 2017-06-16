using System;
using System.Runtime.Serialization;

namespace Foundation.Exception
{
    /// <summary>
    /// Custom exception class inherited from the base Exception class for the application.
    /// This exception class can be used to throw custom exceptions based on the passed
    /// RapidExceptionType enum value and based on this paramter defined in the overloaded
    /// constructor the custom error message will be generated. This class will also carry 
    /// the original exception object, so that we can log the actual exception and stack trace.
    /// </summary>

    public class FoundationException : System.Exception, ISerializable
    {
        #region Private Variables
        // Local variable to store the custom exception message

        #endregion

        #region Public Properties
        /// <summary>
        /// Property to get the custom exception message
        /// </summary>
        public string ExceptionCode { get; } = string.Empty;

        /// <summary>
        /// Property to get the custom exception type
        /// </summary>
        public ExceptionTypes ExceptionType { get; }

        /// <summary>
        /// Property to get the custom exception category
        /// </summary>
        public ExceptionCategories ExceptionCategory { get; } = ExceptionCategories.ERROR;

        /// <summary>
        /// Property to get the original exception
        /// </summary>
        public System.Exception OriginalException { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default overloaded constructor to set the custom exception message
        /// based on the passed exception type.
        /// </summary>
        /// <param name="type">Type of exception of type RapidExceptionType enum</param>
        public FoundationException(ExceptionTypes type)
        {
            ExceptionCode = GetExceptionCode(type);
            ExceptionType = type;
        }

        /// <summary>
        /// Default overloaded constructor to set the custom exception message
        /// based on the passed exception type.
        /// </summary>
        /// <param name="type">Type of exception of type RapidExceptionType enum</param>
        /// <param name="category">Category of exception</param>
        public FoundationException(ExceptionTypes type, ExceptionCategories category)
        {
            ExceptionCode = GetExceptionCode(type);
            ExceptionType = type;
            ExceptionCategory = category;
        }

        /// <summary>
        /// Default overloaded constructor to set the custom exception message
        /// based on the passed exception type.
        /// </summary>
        /// <param name="type">Type of exception of type RapidExceptionType enum</param>
        /// <param name="category">Category of exception</param>
        public FoundationException(ExceptionTypes type, ExceptionCategories category, System.Exception ex)
        {
            ExceptionCode = GetExceptionCode(type);
            ExceptionType = type;
            ExceptionCategory = category;
            OriginalException = ex;
        }

        public FoundationException() { }
        public FoundationException(string message) : base(message) { }
        public FoundationException(string message, FoundationException inner) : base(message, inner) { }
        protected FoundationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Method to return the custom error message based on the passed
        /// RapidExceptionType enum value.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetExceptionCode(ExceptionTypes type)
        {
            return Convert.ToString(((int)ExceptionTypes.ERROR_RETRIEVING_DATA));
        }
        #endregion

    }
}
