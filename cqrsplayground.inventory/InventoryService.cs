using Microsoft.AspNetCore.Mvc;
using Steeltoe.Discovery.Eureka.AppInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.inventory
{
    [Route("inventory")]
    public class InventoryService : ControllerBase, IInventoryService
    {
        private IInventoryService _service;

        public InventoryService(IInventoryService service)
        {
            _service = service;
        }

        public IEnumerable<InstanceInfo> GetRegisteredInstances()
        {
            return _service.GetRegisteredInstances();
        }
    }
}
