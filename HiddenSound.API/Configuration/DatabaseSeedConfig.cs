using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Configuration
{
    public class DatabaseSeedConfig
    {
        public string AdminUsername { get; set; }
        
        public string AdminPassword { get; set; }

        public string ApplicationPublicClientId { get; set; }

        public string ApplicationConfidentialClientId { get; set; }

        public string ApplicationConfidentialClientSecret { get; set; }

        public string ApplicationRedirectUri { get; set; }

        public string VendorClientId { get; set; }

        public string VendorClientSecret { get; set; }

        public string VendorRedirectUri { get; set; }

        public string VendorUsername { get; set; }

        public string VendorPassword { get; set; }
    }
}
