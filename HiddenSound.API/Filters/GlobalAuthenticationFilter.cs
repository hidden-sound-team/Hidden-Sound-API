using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HiddenSound.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace HiddenSound.API.Filters
{
    public class GlobalAuthenticationFilter : IAuthorizationFilter
    {
        public IAPIKeyRepository APIKeyRepository { get; set; }

        public GlobalAuthenticationFilter(IAPIKeyRepository apiKeyRepository)
        {
            APIKeyRepository = apiKeyRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            StringValues header;
            if (request.Headers.TryGetValue("Authorization", out header) && header.Count > 0)
            {
                var headerSplit = header[0].Split(' ');

                if (!string.IsNullOrEmpty(headerSplit[0]) && headerSplit.Length == 2)
                {
                    var scheme = headerSplit[0];
                    var apiKeySplit = headerSplit[1].Split(':');

                    if (apiKeySplit.Length == 2)
                    {
                        var publicKey = apiKeySplit[0];
                        var privateKeyHash = apiKeySplit[1];

                    }
                }

            }

            // context.Result = new UnauthorizedResult();
        }
    }
}
