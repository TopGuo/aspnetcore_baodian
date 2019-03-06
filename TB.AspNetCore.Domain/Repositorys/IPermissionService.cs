using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using TB.AspNetCore.Domain.Action;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Models;

namespace TB.AspNetCore.Domain.Repositorys
{
    public interface IPermissionService
    {
        /// <summary>
        /// 
        /// </summary>
        List<MenuModel> CurrentMenus { get; }
        /// <summary>
        /// 
        /// </summary>
        IReadOnlyList<RoleModel> RolePermissions { get; }
        /// <summary>
        /// 注册Action
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        object RegistAction(List<ControllerActionDescriptor> actionDescriptor);
        /// <summary>
        /// 注册Role
        /// </summary>
        void RegistRole();
        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        bool HasPermission(ActionExecutingContext context, string actionId);
        /// <summary>
        /// 获取角色所拥有的权限
        /// </summary>
        /// <returns></returns>
        ResponsResult GetRoles();
        /// <summary>
        /// 删除指定角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult DeleteRoles(RoleModel model);
        /// <summary>
        /// 保存,添加,修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult SaveRoles(RoleModel model);
    }
}
