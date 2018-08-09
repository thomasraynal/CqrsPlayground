using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Steeltoe.Common.Discovery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.gateway
{
    public class ProxyService
    {
        public ProxyService(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);

            Client = new HttpClient(_handler, false);
        }

        private DiscoveryHttpClientHandler _handler;

        public HttpClient Client { get; private set; }
    }

    public static class ProxyExtensions
    {
        public static IServiceCollection AddProxy(this IServiceCollection services)
        {
            return services.AddSingleton<ProxyService>();
        }

        public static void RunProxy(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProxyMiddleware>();
        }
    }

    public class ProxyMiddleware
    {

        public const String realmHeader = "Realm";

        private readonly RequestDelegate _next;
        
        private Uri MapUri(HttpContext context)
        {
            var uri = new Uri(UriHelper.GetDisplayUrl(context.Request));
            var forwardedUrl = uri.AbsoluteUri.Replace(uri.Authority, context.Request.Headers[realmHeader]);
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

    public static class ProxyMiddlewareUtilities
    {
        private const int StreamCopyBufferSize = 81920;

        public static async Task ProxyRequest(this HttpContext context, Uri destinationUri)
        {
            var proxyService = context.RequestServices.GetRequiredService<ProxyService>();

            using (var requestMessage = context.CreateProxyHttpRequest(destinationUri))
            {
                using (var responseMessage = await context.SendProxyHttpRequest(requestMessage))
                {
                    await context.CopyProxyHttpResponse(responseMessage);
                }
            }
        }
        
        public static HttpRequestMessage CreateProxyHttpRequest(this HttpContext context, Uri uri)
        {

            var request = context.Request;

            var requestMessage = new HttpRequestMessage();
            var requestMethod = request.Method;
            if (!HttpMethods.IsGet(requestMethod) &&
                !HttpMethods.IsHead(requestMethod) &&
                !HttpMethods.IsDelete(requestMethod) &&
                !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(request.Body);
                requestMessage.Content = streamContent;
            }

            foreach (var header in request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()) && requestMessage.Content != null)
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            requestMessage.Headers.Host = uri.Authority;
            requestMessage.RequestUri = uri;
            requestMessage.Method = new HttpMethod(request.Method);

            return requestMessage;
        }

        public static Task<HttpResponseMessage> SendProxyHttpRequest(this HttpContext context, HttpRequestMessage requestMessage)
        {
            var proxyService = context.RequestServices.GetRequiredService<ProxyService>();

            return proxyService.Client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
        }

        public static async Task CopyProxyHttpResponse(this HttpContext context, HttpResponseMessage responseMessage)
        {
            var response = context.Response;

            response.StatusCode = (int)responseMessage.StatusCode;
            foreach (var header in responseMessage.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            response.Headers.Remove("transfer-encoding");

            using (var responseStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                await responseStream.CopyToAsync(response.Body, StreamCopyBufferSize, context.RequestAborted);
            }
        }    
    }
}