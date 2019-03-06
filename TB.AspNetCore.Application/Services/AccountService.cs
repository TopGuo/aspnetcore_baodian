using System;
using System.Collections.Generic;
using System.Linq;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Entitys;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models;
using TB.AspNetCore.Domain.Models.Web;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Auth.MvcAuth;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Logs;
using TB.AspNetCore.Infrastructrue.Utils.Cookie;
using TB.AspNetCore.Infrastructrue.Utils.Encryption;

namespace TB.AspNetCore.Application.Services
{
    public class AccountService:BaseService, IAccountService
    {
        /// <summary>
        /// 后端用户登录操作
        /// </summary>
        /// <param name="model">AccountViewModel实体</param>  
        /// <returns></returns>
        public ResponsResult Login(BackstageUserAdd model)
        {
            ResponsResult result = new ResponsResult();
            string sessionCode = string.Empty;
            try
            {
                var code = CookieUtility.GetCookie(TbConstant.WEBSITE_VERIFICATION_CODE);
                if (code != null)
                {
                    sessionCode = ServiceCollectionExtension.Decrypt(code);
                }
            }
            catch (Exception ex)
            {
                Log4Net.Debug(ex);
            }
            if (model.ErrCount >= 3)
            {
                if (!model.VerCode.ToString().ToLower().Equals(sessionCode.ToLower()))
                {
                    return result.SetStatus(ErrorCode.NotFound, "验证码输入不正确！");
                }
            }

            BackstageUser account = this.First<BackstageUser>(t => t.LoginName == model.LoginName);
            if (account == null)
            {
                return result.SetStatus(ErrorCode.NotFound, "账号不存在！");
            }
            string pwd = Security.MD5(model.Password);
            if (!account.Password.Equals(pwd, StringComparison.OrdinalIgnoreCase))
            {
                return result.SetStatus(ErrorCode.InvalidPassword);
            }
            switch (account.AccountStatus)
            {
                case (int)AccountStatus.Disabled:
                    return result.SetStatus(ErrorCode.AccountDisabled, "账号不可用！");
            }

            account.LastLoginTime = DateTime.Now;
            account.LastLoginIp = "";
            this.Update(account, true);
            MvcIdentity identity = new MvcIdentity(account.Id, account.LoginName, account.LoginName, account.Email, (int)account.AccountType, null, account.LastLoginTime);
            identity.Login(TbConstant.WEBSITE_AUTHENTICATION_SCHEME, x =>
            {
                x.Expires = DateTime.Now.AddHours(25);//滑动过期时间
                x.HttpOnly = true;
            });

            return result;
        }

        /// <summary>
        /// 添加后台管理员
        /// </summary>
        /// <param name="model">AccountAdd</param>
        /// <returns></returns>
        public ResponsResult AddAccount(BackstageUserAdd model)
        {
            ResponsResult result = new ResponsResult();
            BackstageUser account = this.First<BackstageUser>(t => t.LoginName == model.LoginName);
            if (account != null)
            {
                return result.SetStatus(ErrorCode.NotFound, "登录名称已经存在！");
            }
            else
            {
                account = new BackstageUser();
            }
            if (!MethordExtensions.IsIDCard(model.IdCard))
            {
                return result.SetStatus(ErrorCode.InvalidData, "身份证非法！");
            }
            model.Password = model.Password == "" ? "123456" : model.Password;
            string pwd = Security.MD5(model.Password);
            account.Id = Guid.NewGuid().ToString("N");
            account.LoginName = model.LoginName;
            account.FullName = model.FullName;
            account.CreateTime = DateTime.Now;
            account.AccountType = (int)model.AccountType;
            account.RoleId= (int)model.AccountType;
            account.Password = pwd;
            account.Mobile = model.Mobile;
            account.AccountStatus = (int)AccountStatus.Normal;
            account.SourceType = (int)SourceType.Web;
            account.Gender = model.Gender;
            account.IdCard = model.IdCard;
            this.Add(account, true);
            return result;
        }
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponsResult UpdatePwd(BackstageUserAdd model)
        {
            ResponsResult result = new ResponsResult();
            BackstageCookie backUser = GetUserCook();
            BackstageUser backstageModel = this.First<BackstageUser>(t => t.Id == backUser.Id);
            if (backstageModel == null)
            {
                return result.SetStatus(ErrorCode.NotFound, "登录名称不存在！");
            }
            string pwd = Security.MD5(model.OldPassword);
            if (pwd.Equals(backstageModel.Password))
            {
                string pwdNew = Security.MD5(model.ConfirmPassword);
                backstageModel.Password = pwdNew;
            }
            else
            {
                return result.SetStatus(ErrorCode.NotFound, "您输入的密码不正确！");
            }
            this.Update(backstageModel, true);
            return result;
        }
        /// <summary>
        /// 后台管理员修改
        /// </summary>
        /// <param name="model">BackstageUserAdd</param>
        /// <returns></returns>
        public ResponsResult UpdateAccount(BackstageUserAdd model)
        {
            ResponsResult result = new ResponsResult();
            BackstageUser account = base.First<BackstageUser>(t => string.IsNullOrEmpty(model.Id) && t.LoginName.Equals(model.LoginName));
            if (account != null)
            {
                return result.SetStatus(ErrorCode.NotFound, "登录名称已经存在！");
            }
            else
            {
                account = this.First<BackstageUser>(t => t.Id.Equals(model.Id));
                if (account == null)
                {
                    return result.SetStatus(ErrorCode.NotFound, "用户异常操作失败！");
                }
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                string pwd = Security.MD5(model.Password);
                account.Password = pwd;
            }
            if (!MethordExtensions.IsIDCard(model.IdCard))
            {
                return result.SetStatus(ErrorCode.InvalidData, "身份证非法！");
            }
            account.LoginName = model.LoginName;
            account.AccountStatus = (int)model.AccountStatus;
            account.FullName = model.FullName;
            account.RoleId = (int)model.AccountType;
            account.Mobile = model.Mobile;
            account.UpdateTime = DateTime.Now;
            account.Gender = model.Gender;
            account.AccountType = (int)model.AccountType;
            account.IdCard = model.IdCard;
            this.Update(account, true);
            return result;
        }
        /// <summary>
        /// 获取后台用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponsResult GetBackstageUser(string id)
        {
            ResponsResult result = new ResponsResult();
            BackstageUser backstageUser = this.First<BackstageUser>(t => t.Id == id);
            if (backstageUser == null)
            {
                backstageUser = new BackstageUser();
            }
            else
            {
                backstageUser.Password = "";
            }
            result.Data = backstageUser;
            return result;
        }
        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponsResult GetBackstageUserList(AccountSearchModel model)
        {
            ResponsResult result = new ResponsResult();
            var query = base.Query<BackstageUser>();

            if (!string.IsNullOrEmpty(model.FullName))
            {
                query = query.Where(t => t.FullName.Contains(model.FullName));
            }
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                query = query.Where(t => t.Mobile == model.Mobile);
            }
            if (model.AccountStatus != null && (int)model.AccountStatus > 0)
            {
                query = query.Where(t => t.AccountStatus == (int)model.AccountStatus);
            }
            if (model.BeginTime.HasValue)
            {
                query = query.Where(t => t.CreateTime >= model.BeginTime);
            }
            if (model.EndTime.HasValue)
            {
                query = query.Where(t => t.CreateTime <= model.EndTime);
            }
            var objList = query.OrderByDescending(t => t.CreateTime).Pages(model.PageIndex, model.PageSize, out int count).
                Select(t => new
                {
                    t.Id,
                    roleName = t.Role.Name,
                    t.AccountStatus,
                    t.FullName,
                    t.SourceType,
                    t.CreateTime,
                    t.LastLoginTime,
                    t.LastLoginIp,
                    t.Mobile,
                    t.LoginName,
                    t.UpdateTime,
                    t.IdCard,
                    t.AccountType
                }).ToList();
            List<AccountSearchModel> _list = new List<AccountSearchModel>();
            objList.ForEach(t => _list.Add(
                new AccountSearchModel
                {
                    Id = t.Id,
                    RoleName = t.roleName,
                    AccountStatusName = ((AccountStatus)t.AccountStatus).GetString(),
                    FullName = t.FullName,
                    SourceTypeName = ((SourceType)t.SourceType).GetString(),
                    CreateTime = t.CreateTime,
                    LastLoginTime = t.LastLoginTime,
                    LastLoginIp = t.LastLoginIp,
                    Mobile = t.Mobile,
                    LoginName = t.LoginName,
                    UpdateTime = t.UpdateTime,
                    AccountStatus=(AccountStatus)t.AccountStatus,
                    IdCard= t.IdCard,
                    AccountType=(AccountType)t.AccountType
                }
                ));
            result.Data = _list;
            result.RecordCount = count;
            return result;
        }

        /// <summary>
        /// 获取后台用户登录Cookie信息
        /// </summary>
        /// <returns></returns>
        public BackstageCookie GetUserCook()
        {
            string cookie = ServiceCollectionExtension.Decrypt(CookieUtility.GetCookie(TbConstant.WEBSITE_AUTHENTICATION_SCHEME));
            BackstageCookie back = new BackstageCookie();
            back = cookie.GetModel<BackstageCookie>();
            return back;
        }
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        public ResponsResult LogoutUser()
        {
            ResponsResult result = new ResponsResult();
            CookieUtility.RemoveCookie(TbConstant.WEBSITE_AUTHENTICATION_SCHEME);
            return result;
        }
    }
}
