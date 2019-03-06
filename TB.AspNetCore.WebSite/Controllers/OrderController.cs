using System;
using Microsoft.AspNetCore.Mvc;
using TB.AspNetCore.Application.Action;

namespace TB.AspNetCore.WebSite.Controllers
{
    public class OrderController : Controller
    {
        [Action("订单管理", ActionType.CaiwuManager, 2)]
        [Route("Order/Order")]
        public ViewResult Order()
        {
            return View();
        }
    }
}