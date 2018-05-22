using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AzR.WebFw.Handlers
{
    public class ContextItemHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var guid = Guid.NewGuid();
            HttpContext.Current.Items["AzRADMINUSER"] = guid;

            // An Async operation
            var result = await base.SendAsync(request, cancellationToken);

            //All code from this point is not gauranteed to run on the same thread that started the handler

            var restoredGuid = (Guid)HttpContext.Current.Items["AzRADMINUSER"];

            //Is this gauranteed to be true
            var areTheSame = guid == restoredGuid;

            return result;
        }
    }
}