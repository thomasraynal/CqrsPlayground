using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class JwtBearerAuthorize : AuthorizeAttribute
    {
        public JwtBearerAuthorize()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }

        public JwtBearerAuthorize(string policy) : base(policy)
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
