using IdentityModel;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.AspNetCore.Infrastructrue.Auth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public ResourceOwnerPasswordValidator()
        {

        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (ValidateCredentials(context.UserName, context.Password, out TestUser user))
            {
                context.Result = new GrantValidationResult(user.SubjectId, OidcConstants.AuthenticationMethods.Password);
            }

            return Task.FromResult(0);
        }
        private bool ValidateCredentials(string username, string password, out TestUser user)
        {
            user = Config.GetUsers().FirstOrDefault(i => i.Username == username);
            if (user != null)
            {
                return user.Password.Equals(password);
            }

            return false;
        }
    }
}
