using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TB.AspNetCore.Domain.Config
{
    public class ResponsResult
    {
        private class Exceptions
        {
            public string Type
            {
                get;
                set;
            }

            public string Message
            {
                get;
                set;
            }

            public string StackTrace
            {
                get;
                set;
            }
        }

        protected const int SUCCESS_STATUS = 200;

        protected const int SYSTEM_ERROR_STATUS = 500;

        protected const int CUSTOMER_ERROR_STATUS = 501;

        protected static string _msg = "Successful!";

        private object defaultData;

        private string _message;

        [Description("e.g. 200:success; 500:system error; 404:not found; 401:Unauthorized ")]
        public int Status
        {
            get;
            set;
        } = 200;


        [Description("response extend data")]
        public object Data
        {
            get
            {
                if (!Success)
                {
                    defaultData = null;
                }
                return defaultData;
            }
            set
            {
                defaultData = value;
            }
        }

        [Description("response message")]
        public string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_message))
                {
                    return _msg;
                }
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        [Description("record count for paging")]
        public int? RecordCount
        {
            get;
            set;
        }

        [Description("for paging the record's page count")]
        public int? PageCount
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool Success
        {
            get
            {
                return Status == 200;
            }
        }

        public static void SetDefaultMessage(string message)
        {
            _msg = message;
        }

        public ResponsResult()
        {
        }

        public ResponsResult(object data)
        {
            defaultData = data;
        }

        public ResponsResult(int status, string message)
        {
            Status = status;
            _message = message;
        }

        public ResponsResult(Enum status, string message = "")
        {
            SetStatus(status, message);
        }

        public ResponsResult(Exception exception, bool showStackTrace = false)
        {
            SetStatus(exception, showStackTrace);
        }

        public virtual ResponsResult SetError(string errorMessage)
        {
            Status = 501;
            _message = errorMessage;
            return this;
        }

        public ResponsResult(int status, string message, Exception ex = null)
        {
            Status = status;
            if (ex != null)
            {
                Error(ex, false);
            }
            if (!string.IsNullOrEmpty(message))
            {
                Message = message;
            }
        }

        public ResponsResult Error(Exception exception, bool showStackTrace = false)
        {
            if (exception == null)
            {
                throw new NullReferenceException("exception canot be null");
            }
            Data = null;
            if (Status == 200)
            {
                Status = 500;
            }
            StringBuilder val = new StringBuilder();
            while (exception != null)
            {
                val.Insert(0, exception.GetType().Name + ":" + exception.Message + Environment.NewLine);
                if (showStackTrace)
                {
                    val.AppendLine(exception.StackTrace);
                }
                exception = exception.InnerException;
            }
            Message = ((object)val).ToString();
            return this;
        }

        public virtual ResponsResult SetStatus(Exception exception, bool showStackTrace = false)
        {
            Status = 500;
            StringBuilder stringBuilder = new StringBuilder();
            while (exception != null)
            {
                stringBuilder.Insert(0, $"{exception.GetType().Name}:{exception.Message}{Environment.NewLine}");
                if (showStackTrace)
                {
                    stringBuilder.AppendLine(exception.StackTrace);
                }
                exception = exception.InnerException;
            }
            Message = stringBuilder.ToString();
            return this;
        }

        public virtual ResponsResult SetStatus(Enum status, string message = "")
        {
            Status = Convert.ToInt32(status);
            if (string.IsNullOrEmpty(message))
            {
                _message = GetString(status);
            }
            else
            {
                _message = message;
            }
            return this;
        }

        public virtual ResponsResult SetStatus(int status, string message)
        {
            Data = defaultData;
            Status = status;
            _message = message;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        private string GetString(Enum _enum)
        {
            if (_enum == null)
            {
                return string.Empty;
            }
            Type type = _enum.GetType();
            string name = Enum.GetName(type, _enum);
            if (!string.IsNullOrEmpty(name))
            {
                DescriptionAttribute customAttribute = (type.GetField(name)).GetCustomAttribute<DescriptionAttribute>();
                if (customAttribute != null)
                {
                    return customAttribute.Description;
                }
                return name;
            }
            List<FieldInfo> list = (from t in type.GetFields()
                                    where t.FieldType == type
                                    select t).ToList();
            List<string> description = new List<string>();
            list.ForEach(delegate (FieldInfo t)
            {
                if (_enum.HasFlag((Enum)t.GetValue(_enum)))
                {
                    DescriptionAttribute customAttribute2 = ((MemberInfo)t).GetCustomAttribute<DescriptionAttribute>();
                    if (customAttribute2 != null && !string.IsNullOrEmpty(customAttribute2.Description))
                    {
                        description.Add(customAttribute2.Description);
                    }
                    else
                    {
                        description.Add(t.Name);
                    }
                }
            });
            return string.Join(",", description);
        }
    }
}
