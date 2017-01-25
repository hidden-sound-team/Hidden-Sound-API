using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace HiddenSound.API.Auth
{
    public class HmacSha256AuthenticationHandler : AuthenticationHandler<HmacSha256Options>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
