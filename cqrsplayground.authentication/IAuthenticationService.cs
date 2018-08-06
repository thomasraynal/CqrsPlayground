using System;
using System.Threading.Tasks;

namespace cqrsplayground.authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceIdentity> Authenticate(String token);
        Task<bool> IsValid(String token);
    }
}