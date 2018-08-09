using System;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public interface IAuthenticatedClientProvider
    {
        TService GetClientFor<TService>(string consumerKey, String endpoint, string realm);
    }
}