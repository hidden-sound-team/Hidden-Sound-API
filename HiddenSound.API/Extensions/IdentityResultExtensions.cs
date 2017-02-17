using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HiddenSound.API.Extensions
{
    public static class IdentityResultExtensions
    {
        public static void AddErrors(this IdentityResult result, ModelStateDictionary modelStateDictionary)
        {
            foreach (var error in result.Errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }
        }
    }
}
