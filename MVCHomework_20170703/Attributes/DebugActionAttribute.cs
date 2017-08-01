using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace MVCHomework_20170703.Attributes
{
    public class DebugActionAttribute : ActionFilterAttribute
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            sw.Reset();
            sw.Start();

            Debug.WriteLine("{0}.{1}.OnActionExecuting 開始時間 : {2}", controllerName, actionName, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            Debug.WriteLine("{0}.{1}.OnActionExecuted 開始時間 : {2}", controllerName, actionName, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff")); 
            sw.Stop();
            Debug.WriteLine("{0}.{1}.Action 執行時間 : {2}", controllerName, actionName, sw.Elapsed.TotalMilliseconds.ToString());
             
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string actionName = filterContext.RouteData.Values["action"].ToString();
            string controllerName = filterContext.RouteData.Values["controller"].ToString();

            sw.Reset();
            sw.Start();

            Debug.WriteLine("{0}.{1}.OnResultExecuting 開始時間 : {2}", controllerName, actionName, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff"));

            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            string actionName = filterContext.RouteData.Values["action"].ToString();
            string controllerName = filterContext.RouteData.Values["controller"].ToString();

            Debug.WriteLine("{0}.{1}.OnResultExecuted 開始時間 : {2}", controllerName, actionName, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff")); 
            sw.Stop();
            Debug.WriteLine("{0}.{1}.ActionResult 執行時間 : {2}", controllerName, actionName, sw.Elapsed.TotalMilliseconds.ToString());

            base.OnResultExecuted(filterContext);
        }
    }
}