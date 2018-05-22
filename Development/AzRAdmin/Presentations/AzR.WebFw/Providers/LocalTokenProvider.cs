using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AzR.WebFw.Providers
{
    public class LocalTokenProvider
    {
        public static JObject GenerateLocalAccessTokenResponse(Dictionary<string, string> claims)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut(Startup.OAuthOptions.AuthenticationType);

            var identity = new ClaimsIdentity(WebFw.Startup.OAuthOptions.AuthenticationType);
            identity.AddClaims(claims.Select(item => new Claim(item.Key, item.Value.ToString())));


            var newClaims = identity
                .Claims
                .Where(s => !s.Type.Contains("http://") && !s.Type.Contains("SecurityStamp"));

            var properties = new AuthenticationProperties(newClaims.ToDictionary(t => t.Type, t => t.Value))
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(WebFw.Startup.OAuthOptions.AuthorizationCodeExpireTimeSpan)
            };

            var ticket = new AuthenticationTicket(identity, properties);
            var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);


            var context = new AuthenticationTokenCreateContext(HttpContext.Current.GetOwinContext(), Startup.OAuthOptions.AccessTokenFormat, ticket);
            Startup.OAuthOptions.RefreshTokenProvider.Create(context);
            properties.Dictionary.Add("refresh_token", context.Token);


            context.Request.Context.Authentication.SignIn(properties, identity);

            //Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var authList = new List<JProperty>
            {
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("refresh_token", ticket.Properties.Dictionary["refresh_token"]),
                new JProperty("expires_in", Startup.OAuthOptions.AuthorizationCodeExpireTimeSpan.TotalSeconds.ToString())
            };

            authList.AddRange(newClaims.Select(s => new JProperty(s.Type, s.Value)).ToList());

            authList.AddRange(new List<JProperty>
            {
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
            });


            var tokenResponse = new JObject(authList.ToArray());

            return tokenResponse;
        }

    }
}
