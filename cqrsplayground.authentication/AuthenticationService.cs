using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cqrsplayground.authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private List<ServiceIdentity> _identities = new List<ServiceIdentity>()
        {
            new ServiceIdentity()
            {
                ServiceId = "cqrsplayground.trade.service",
                ServiceToken ="__cqrsplayground.trade.service.token__"
            },
            new ServiceIdentity()
            {
                ServiceId = "cqrsplayground.booking.service",
                ServiceToken ="__cqrsplayground.booking.service.token__"
            },
            new ServiceIdentity()
            {
                ServiceId = "cqrsplayground.compliance.service",
                ServiceToken ="__cqrsplayground.compliance.service.token__"
            },
             new ServiceIdentity()
            {
                ServiceId = "cqrsplayground.generator",
                ServiceToken ="__cqrsplayground.generator.token__"
            },
        };

        public Task<ServiceIdentity> Authenticate(String token)
        {
            return Task.FromResult(_identities.FirstOrDefault(identity => identity.ServiceId == token));
        }

        public Task<bool> IsValid(string token)
        {
            return Task.FromResult(_identities.Any(identity => identity.ServiceId == token));
        }
    }
}
