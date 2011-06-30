using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;
using Thinktecture.IdentityModel.Extensions;

namespace Blog.Web.App.Security
{
    public class RegisteredUsersOnly : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isRegistered = false;

            var id = httpContext.User.Identity as IClaimsIdentity;
            var nameId = id.FindClaims(ClaimTypes.NameIdentifier).FirstOrDefault();

            if (nameId != null && nameId.Issuer.Equals("Local Authority", StringComparison.OrdinalIgnoreCase))
            {
                isRegistered = true;
            }

            return isRegistered;
        }
    }
}