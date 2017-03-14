using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;

namespace HiddenSound.API.Areas.Shared.Repositories
{
    public interface IAuthorizationRepository
    {
        List<Authorization> GetAllAuthorizations();

        Authorization GetAuthorization(string code);

        void UpdateAuthorization(Authorization authorization);

        void CreateAuthorization(Authorization authorization);
    }
}
