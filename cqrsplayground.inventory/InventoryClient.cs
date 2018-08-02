using Microsoft.Extensions.Configuration;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.Eureka.AppInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.inventory
{
    public class InventoryClient : IInventoryService
    {
        private EurekaDiscoveryClient _client;

        public InventoryClient(IConfiguration configuration, EurekaDiscoveryClient client)
        {
            _client = client;
        }

        public IEnumerable<InstanceInfo> GetRegisteredInstances()
        {
            return _client.Applications
                            .GetRegisteredApplications()
                            .SelectMany(app => app.Instances)
                            .AsEnumerable();
        }
    }

}
