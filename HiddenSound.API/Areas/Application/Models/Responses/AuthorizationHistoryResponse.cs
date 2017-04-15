using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models.Responses
{
    public class AuthorizationHistoryResponse
    {
        public List<Authorization> Authorizations { get; set; }

        public class Authorization
        {
            public string AppName { get; set; }

            public DateTime CreatedOn { get; set; }

            public string Status { get; set; }
        }
    }
}
