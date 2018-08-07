using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly Func<Task<ServiceToken>> getToken;
        private ServiceToken _current;

        public AuthenticatedHttpClientHandler(Func<Task<ServiceToken>> getToken)
        {
            this.getToken = getToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = request.Headers.Authorization;
            if (null == _current || _current.HasExpired)
            {
                _current = await getToken();
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, _current.Token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
