using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models.Responses
{
    public class DeviceListResponse
    {
        public List<Device> Devices { get; set; }

        public class Device
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}
