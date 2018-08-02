using Steeltoe.Discovery.Eureka.AppInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.inventory
{
    public interface IInventoryService
    {
        IEnumerable<InstanceInfo> GetRegisteredInstances();
    }
}
