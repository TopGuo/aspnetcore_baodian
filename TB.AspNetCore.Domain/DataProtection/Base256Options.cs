using System;
using System.IO;

namespace TB.AspNetCore.Domain.DataProtection
{
    public class Base256Options
    {
        private DirectoryInfo _di = new DirectoryInfo(Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "ASP.NET\\DataProtection-Keys"));

        public DirectoryInfo KeyDirectory
        {
            get
            {
                if (!_di.Exists)
                {
                    _di.Create();
                }
                return _di;
            }
            set
            {
                _di = value;
            }
        }

        /// <summary>
        /// default to 90 days
        /// </summary>
        public TimeSpan Expires
        {
            get;
            set;
        } = new TimeSpan(90, 0, 0, 0);


        public bool CreateKey
        {
            get;
            set;
        }
    }
}
