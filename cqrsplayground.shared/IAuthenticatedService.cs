using System;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public interface IAuthenticatedService
    {
        Task<ServiceToken> GetServiceToken(String consumerServiceKey);
    }
}