using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public interface IServiceInstance
    {
        String Key { get; set; }
        String Token { get; set; }
        String Id { get; set; }
        String Issuer { get;  }
        SymmetricSecurityKey ServiceKey { get; set; }
    }
}
