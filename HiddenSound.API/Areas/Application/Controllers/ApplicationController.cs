using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Context;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Area("Application")]
    public abstract class ApplicationController : Controller
    {
        protected ActionResult ModelStateError()
        {
            var response = new ErrorResponse();

            foreach (var errorKv in ModelState)
            {
                foreach (var error in errorKv.Value.Errors)
                {
                    if (response.Errors.ContainsKey(errorKv.Key))
                    {
                        response.Errors[errorKv.Key].Add(error.ErrorMessage);
                    }
                    else
                    {
                        response.Errors.Add(errorKv.Key, new List<string>() { error.ErrorMessage });
                    }
                }
            }

            return BadRequest(response);
        }

        protected ActionResult BadRequest(string errorKey, string errorValue)
        {
            var response = new ErrorResponse();
            response.Errors.Add(errorKey, new List<string>() { errorValue });

            return BadRequest(response);
        }
    }
}
