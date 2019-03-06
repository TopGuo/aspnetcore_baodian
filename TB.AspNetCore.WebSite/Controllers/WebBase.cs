using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Linq;
using System.Reflection;
using TB.AspNetCore.Application.Action;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Auth.MvcAuth;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Logs;
using TB.AspNetCore.Infrastructrue.Utils.Cookie;

namespace TB.AspNetCore.WebSite.Controllers
{
    /// <summary>
    /// 网站继承该 contoller
    /// </summary>
    [MvcAuthorize(AuthenticationSchemes = TbConstant.WEBSITE_AUTHENTICATION_SCHEME)]
    public class WebBase : BaseController
    {
        static WebBase()
        {
            try
            {
                var actions = ServiceCollectionExtension.Get<IPermissionService>();
                
                if (actions != null)
                {
                    var provider = ServiceCollectionExtension.Get<IActionDescriptorCollectionProvider>();
                    var descriptorList = provider.ActionDescriptors.Items.Cast<ControllerActionDescriptor>()
                        .Where(t => t.MethodInfo.GetCustomAttribute<ActionAttribute>() != null).ToList();
                    actions.RegistAction(descriptorList);

                    actions.RegistRole();
                }
            }
            catch (Exception ex)
            {
                Log4Net.Error(ex);
            }
        }

        //==========
        #region auto log in
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var provider = ServiceCollectionExtension.Get<IActionDescriptorCollectionProvider>();
            var desc1 = (context.ActionDescriptor as ControllerActionDescriptor);
            var desc2 = provider.ActionDescriptors.Items.Cast<ControllerActionDescriptor>()
                .Where(t => t.MethodInfo.GetCustomAttribute<ActionAttribute>() != null&&t.DisplayName== desc1.DisplayName).FirstOrDefault();
            var desc3 = desc2 ?? desc1;
            var action = desc3.MethodInfo.GetCustomAttribute<ActionAttribute>();
            if (action != null)
            {
                var actions = ServiceCollectionExtension.Get<IPermissionService>();
                if (actions != null && !actions.HasPermission(context, desc3.Id))
                {
                    return;
                }

            }

            if (desc3.ActionName == "Index" && desc3.ControllerName == "Home")
            {
                if (User.Identity.IsAuthenticated)
                {
                    string path = HttpContext.Request.Query["from"];
                    if (string.IsNullOrEmpty(path))
                    {
                        path = CookieUtility.GetCookie(TbConstant.LAST_LOGIN_PATH);
                    }
                    if (!string.IsNullOrEmpty(path) && path != "/")
                    {
                        context.Result = Redirect(path);
                    }
                }
            }
            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                base.OnActionExecuted(context);
                return;
            }
            var action = context.ActionDescriptor as ControllerActionDescriptor;
            if (!context.HttpContext.IsAjaxRequest() && !action.ActionName.Equals("Index", StringComparison.OrdinalIgnoreCase) && !action.ControllerName.Equals("webapi", StringComparison.OrdinalIgnoreCase))
            {
                CookieUtility.AppendCookie(TbConstant.LAST_LOGIN_PATH, HttpContext.Request.Path);
            }
            base.OnActionExecuted(context);
        }
        #endregion
    }
}
