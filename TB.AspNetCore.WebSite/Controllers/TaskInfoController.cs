using Microsoft.AspNetCore.Mvc;
using TB.AspNetCore.Application.Action;

namespace TB.AspNetCore.WebSite.Controllers
{
    public class TaskInfoController : WebBase
    {
        //
        [Action("任务管理", ActionType.SystemManager, 3)]
        public ViewResult Index()
        {
            return View();
        }
    }
}