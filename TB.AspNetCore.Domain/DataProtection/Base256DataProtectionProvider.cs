using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TB.AspNetCore.Domain.DataProtection
{
    public class Base256DataProtectionProvider : IDataProtectionProvider
    {
        public Base256Options Options
        {
            get;
        }

        public Base256DataProtectionProvider(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                Options = new Base256Options();
            }
            else
            {
                Options = serviceProvider.GetService(typeof(Base256Options)) as Base256Options;
            }
            if (Options != null)
            {
                return;
            }
            throw new NullReferenceException("Base256Options");
        }

        public IDataProtector CreateProtector(string purpose)
        {
            if (string.IsNullOrEmpty(purpose))
            {
                throw new ArgumentNullException("purpose");
            }
            return new Base256DataProtector(this, null, purpose);
        }
    }
}
