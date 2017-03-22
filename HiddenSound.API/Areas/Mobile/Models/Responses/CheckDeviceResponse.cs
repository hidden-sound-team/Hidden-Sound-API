using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Mobile.Models.Responses
{
    public class CheckDeviceResponse
    {
        public bool IsUserDevice { get; set; }

        public bool IsDeviceLinked { get; set; }

        public bool UserHasDevice { get; set; }
    }
}
