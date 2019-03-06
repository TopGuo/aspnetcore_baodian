using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TB.AspNetCore.Infrastructrue.Extensions;

namespace TB.AspNetCore.Application.Action
{
    /// <summary>
    /// 父菜单
    /// </summary>
    public class ParentAtions
    {
        readonly string guid = Guid.Empty.ToString();
        public ParentAtions(ActionType actionType)
        {
            ActionType = actionType;
        }
        public ParentAtions(ActionType actionType, string url)
        {
            ActionType = actionType;
            this.Url = url;
        }
        /// <summary>
        /// 编号-重组一个和菜单相关的guid
        /// </summary>
        public string Id
        {
            get
            {
                var id = ((int)ActionType).ToString();
                return Guid.Parse($"{guid.Substring(0, guid.Length - id.Length)}{id}").ToString();
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get { return ActionType.GetString(); } }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; } = "";
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get { return (int)ActionType; } }

        static List<ParentAtions> parentActions = new List<ParentAtions>();
        internal ActionType ActionType { get; }

        /// <summary>
        /// 获取或初始化父菜单
        /// </summary>
        public static List<ParentAtions> ParentAtionsList
        {
            get
            {
                if (parentActions.Count == 0)
                {
                    foreach (var type in JsonExtensions.GetStrings<ActionType>())
                    {
                        parentActions.Add(new ParentAtions((ActionType)type.Item1));
                    }
                }
                return parentActions;
            }
        }

    }
    /// <summary>
    /// 父菜单类型
    /// </summary>
    public enum ActionType
    {
        [Description("系统管理")]
        SystemManager = 1,
        [Description("用户管理")]
        UsersManager,
        [Description("财务管理")]
        CaiwuManager,
        [Description("统计分析")]
        TongJi,
        /// <summary>
        /// 内容管理
        /// </summary>
        [Description("内容管理")]
        ContentManager,
        [Description("测试杂谈")]
        Test,
        
    }

    /// <summary>
    /// action特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ActionAttribute : Attribute
    {
        public ActionAttribute(string name, ActionType parent, int order = 0)
        {
            this.Name = name;
            this.Parent = parent;
            this.Order = order;
        }

        public string Name { get; }
        public ActionType Parent { get; }
        public int Order { get; }
        public string ParentId
        {
            get
            {
                var p = ParentAtions.ParentAtionsList.SingleOrDefault(t => t.ActionType == Parent);
                if (p == null)
                {
                    return null;
                }
                return p.Id;
            }
        }
        public string ParentName
        {
            get
            {
                var p = ParentAtions.ParentAtionsList.SingleOrDefault(t => t.ActionType == Parent);
                if (p == null)
                {
                    return string.Empty;
                }
                return p.Name;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MenuActionAttribute : Attribute
    {
        public MenuActionAttribute(string parentName)
        {
            ParentName = parentName;
        }
        public string ParentName { get; }
    }
}
