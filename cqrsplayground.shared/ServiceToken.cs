using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class ServiceToken
    {
        public String Service { get; set; }
        public String Token { get; set; }
        public long Expiration { get; set; }
        public String InstanceId { get; set; }

        public bool HasExpired
        {
            get
            {
                return Expiration <= DateTime.Now.Ticks;
            }
        }
    }
}
