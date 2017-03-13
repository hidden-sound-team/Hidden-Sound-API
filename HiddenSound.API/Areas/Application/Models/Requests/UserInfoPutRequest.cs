using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models.Requests
{
    public class UserInfoPutRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
