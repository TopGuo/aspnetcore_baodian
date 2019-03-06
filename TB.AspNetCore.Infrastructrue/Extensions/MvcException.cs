using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Infrastructrue.Extensions
{
    /// <summary>
    /// 重写excepiton message
    /// </summary>
    public class MvcException : Exception
    {
        private StringBuilder messages = new StringBuilder();

        public override string Message => messages.ToString();

        public MvcException(string message)
            : this(message, null)
        {
        }

        public MvcException(Exception innerException)
            : this(null, innerException)
        {
        }

        public MvcException(string message, Exception innerException)
        {
            if (!string.IsNullOrEmpty(message))
            {
                messages.AppendLine(message);
            }
            for (Exception ex = innerException; ex != null; ex = ex.InnerException)
            {
                messages.AppendLine(ex.Message);
            }
        }
    }
}
