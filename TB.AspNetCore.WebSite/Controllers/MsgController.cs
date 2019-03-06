using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TB.AspNetCore.Application.Action;

namespace TB.AspNetCore.WebSite.Controllers
{
    public class MsgController : Controller
    {
        [Action("消息管理", ActionType.ContentManager, 2)]
        [Route("Msg/Msg")]
        public ViewResult Msg()
        {
            return View();
        }
    }
}