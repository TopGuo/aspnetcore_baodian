using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TB.AspNetCore.WebSite.Controllers
{
    public class RegisterController : WebBase
    {
        [AllowAnonymous]
        [Route("Register/{id?}")]
        public ViewResult Register(int id)
        {
            return View();
        }
    }
}