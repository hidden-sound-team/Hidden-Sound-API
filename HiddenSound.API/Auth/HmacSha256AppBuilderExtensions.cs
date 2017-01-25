using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace HiddenSound.API.Auth
{
    public static class HmacSha256AppBuilderExtensions
    {
        public static IApplicationBuilder UseHmacSha256Authentication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HmacSha256AuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseHmacSha256Authentication(this IApplicationBuilder app, Action<HmacSha256Options> options)
        {
            var newOptions = new HmacSha256Options();
            options.Invoke(newOptions);
            return app.UseMiddleware<HmacSha256AuthenticationMiddleware>(Options.Create(newOptions));
        }
    }
}
