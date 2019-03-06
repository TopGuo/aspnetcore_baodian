//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using Microsoft.AspNetCore.Mvc.Filters;
//using TB.AspNetCore.Domain.Action;
//using TB.AspNetCore.Domain.Config;
//using TB.AspNetCore.Domain.Contents;
//using TB.AspNetCore.Domain.Entitys;
//using TB.AspNetCore.Domain.Enums;
//using TB.AspNetCore.Domain.Models;
//using TB.AspNetCore.Infrastructrue.Auth.MvcAuth;
//using TB.AspNetCore.Infrastructrue.Extensions;
//using TB.AspNetCore.Infrastructrue.Logs;

//namespace TB.AspNetCore.Infrastructrue.Action
//{
//    public class RegistActions : IRegistActions
//    {
//        public static object locker = new object();

//        static List<MenuModel> _menus = new List<MenuModel>();

//        public static IReadOnlyList<MenuModel> Menus { get { return _menus.AsReadOnly(); } }

//        public static void ReloadMenus()
//        {
//            currentMenus = new Dictionary<int, List<MenuModel>>();
//        }

//        static Dictionary<int, List<MenuModel>> currentMenus = new Dictionary<int, List<MenuModel>>();

//        /// <summary>
//        /// 当前用户权限,菜单
//        /// </summary>
//        public static List<MenuModel> CurrentMenus
//        {
//            get
//            {
//                var identity = (ServiceCollectionExtension.HttpContext.User.Identity as MvcIdentity);
//                if (currentMenus == null)
//                {
//                    return _menus = new List<MenuModel>();
//                }
//                if (!currentMenus.ContainsKey(identity.RoleId) || currentMenus[identity.RoleId].Count == 0)
//                {
//                    lock (locker)
//                    {
//                        if (!currentMenus.ContainsKey(identity.RoleId) || currentMenus[identity.RoleId].Count == 0)
//                        {
//                            var role = PermissionService.RolePermissions.SingleOrDefault(x => x.Id == identity.RoleId);
//                            List<MenuModel> list = new List<MenuModel>();
//                            if (role != null)
//                            {
//                                role.Menus.ForEach(x =>
//                                {
//                                    var menu = Menus.SingleOrDefault(t => t.ActionId == x);
//                                    if (menu == null)
//                                    {
//                                        return;
//                                    }
//                                    list.Add(menu);
//                                    if (string.IsNullOrEmpty(menu.ParentId) && !list.Exists(t => t.ActionId == menu.ParentId))
//                                    {
//                                        list.Add(Menus.SingleOrDefault(t => t.ActionId == menu.ParentId));
//                                    }
//                                });
//                            }
//                            currentMenus[identity.RoleId] = list;
//                        }
//                    }
//                }
//                return currentMenus[identity.RoleId];
//            }
//        }

//        public bool HasPermission(ActionExecutingContext context, string actionId)
//        {
//            var identity = (context.HttpContext.User.Identity as MvcIdentity);
//            if (identity.IsAuthenticated)
//            {
//                var role = PermissionService.RolePermissions.SingleOrDefault(x => x.Id == identity.RoleId);
//                if (role != null && role.Menus.Contains(actionId))
//                {
//                    return true;
//                }
//                if (context.HttpContext.IsAjaxRequest())
//                {
//                    Result result = new Result();
//                    result.SetStatus(ErrorCode.Forbidden);
//                    context.Result = new ObjectResult(result);
//                }
//                else
//                {
//                    var view = new ViewResult();
//                    view.ViewName = "~/Views/Home/Welcome.cshtml";
//                    context.Result = view;
//                }
//                return false;
//            }
//            return true;
//        }

//        public object RegistAction(List<ControllerActionDescriptor> actionDescriptor)
//        {
//            try
//            {
//                var db = ServiceCollectionExtension.New<RestApiContext>();
//                var actionNames = actionDescriptor.Select(t => t.DisplayName).ToList();
//                //TODO:
//                //db.RunSql($"DELETE SystemActions WHERE ActionName  NOT IN ({string.Join(",", actionNames.Select(t => $"'{t.ToString()}'"))})");
//                ParentAtions.ParentAtionsList.ForEach(t =>
//                {
//                    db.SystemActions.Add(new SystemActions { ActionId = t.Id, ActionName = t.Name, ActionDescription = t.Name, Orders = t.Order, Url = t.Url, CreateTime = DateTime.Now });
//                });
//                var actions = db.SystemActions.ToList();
//                foreach (var a in actionDescriptor)
//                {
//                    var dbAction = actions.SingleOrDefault(t => t.ActionName == a.DisplayName);
//                    if (dbAction != null)
//                    {
//                        db.Update(dbAction);
//                    }
//                    else
//                    {
//                        dbAction = new Domain.Entitys.SystemActions
//                        {
//                            ActionId = Guid.NewGuid().ToString("N"),//this.NewSequentialId(),
//                            CreateTime = DateTime.Now,
//                            ActionName = a.DisplayName
//                        };
//                        db.Add(dbAction);
//                    }
//                    a.SetFieldValue("Id", dbAction.ActionId.ToString());
//                    var attr = a.MethodInfo.GetCustomAttribute<ActionAttribute>();
//                    dbAction.ActionDescription = attr.Name;
//                    dbAction.ParentAction = attr.ParentId;
//                    dbAction.Orders = attr.Order;
//                    //TODO:
//                    dbAction.Url = a.ActionName;//a.Url;
//                }


//                db.SaveChanges();
//                _menus = db.SystemActions.Select(t => new MenuModel
//                {
//                    ActionName = t.ActionName,
//                    ActionId = t.ActionId,
//                    ActionDescription = t.ActionDescription,
//                    Url = t.Url,
//                    Orders = t.Orders ?? 0,
//                    ParentId = t.ParentAction
//                }).ToList();
//                _menus.ForEach(t =>
//                {
//                    var p = ParentAtions.ParentAtionsList.SingleOrDefault(x => x.Id == t.ParentId);
//                    if (p != null)
//                    {
//                        t.Parent = p.Name;
//                    }
//                });

//                return _menus;

//            }
//            catch (Exception ex)
//            {
//                Log4Net.Error($"[RegistAction:]{ex}");
//                throw ex;
//            }
//        }

//        public void RegistRole()
//        {
//            var db = ServiceCollectionExtension.New<RestApiContext>();

//            var list1 = JsonExtensions.GetStrings<AccountType>();
//            var pList = db.SystemRolePermission.ToList();
//            var mIds = Menus.Select(t => t.ActionId).ToList();
//            //删除不存在的menu
//            pList.Where(t => !mIds.Contains(t.ActionId)).ToList().ForEach(x =>
//            {
//                db.Remove(x);
//            });
//            foreach (var l in list1)
//            {
//                var p = db.SystemRoles.SingleOrDefault(t => t.Id == l.Item1);
//                if (p == null)
//                {
//                    db.SystemRoles.Add(new SystemRoles { Id = l.Item1, Name = l.Item2 });
//                }
//                else
//                {
//                    db.Update(p);
//                    p.Name = l.Item2;
//                }
//                //超级管理员 赋予所有权限
//                if (l.Item1 == (int)AccountType.Admin)
//                {
//                    foreach (var m in Menus)
//                    {
//                        if (!pList.Any(x => x.RoleId == l.Item1 && x.ActionId == m.ActionId))
//                        {
//                            db.SystemRolePermission.Add(new SystemRolePermission { RoleId = l.Item1, ActionId = m.ActionId, CreateTime = DateTime.Now });
//                        }
//                    }
//                }
//            }
//            db.SaveChanges();
//        }
//    }
//}
