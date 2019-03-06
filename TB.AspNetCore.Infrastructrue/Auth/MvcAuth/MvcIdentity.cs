using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Utils.Cookie;

namespace TB.AspNetCore.Infrastructrue.Auth.MvcAuth
{
    public class MvcIdentity : IIdentity
    {
        protected const string AUTHENTICATION_TYPE = ".NetCoreIdentity";

        public static MvcIdentity Instance => new MvcIdentity();

        [JsonIgnore]
        public string AuthenticationType
        {
            get;
            private set;
        }

        /// <summary>
        /// minutes utc +0800 = -480
        /// </summary>
        [JsonIgnore]
        public int TimezoneOffset
        {
            get
            {
                string cookie = CookieUtility.GetCookie("_Client_Time_Offset_", true);
                if (!string.IsNullOrEmpty(cookie))
                {
                    int.TryParse(cookie.Trim(), out int result);
                    return result;
                }
                return 0;
            }
        }

        [JsonIgnore]
        public bool IsAuthenticated
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    if (!string.IsNullOrWhiteSpace(Id))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        [JsonProperty]
        public string Name
        {
            get;
            protected set;
        }

        [JsonProperty]
        public string Id
        {
            get;
            protected set;
        }

        [JsonProperty]
        public Guid Token
        {
            get;
            protected set;
        }

        [JsonProperty]
        public string UserName
        {
            get;
            protected set;
        }

        [JsonProperty]
        public string Email
        {
            get;
            protected set;
        }

        /// <summary>
        /// employee role id
        /// </summary>
        [JsonProperty]
        public int RoleId
        {
            get;
            protected set;
        }

        [JsonProperty]
        public DateTime? LastLogin
        {
            get;
            protected set;
        }

        public virtual object Data
        {
            get;
            set;
        }

        protected MvcIdentity()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name">can same as UserName</param>
        /// <param name="userName"></param>
        /// <param name="roleId"></param>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <param name="lastLogin"></param>
        public MvcIdentity(string id, string name, string userName, string email = null, int? roleId = default(int?), Guid? token = default(Guid?), DateTime? lastLogin = default(DateTime?), object data = null)
        {
            Id = id;
            Name = name;
            UserName = (userName ?? name);
            RoleId = (roleId ?? (-100));
            Email = email;
            Token = (token ?? Guid.Empty);
            LastLogin = lastLogin;
            if (data != null)
            {
                Data = data;
            }
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(Id))
            {
                AuthenticationType = ".NetCoreIdentity";
            }
        }

        public virtual T GetData<T>()
        {
            if (Data != null)
            {
                return ((JToken)(Data as JContainer)).ToObject<T>();
            }
            return default(T);
        }

        public virtual List<T> GetDataList<T>()
        {
            if (Data != null)
            {
                return ((JToken)(Data as JContainer)).ToObject<List<T>>();
            }
            return new List<T>();
        }

        public MvcPrincipal GetPrincipal()
        {
            if (IsAuthenticated)
            {
                AuthenticationType = ".NetCoreIdentity";
            }
            return new MvcPrincipal(this);
        }

        public void Login(string scheme, Action<CookieOptions> options = null)
        {
            CookieUtility.AppendCookie(scheme, ServiceCollectionExtension.Encrypt(JsonConvert.SerializeObject(this)), true, options);
        }

        public void Logout(string scheme)
        {
            CookieUtility.RemoveCookie(scheme, true, "");
        }
    }
}
