using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AzR.Web.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var guid = Guid.NewGuid().ToString();

            // copy all properties and set the desired lifetime of refresh token  
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)//DateTime.UtcNow.AddYears(1)  
            };
            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            _refreshTokens.TryAdd(guid, refreshTokenTicket);

            // consider storing only the hash of the handle  
            context.SetToken(guid);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            //Get the claim which holds creation date
            //  DateTime creationDate = Convert.ToDateTime(clientid.Claims.Where(c => c.Type == "creationDate").Single().Value);
            //Create a variable holding current time minus 30 seconds(This is how long time you can create new refresh tokens by providing your original refresh token)
            //DateTime now = DateTime.UtcNow.AddSeconds(-30);


            //If the time has passed more than 30 seconds from the time you got your original access and refresh token by providing credentials
            //you may not create and return new refresh tokens(Obviously the 30  seconds could be changed to something less or more aswell)
            //if (now < ceationDate)
            //{
            // Expiration time in seconds
            int expire = 2 * 60;
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(expire));
            context.SetToken(context.SerializeTicket());
            // }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;
            var header = context.OwinContext.Request.Headers["Authorization"];

            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
            }
        }
    }
}