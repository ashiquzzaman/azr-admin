using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace AzR.Web.Providers
{
    public static class CustomClaimProvider
    {
        public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return;

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        }
        public static void UpdateClaim(this IPrincipal currentPrincipal, Dictionary<string, object> dictionary)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            // identity = new ClaimsIdentity(currentPrincipal.Identity);
            if (identity == null) return;
            foreach (var item in dictionary)
            {
                var existingClaim = identity.FindFirst(item.Key);
                if (existingClaim == null) continue;
                identity.RemoveClaim(existingClaim);
                identity.AddClaim(new Claim(item.Key, item.Value.ToString()));

            }

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                new ClaimsPrincipal(new ClaimsIdentity(identity))
                , new AuthenticationProperties
                {
                    IsPersistent = true,
                });
            ////authenticationManager.SignOut(identity.AuthenticationType);
            ////authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, new ClaimsIdentity(identity));
        }

        public static void UpdateClaim(this IPrincipal currentPrincipal, string key, string value)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return;

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim == null) return;
            identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            authenticationManager.AuthenticationResponseGrant =
                new AuthenticationResponseGrant(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true }
                );
        }

        public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim.Value;
        }
    }
}
