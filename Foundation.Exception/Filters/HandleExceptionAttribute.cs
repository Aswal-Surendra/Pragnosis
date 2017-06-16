using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using Foundation.Logging;
using System.Configuration;

namespace Foundation.Exception
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class HandleExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        private ExceptionTypes _rapidExceptionType;
        private ExceptionCategories _rapidExceptionCategory = ExceptionCategories.ERROR;
        public HandleExceptionAttribute(ExceptionTypes type)
        {
            // Errors come here before other error filters
            Order = 1;
            _rapidExceptionType = type;
        }

        public HandleExceptionAttribute(ExceptionTypes type, ExceptionCategories category)
        {
            // Errors come here before other error filters
            Order = 1;
            _rapidExceptionType = type;
            _rapidExceptionCategory = category;
        }

        public virtual void OnException(ExceptionContext filterContext)
        {
            string errorCode = string.Empty;

            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (filterContext.IsChildAction || (filterContext.ExceptionHandled)) return;
            var exception = filterContext.Exception;
            if ((new HttpException(null, exception).GetHttpCode() != 500) ||
                !ExceptionType.IsInstanceOfType(exception)) return;
            // Check if the exception type is custom CTPException
            var foundationException = exception as FoundationException;
            if (foundationException != null)
            {
                var rapidException = new FoundationException(_rapidExceptionType, _rapidExceptionCategory, (System.Exception)exception);
                errorCode = rapidException.ExceptionCode;
                switch (rapidException.ExceptionCategory)
                {

                    case ExceptionCategories.WARNING:
                        LogManager.Warning(rapidException.ExceptionCode);
                        break;
                    case ExceptionCategories.ERROR:
                        LogManager.Exception(rapidException);
                        break;
                    case ExceptionCategories.FATAL:
                        LogManager.Fatal(rapidException);
                        break;
                    default:
                        LogManager.Exception(rapidException);
                        break;
                }
            }
            else
            {
                // Log exception
                LogManager.Exception(exception);
            }
            var errorFilePath = RedirectCustomErrorsView();
            if (!string.IsNullOrEmpty(errorFilePath))
            {
                // Show error view
                ErrorFilterHelper.SetFilerContext(filterContext, Master, View);
                if (System.Web.HttpContext.Current != null)
                    System.Web.HttpContext.Current.Response.RedirectPermanent(String.Format("/Error404", errorCode), true);
            }
            else
            {
                throw exception;
            }
        }

        public Type ExceptionType
        {
            get
            {
                return this._exceptionType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (!typeof(System.Exception).IsAssignableFrom(value))
                {
                    throw new ArgumentException("Non-ExceptionType");
                }
                _exceptionType = value;
            }
        }
        private Type _exceptionType = typeof(System.Exception);

        public string Master
        {
            get
            {
                return (_master ?? string.Empty);
            }
            set
            {
                _master = value;
            }
        }
        private string _master;

        public override object TypeId { get; } = new object();

        public string View
        {
            get
            {
                return string.IsNullOrEmpty(this._view) ? "Error" : _view;
            }
            set
            {
                _view = value;
            }
        }
        private string _view;

        public static string RedirectCustomErrorsView()
        {
            Configuration config = null;
            CustomErrorsSection section = null;
            string path = string.Empty;
            try
            {
                config = WebConfigurationManager.OpenWebConfiguration("~");
                section = (CustomErrorsSection)config.GetSection("system.web/customErrors");

                if (section?.Errors != null && section.Errors.Count > 0)
                {
                    for (var i = 0; i < section.Errors.Count; i++)
                    {
                        if (section.Errors[i].StatusCode == 500)
                        {
                            path = section.Errors[i].Redirect ?? string.Empty;
                            break;
                        }
                    }
                }
                else if (section != null && section.DefaultRedirect != null)
                {
                    path = section.DefaultRedirect;
                }
            }
            finally
            {
                config = null;
                section = null;
            }
            return path;
        }

    }
}
