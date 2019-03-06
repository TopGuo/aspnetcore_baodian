using Microsoft.AspNetCore.Mvc;
using TB.AspNetCore.Application.Action;
using TB.AspNetCore.Domain.Models;

namespace TB.AspNetCore.WebSite.Controllers
{
    public class MemberController : WebBase
    {
        [Action("操作员管理", ActionType.SystemManager, 2)]
        public ViewResult BackstageUser(AccountViewModel model)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MenuAction("MemberController.BackstageUser(AccountViewModel)")]
        public ViewResult AddMember(string id = "0")
        {
            if (id.Equals("0"))
            {
                ViewBag.title = "添加管理员";
            }
            else
            {
                ViewBag.title = "修改管理员";
            }
            return View();
        }
    }
}