using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Claims;
using Thinktecture.IdentityModel.Extensions;
using Raven.Client;
using Blog.Model;

namespace Blog.Web.App.Security
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        public ClaimsTransformer()
        {
        }

        public override IClaimsPrincipal Authenticate(string resourceName, IClaimsPrincipal incomingPrincipal)
        {
            // do nothing if anonymous request
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }

            using (var dbSession = (System.Web.Mvc.DependencyResolver.Current.GetService(
                    typeof(Blog.Web.App.Raven.IRavenSessionFactoryBuilder)) as Blog.Web.App.Raven.IRavenSessionFactoryBuilder)
                    .GetSessionFactory().CreateSession())
            {
                string uniqueId = GetUniqueId(incomingPrincipal);

                // check if user is registered
                var data = dbSession.Query<UserData>().FirstOrDefault(u => u.Ids.Keys.Any(i => i == uniqueId));
                if (data == null)
                {
                    data = new UserData();
                    data.Ids.Add(uniqueId, ToSimpleClaim(incomingPrincipal.Identities[0].Claims).ToList());
                    dbSession.Store(data);
                }
                else
                {
                    // sync claims
                }
                dbSession.SaveChanges();

                //return CreateUserPrincipal(uniqueId, data);


                //// authenticated by ACS, but not registered
                //// create unique id claim
                //incomingPrincipal.Identities[0].Claims.Add(new Claim(Constants.ClaimTypes.Id, uniqueId));
                //return incomingPrincipal;

                return base.Authenticate(resourceName, incomingPrincipal);
            }
        }

        private IEnumerable<SimpleClaim> ToSimpleClaim(ClaimCollection claimCollection)
        {
            return claimCollection.Select(c => new SimpleClaim
            {
                ClaimType = c.ClaimType,
                Issuer = c.Issuer,
                Value = c.Value,
            });
        }


        //public static void IssueSessionToken(string uniqueId, UserData data)
        //{
        //    var principal = CreateUserPrincipal(uniqueId, data);
        //    var sessionToken = new Microsoft.IdentityModel.Tokens.SessionSecurityToken(principal);

        //    Microsoft.IdentityModel.Web.FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionToken);
        //}

        private static IClaimsPrincipal CreateUserPrincipal(string uniqueId, UserData data)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, uniqueId),
            };
            claims.Add(new Claim(ClaimTypes.Role, data.Registerd ? "Registerd" : "NotRegisterd"));

            if (!string.IsNullOrEmpty(data.Name))
                claims.Add(new Claim(ClaimTypes.Name, data.Name));
            if (!string.IsNullOrEmpty(data.Email))
                claims.Add(new Claim(ClaimTypes.Email, data.Email));

            var id = new ClaimsIdentity(claims);
            return ClaimsPrincipal.CreateFromIdentity(id);
        }

        private static string GetUniqueId(IClaimsPrincipal incomingPrincipal)
        {
            // create unique id claim

            var nameId = incomingPrincipal.FindClaims(claim => claim.ClaimType == ClaimTypes.Name || claim.ClaimType == ClaimTypes.NameIdentifier).FirstOrDefault();
            var idp = incomingPrincipal.FindClaims(Constants.ClaimTypes.IdP).FirstOrDefault();

            if (idp != null && nameId != null)
            {
                return string.Format("{0}\\{1}", idp.Value, nameId.Value);
            }
            else
            {
                throw new Exception("No Unique ID");
            }
        }
    }
}