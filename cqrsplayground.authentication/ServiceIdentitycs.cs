using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.authentication
{
    public class ServiceIdentity
    {
        public String ServiceId { get; set; }
        public String ServiceToken { get; set; }
    }
}
