using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Configuration
{
    public class AppSettingsConfig
    {
        public string WebUri { get; set; }

        public string ApiUri { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
