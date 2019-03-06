using System;
using System.Diagnostics;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.AspNetCore.Application.Action;
using TB.AspNetCore.Application.Services;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Auth;
using TB.AspNetCore.Infrastructrue.Auth.MvcAuth;
using TB.AspNetCore.Infrastructrue.Config;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Utils.Code;
using TB.AspNetCore.Infrastructrue.Utils.Cookie;
using TB.AspNetCore.WebSite.Models;

namespace TB.AspNetCore.WebSite.Controllers
{
    public class HomeController : WebBase
    {
        [AllowAnonymous]
        public ActionResult Index()
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
                    return Redirect(System.Web.HttpUtility.UrlDecode(path));
                }
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult ValidateCode()
        {
            ValidateCode _vierificationCodeServices = new ValidateCode();
            string code = "";
            System.IO.MemoryStream ms = _vierificationCodeServices.Create(out code);
            CookieUtility.AppendCookie(TbConstant.WEBSITE_VERIFICATION_CODE, ServiceCollectionExtension.Encrypt(code));
            return File(ms.ToArray(), @"image/png");
        }

        [Route("Welcome")]
        public ViewResult Welcome()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("Denied")]
        public ViewResult Denied()
        {
            return View();
        }

        [Action("角色管理", ActionType.SystemManager, 1)]
        public ViewResult Roles()
        {
            var result = PermissionService.Menus;
            return View(result);
        }
    }
}
