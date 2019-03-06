using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using TB.AspNetCore.Application.Services;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Contexts;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Middleware.ErrorHandlerMiddleware;

namespace TB.AspNetCore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "爆点API接口文档",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Name = "鸟窝",
                        Email = "topbrids@gmail.com",
                        Url = "http://www.cnblogs.com/gdsblog"
                    }
                });
                c.IncludeXmlComments(XmlCommentsFilePath);
                c.IncludeXmlComments(XmlDomianCommentsFilePath);
            });

            services.AddApiDoc(t =>
            {
                t.ApiDocPath = "apidoc";//api访问路径
                t.Title = "爆点";//文档名称
            });

            //注入application service
            services.AddScoped<IRpwService, RpwService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IMemberService, MemberService>();

            services.AddAutoMapper(); // Adding automapper
            var sqlStr = Configuration["MySqlDbConnection"];
            services.AddDbContextPool<BaoDianContext>(
                options => options.UseMySql(sqlStr,
                    mysqlOptions =>
                    {
                        mysqlOptions.ServerVersion(new Version(5, 7, 24), ServerType.MySql); // replace with your Server Version and Type
                    }
            ));
            services.AddMvcCustomer();
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = ApplicationEnvironment.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        static string XmlDomianCommentsFilePath
        {
            get
            {
                var basePath = ApplicationEnvironment.ApplicationBasePath;
                var fileName = typeof(TbConstant).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            //new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot"))
            //}
            //错误消息处理中间件
            app.UseErrorHandlerMiddleware();
            app.UseCors(t =>
            {
                t.WithMethods("POST", "PUT", "GET");
                t.WithHeaders("X-Requested-With", "Content-Type", "User-Agent");
                t.WithOrigins("*");
            });
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "爆点");
            });

            app.UseMvc();
        }
    }
}
