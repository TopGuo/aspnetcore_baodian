using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Action
{
    public interface IRegistActions
    {
        object RegistAction(List<ControllerActionDescriptor> actionDescriptor);
        void RegistRole();
        bool HasPermission(ActionExecutingContext context, string actionId);
    }
}
