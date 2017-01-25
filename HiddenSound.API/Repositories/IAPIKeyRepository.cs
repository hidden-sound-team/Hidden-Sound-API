using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.API.Models;

namespace HiddenSound.API.Repositories
{
    public interface IAPIKeyRepository
    {
        List<APIKey> GetAPIKeys();
    }
}
