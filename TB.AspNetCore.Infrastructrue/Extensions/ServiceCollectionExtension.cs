using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.DataProtection;
using TB.AspNetCore.Infrastructrue.Auth;
using TB.AspNetCore.Infrastructrue.Auth.MvcAuth;
using TB.AspNetCore.Infrastructrue.Config;
using TB.AspNetCore.Infrastructrue.Logs;

namespace TB.AspNetCore.Infrastructrue.Extensions
{
    public static class ServiceCollectionExtension
    {
        private static IHttpContextAccessor _httpContextAccessor;

        private static IServiceProvider _serviceProvider;
        /// <summary>
        /// cerf weige
        /// </summary>
        private static IDataProtector Protector => ServiceProvider.GetDataProtector("AspNetCore", Array.Empty<string>());


        /// <summary>
        /// 注册常用服务
        /// </summary>
        /// <param name="service"></param>
        public static IServiceCollection AddRegisterContainer(this IServiceCollection services)
        {
            //注入httpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //注入配置文件获取服务
            services.AddSingleton<IConfigGeter, ConfigGeter>();
            return services;
        }

        //
        /// <summary>
        /// 创建自定义AddMvc
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mvcOptions"></param>
        /// <returns></returns>
        public static IMvcBuilder AddMvcCustomer(this IServiceCollection services, Action<MvcApplicationOptions> mvcOptions = null)
        {
            ServiceCollectionDescriptorExtensions.Replace(services, ServiceDescriptor.Singleton<IFilterProvider, MvcFilterProvider>());

            var result= services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(_mvcJsonOptions =>
            {
                _mvcJsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                _mvcJsonOptions.SerializerSettings.DateFormatString = "yyyy-MM-d HH:mm";
                _mvcJsonOptions.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                _mvcJsonOptions.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                _mvcJsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddAuthentication(options => options.AddScheme<MvcCookieAuthenticationHandler>(TbConstant.WEBSITE_AUTHENTICATION_SCHEME, TbConstant.WEBSITE_AUTHENTICATION_SCHEME));
            
            services.BuildServiceProvider().RegisterServiceProvider();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizedFilter<T>(this IServiceCollection services) where T : class, IAuthorizationFilter
        {
            services.AddTransient<IAuthorizationFilter, T>();
            return services;
        }
        /// <summary>
        /// 创建服务提供者
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider RegisterServiceProvider(this IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new MvcException("IServiceProvider serviceProvider canot be null");
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            return serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    return _httpContextAccessor.HttpContext.RequestServices;
                }
                return _serviceProvider;
            }
        }

        public static HttpContext HttpContext => _httpContextAccessor?.HttpContext;
        public static string ClientIP { get { return HttpContext==null?"::ip":HttpContext.Connection.RemoteIpAddress.ToString(); } }


        public static object New(Type type)
        {
            return ActivatorUtilities.CreateInstance(ServiceProvider, type, Array.Empty<object>());
        }

        public static T New<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(ServiceProvider, Array.Empty<object>());
        }

        public static T Get<T>()
        {
            T val;
            try
            {
                val = ActivatorUtilities.GetServiceOrCreateInstance<T>(ServiceProvider);
            }
            catch (Exception ex)
            {
                try
                {
                    val = ServiceProvider.GetService<T>();
                }
                catch (Exception ex2)
                {
                    try
                    {
                        val = default(T);
                    }
                    catch (Exception ex3)
                    {
                        throw new MvcException($"ex0={ex.Message}; ex1={ex2.Message}; ex2={ex3.Message};");
                    }
                }
            }
            if (val != null)
            {
                return val;
            }
            return default(T);
        }

        public static object Get(Type type)
        {
            try
            {
                return ActivatorUtilities.GetServiceOrCreateInstance(ServiceProvider, type);
            }
            catch
            {
                object service = ServiceProvider.GetService(type);
                if (service == null)
                {
                    return null;
                }
                return service;
            }
        }

        /// <summary>
        /// base 64/256解密
        /// </summary>
        /// <param name="plaintext">密文</param>
        /// <returns></returns>
        public static string Decrypt(string plaintext)
        {
            IDataProtector protector = Protector;
            if (protector is Base256DataProtector)
            {
                return (protector as Base256DataProtector).Unprotect(plaintext, true);
            }
            return protector.Unprotect(plaintext);
        }

        /// <summary>
        /// base64/256加密
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <returns></returns>
        public static string Encrypt(string plaintext)
        {
            IDataProtector protector = Protector;
            if (protector is Base256DataProtector)
            {
                return (protector as Base256DataProtector).Protect(plaintext, true);
            }
            return protector.Protect(plaintext);
        }

        #region autoregisterService
        ///// <summary>
        ///// 添加服务注册扫描
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //public static IServiceCollection AutoRegisteServices(this IServiceCollection services)
        //{
        //    var assemblys = AppDomain.CurrentDomain.GetAssemblies();
        //    services.Scan(scan => scan
        //        .FromAssemblies(assemblys)
        //        .AddClasses(classes => classes.AssignableToAny(typeof(ITransientDependency)))
        //            .AsImplementedInterfaces()
        //            .WithTransientLifetime()
        //        .AddClasses(classes => classes.AssignableToAny(typeof(IScopedDependency)))
        //            .AsImplementedInterfaces()
        //            .WithScopedLifetime()
        //        .AddClasses(classes => classes.AssignableToAny(typeof(ISingletonDependency)))
        //            .AsImplementedInterfaces()
        //            .WithSingletonLifetime()
        //    );
        //    return services;
        //}
        #endregion

        #region addconfigmodel
        //public static IServiceCollection AddConfigModel(this IServiceCollection services)
        //{
        //    var types = AppDomain.CurrentDomain.GetAssemblies()
        //        .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IConfigModel))))
        //        .ToArray();
        //    var ioption = typeof(IOptions<>);
        //    var ioptionIml = typeof(Option<>);
        //    foreach (var type in types)
        //    {
        //        services.AddSingleton(type, provider =>
        //        {
        //            var config = provider.GetService<IConfiguration>().GetSection(type.Name);
        //            return config.Get(type);
        //        });
        //        Type[] typeArgs = { type };
        //        services.AddSingleton(ioption.MakeGenericType(typeArgs), provider =>
        //        {
        //            var config = provider.GetService<IConfiguration>().GetSection(type.Name).Get(type);
        //            return Activator.CreateInstance(ioptionIml.MakeGenericType(typeArgs), config);
        //        });
        //    }
        //    return services;
        //} 
        #endregion

        public static IServiceCollection AddTestIdentityServer(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Auth.Config.GetApiResources())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddInMemoryClients(Auth.Config.GetClients())
                .AddTestUsers(Auth.Config.GetUsers());
            return services;
        }

        public static IServiceCollection AddTestAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = ConfigLocator.Instance["ApplicationUrl"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "banbrickcustomer";
                });
            return services;
        }
    }
}
