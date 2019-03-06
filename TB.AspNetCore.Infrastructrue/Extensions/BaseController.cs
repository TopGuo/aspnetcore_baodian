using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Infrastructrue.Logs;

namespace TB.AspNetCore.Infrastructrue.Extensions
{
    /// <summary>
    /// 此basecontroller为mvc和api封装
    /// </summary>
    public class BaseController:Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            OnCreateProperties(context);
        }

        /// <summary>
        /// 创建控制器内属性实列
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnCreateProperties(ActionExecutingContext context)
        {
            object controller = context.Controller;
            foreach (PropertyInfo declaredProperty in controller.GetType().GetTypeInfo().DeclaredProperties)
            {
                if (declaredProperty.CanWrite)
                {
                    declaredProperty.GetSetMethod(true).Invoke(controller, new object[1]
                    {
                        ActivatorUtilities.GetServiceOrCreateInstance(context.HttpContext.RequestServices, declaredProperty.PropertyType)
                    });
                }
            }
        }


        /// <summary>
        /// 处理api请求我们以json形式返回mvc我们返回view
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.ExceptionHandled = true;
                Log4Net.Error(context.Exception);
                if (context.HttpContext.IsAjaxRequest())
                {
#if DEBUG
                    context.Result = Json(new ResponsResult(context.Exception, true));
#else
                     context.Result = Json(new ResponsResult(context.Exception));
#endif

                }
                else
                {
#if DEBUG
                    context.Result = View("Error", new ResponsResult(context.Exception, true));
#else
                    context.Result = View("Error", new ResponsResult(context.Exception));
#endif
                }
                return;
            }
            base.OnActionExecuted(context);
        }
    }
}
