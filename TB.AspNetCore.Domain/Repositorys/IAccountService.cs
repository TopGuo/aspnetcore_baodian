using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Models;
using TB.AspNetCore.Domain.Models.Web;

namespace TB.AspNetCore.Domain.Repositorys
{
    public interface IAccountService
    {
        ResponsResult Login(BackstageUserAdd model);

        ResponsResult UpdateAccount(BackstageUserAdd model);
        /// <summary>
        /// 添加后台管理员
        /// </summary>
        /// <param name="model">AccountAdd</param>
        /// <returns></returns>
        ResponsResult AddAccount(BackstageUserAdd model);

        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult UpdatePwd(BackstageUserAdd model);

        /// <summary>
        /// 获取后台用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponsResult GetBackstageUser(string id);
        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult GetBackstageUserList(AccountSearchModel model);

        /// <summary>
        /// 获取后台用户登录Cookie信息
        /// </summary>
        /// <returns></returns>
        BackstageCookie GetUserCook();
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        ResponsResult LogoutUser();

    }
}
