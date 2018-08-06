using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class ServiceInstance: IServiceInstance
    {
        public String Key { get; set; }
        public String Token { get; set; }
        public String Id { get; set; }
        public SymmetricSecurityKey ServiceKey { get; set; }
        public string Issuer
        {
            get
            {
                return Key;
            }
        }
    }
}
