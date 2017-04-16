using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.Areas.Application.Models.Responses
{
    public class AuthorizedApplicationListResponse
    {
        public List<AuthorizedApplication> AuthorizedApplications { get; set; }
    }

    public class AuthorizedApplication
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
