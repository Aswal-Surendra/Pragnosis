using System;
using System.Web;
using System.Web.Mvc;
using Foundation.Logging;

namespace Foundation.Exception.Filters 
{
   public class HandleExceptionFilter : IExceptionFilter
    {
        private string _view;
        private string _master;
       
        public HandleExceptionFilter( string master, string view)
        {
            _master = master ?? string.Empty;
            _view = view ?? string.Empty;
        }

        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (filterContext.IsChildAction || (filterContext.ExceptionHandled)) return;
            var exception = filterContext.Exception;
            if ((new HttpException(null, exception).GetHttpCode() != 500)) return;
            // Log exception
            LogManager.Exception(exception);
           
            // Show error view
            ErrorFilterHelper.SetFilerContext(filterContext, _master, _view);
        }
    }
}
