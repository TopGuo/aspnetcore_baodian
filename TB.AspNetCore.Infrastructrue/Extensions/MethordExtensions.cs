using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TB.AspNetCore.Infrastructrue.Extensions
{
    public static class MethordExtensions
    {

        /// <summary>
        /// get value from RouteData,QueryString,Form,Cookie
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Params(this HttpContext context, string key)
        {
            string value = string.Empty;
            var help = context.Items.SingleOrDefault(t => t.Value is UrlHelper).Value as UrlHelper;
            if (help != null)
            {
                var route = help.ActionContext.RouteData.Values[key];
                if (route != null)
                {
                    return route.ToString();
                }
            }

            if (context.Request.Query.ContainsKey(key))
            {
                value = context.Request.Query[key];
            }
            else if (context.Request.HasFormContentType && context.Request.Form.ContainsKey(key))
            {
                value = context.Request.Form[key];
            }
            else if (context.Request.Cookies.Count > 0 && context.Request.Cookies.ContainsKey(key))
            {
                value = context.Request.Cookies[key];
            }

            return value;
        }

        public static string Params(this RazorPage page, string key)
        {
            return page.Context.Params(key);
        }
        public static string Params(this ControllerBase controller, string key)
        {
            return controller.HttpContext.Params(key);
        }
        //
        /// <summary>
        /// 生成图片/Web全路径
        /// </summary>
        /// <param name="picturePath"></param>
        /// <param name="baseUrl"></param>
        /// <param name="defaultPictrue"></param>
        /// <returns></returns>
        public static string GetPWFullPath(this string picturePath, string baseUrl, string defaultPictrue = "")
        {
            if (!string.IsNullOrEmpty(picturePath))
            {
                try
                {
                    return string.Format("{0}{1}", baseUrl, picturePath);
                }
                catch
                {
                    return defaultPictrue;
                }
            }
            return defaultPictrue;
        }

        /// <summary>
        /// 生成图片/Web全路径 Lists
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static List<string> GetPwFullPath(this List<string> lists, string baseUrl)
        {
            List<string> newLists = new List<string>();
            lists.ForEach(t =>
            {
                newLists.Add(t.GetPWFullPath(baseUrl));
            });
            return newLists;
        }
        #region 生成yyyyMMddhhmmss+guid
        /// <summary>
        ///  生成yyyyMMddhhmmss+guid
        /// </summary>
        /// <param name="obj">never use it</param>
        /// <returns></returns>
        public static string GenNewGuid(this object obj)
        {
            var orderdate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var ordercode = Guid.NewGuid().GetHashCode();
            if (ordercode < 0) { ordercode = -ordercode; }
            var orderlast = ordercode.ToString().Length > 11 ? ordercode.ToString().Substring(0, 11) : ordercode.ToString().PadLeft(11, '0');
            return $"{orderdate}{orderlast}";
        }
        #endregion
        /// <summary>
        /// 设置/修改字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFieldValue<T>(this object obj, string fieldName, T value)
        {
            var type = obj.GetType();
            FieldInfo field = null;
            while (type != null)
            {
                field = type.GetTypeInfo().DeclaredFields.SingleOrDefault(t => t.Name.IndexOf($"<{fieldName}>", StringComparison.OrdinalIgnoreCase) > -1);
                if (field != null)
                {
                    break;
                }
                else
                {
                    type = type.GetTypeInfo().BaseType;
                }
            }
            if (field != null)
            {
                field.SetValue(obj, value);
            }
        }

        /// <summary>
        /// 是否为有效的手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(this string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return false;
            }
            return Regex.IsMatch(mobile, "^1[0-9]{10}$");//^(13[0-9]|15[012356789]|18[0-9]|14[579]|17[135678])[0-9]{8}$//^1[0-9]{10}
        }

        static readonly string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        /// <summary>
        /// 是否为有效的身份证号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsIDCard(this string id)
        {
            long n = 0;
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            if (id.Length != 18)
            {
                return false;
            }
            if (long.TryParse(id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            if (address.IndexOf(id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = sum % 11;
            if (arrVarifyCode[y] != id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }
    }
}
