using OpenIddict.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenSound.API.OpenIddict
{
    public class HSOpenIddictToken : OpenIddictToken<Guid, HSOpenIddictApplication, HSOpenIddictAuthorization>
    {
    }
}
