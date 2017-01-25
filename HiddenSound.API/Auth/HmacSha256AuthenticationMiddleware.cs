using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HiddenSound.API.Auth
{
    public class HmacSha256AuthenticationMiddleware : AuthenticationMiddleware<HmacSha256Options>
    {
        public HmacSha256AuthenticationMiddleware(RequestDelegate next, IOptions<HmacSha256Options> options, ILoggerFactory loggerFactory, UrlEncoder encoder) : base(next, options, loggerFactory, encoder)
        {
        }

        protected override AuthenticationHandler<HmacSha256Options> CreateHandler()
        {
            return new HmacSha256AuthenticationHandler();
        }
    }
}
