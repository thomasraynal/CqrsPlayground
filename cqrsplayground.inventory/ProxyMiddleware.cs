using cqrsplayground.shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;

namespace cqrsplayground.gateway
{
    public class ProxyMiddleware
    {
        private RequestDelegate _next;

        private Uri MapUri(HttpContext context)
        {
            var uri = new Uri(UriHelper.GetDisplayUrl(context.Request));
            var forwardedUrl = uri.AbsoluteUri.Replace(uri.Authority, context.Request.Headers[ServiceConstants.ServiceRealmHeader]);
            return new Uri(forwardedUrl);
        }

        public ProxyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var uri = MapUri(context);
            return context.ProxyRequest(uri);
        }
    }
}
