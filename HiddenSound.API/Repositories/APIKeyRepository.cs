using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.API.Models;

namespace HiddenSound.API.Repositories
{
    public class APIKeyRepository : IAPIKeyRepository
    {
        public HiddenSoundDbContext DbContext { get; set; }

        public List<APIKey> GetAPIKeys()
        {
            var keys = DbContext.APIKey.ToList();
            return keys;
        }
    }
}
