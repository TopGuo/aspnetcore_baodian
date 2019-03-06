using System;
using System.Collections.Generic;
using System.Text;
using TB.AspNetCore.Domain.Entitys;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Domain.Models.Base;
using TB.AspNetCore.Infrastructrue.Extensions;

namespace TB.AspNetCore.Application.Services
{
    public class SystemSettingService:BaseService
    {
        #region internal savesetting&getsetting
        public void SaveSettings<T>(T model)
           where T : SettingsBase, new()
        {
            if (model == null)
            {
                model = new T();
            }
            SystemSetting settings = this.Single<SystemSetting>(t => t.Name == model.Name);
            if (settings == null)
            {
                settings = new SystemSetting { Id = Guid.NewGuid().ToString("N"), Name = model.Name, Value = model.GetJson(), CreateTime = DateTime.Now };
                this.Add(settings);
            }
            else
            {
                settings.Value = model.GetJson();
                settings.UpdateTime = DateTime.Now;
                this.Update(settings);
            }
            base.Save();
        }
        public void SaveSettings<T>(List<T> models)
            where T : SettingsBase, new()
        {
            var key = new T().Name;
            if (models.Count == 0)
            {
                models = new List<T>();
            }
            SystemSetting settings = this.Single<SystemSetting>(t => t.Name == key);
            if (settings == null)
            {
                settings = new SystemSetting { Id = Guid.NewGuid().ToString("N"), Name = key, Value = models.GetJson(), CreateTime = DateTime.Now };
                this.Add(settings);
            }
            else
            {
                settings.Value = models.GetJson();
                settings.UpdateTime = DateTime.Now;
                this.Update(settings);
            }
            base.Save();
        }

        public T GetSetting<T>() where T : SettingsBase, new()
        {
            var model = new T();
            var settings = this.Single<SystemSetting>(x => x.Name == model.Name);
            if (settings != null)
            {
                model = settings.Value.GetModel<T>();
            }
            return model;
        }

        public List<T> GetSettings<T>() where T : SettingsBase, new()
        {
            var t = new T();
            List<T> list = new List<T>();
            var settings = this.Single<SystemSetting>(x => x.Name == t.Name);
            if (settings != null)
            {
                list = settings.Value.GetModelList<T>();
            }
            return list;
        }
        #endregion

        #region app version
        /// <summary>
        /// create or get android version
        /// </summary>
        /// <returns></returns>
        public static AndroidVersion AndroidVersion
        {
            get
            {
                SystemSettingService service = new SystemSettingService();

                AndroidVersion android = service.GetSetting<AndroidVersion>();

                if (android == null)
                {
                    android = new AndroidVersion { };

                    service.SaveSettings(android);
                }
                return android;
            }
        }

        /// <summary>
        /// create or get ios version
        /// </summary>
        /// <returns></returns>
        public static IosVersion IosVersion
        {
            get
            {
                SystemSettingService service = new SystemSettingService();

                IosVersion ios = service.GetSetting<IosVersion>();

                if (ios == null)
                {
                    ios = new IosVersion { };

                    service.SaveSettings(ios);
                }
                return ios;
            }
        }
        #endregion

        #region system_setting
        protected static SystemSettingService Instance { get { return new SystemSettingService(); } }
        /// <summary>
        /// create or get
        /// </summary>
        /// <returns></returns>
        public static SystemSettingModel SystemSetting
        {
            get
            {
                var setting = Instance.GetSetting<SystemSettingModel>();
                if (setting == null)
                {
                    setting = new SystemSettingModel();
                    Instance.Add(new SystemSetting { Id = setting.GenNewGuid(), Name = setting.Name, Value = setting.GetJson(), CreateTime = DateTime.Now }, true);
                }

                return setting;
            }
        }
        #endregion
    }
}
